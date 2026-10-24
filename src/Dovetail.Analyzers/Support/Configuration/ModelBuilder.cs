using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dovetail.Support.Configuration;

/// <summary>
///     Turns a <c>[DovetailConfiguration]</c> declaration into a <see cref="ConfigurationSection" />, describing what configuration
///     binding can fill: settable, init-only and constructor-bound properties, plus get-only collections and objects it binds
///     in place.
/// </summary>
internal sealed class ModelBuilder
{
    private const int MaxDepth = 16;
    private const string DataAnnotations = "System.ComponentModel.DataAnnotations.";
    private static readonly string[] SectionSuffixes = ["Configuration", "Settings", "Options", "Config"];

    // .NET-only regular expression syntax that ECMA-262 (what JSON Schema uses) doesn't understand.
    private static readonly string[] NonPortableRegex = ["(?i", "(?m", "(?s", "(?n", "(?x", "(?-", "(?>", "(?#", "(?(", "(?'", "\\A", "\\Z", "\\z", "\\G", "\\p{", "\\P{"];

    private readonly Location? _declarationLocation;
    private readonly CancellationToken _cancellationToken;
    private readonly List<DiagnosticInfo> _diagnostics = [];
    // A collection expression can't take the comparer here: the analyzer also builds against Roslyn 5.0 (C# 14).
#pragma warning disable IDE0028
    private readonly HashSet<ITypeSymbol> _visiting = new(SymbolEqualityComparer.Default);
    private readonly HashSet<ITypeSymbol> _reportedRecursion = new(SymbolEqualityComparer.Default);
#pragma warning restore IDE0028

    private ModelBuilder(Location? declarationLocation, CancellationToken cancellationToken)
    {
        _declarationLocation = declarationLocation;
        _cancellationToken = cancellationToken;
    }

    /// <summary>Builds the section for one <c>[DovetailConfiguration]</c> attribute.</summary>
    /// <param name="attribute">The attribute.</param>
    /// <param name="attributedType">The class it's on, or <see langword="null" /> on the assembly.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static ConfigurationDeclaration Build(AttributeData attribute, INamedTypeSymbol? attributedType, CancellationToken cancellationToken)
    {
        string? section = null;
        ITypeSymbol? type = attributedType;
        foreach (var argument in attribute.ConstructorArguments)
        {
            if (argument is { Kind: TypedConstantKind.Primitive, Value: string value })
            {
                section = value;
            }
            else if (argument is { Kind: TypedConstantKind.Type, Value: ITypeSymbol typeArgument })
            {
                type = typeArgument;
            }
        }

        var order = 0;
        var registerOptions = true;
        var validateOnStart = false;
        foreach (var named in attribute.NamedArguments)
        {
            switch (named.Key)
            {
                case "Order" when named.Value.Value is int value:
                    order = value;
                    break;
                case "RegisterOptions" when named.Value.Value is bool value:
                    registerOptions = value;
                    break;
                case "ValidateOnStart" when named.Value.Value is bool value:
                    validateOnStart = value;
                    break;
                default:
                    break;
            }
        }

        var builder = new ModelBuilder(attribute.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation(), cancellationToken);
        var result = builder.BuildSection(type, section, order, registerOptions, validateOnStart);
        return new(result, builder._diagnostics.ToEquatableArray());
    }

    /// <summary>The default section name: the type name without an Options/Settings/Configuration/Config suffix.</summary>
    public static string DefaultSectionName(string typeName)
    {
        foreach (var suffix in SectionSuffixes)
        {
            if (typeName.EndsWith(suffix, StringComparison.Ordinal))
            {
                return typeName[..^suffix.Length];
            }
        }

        return typeName;
    }

    private ConfigurationSection? BuildSection(ITypeSymbol? type, string? section, int order, bool registerOptions, bool validateOnStart)
    {
        if (type is null or { TypeKind: TypeKind.Error })
        {
            return null;
        }

        var typeName = type.ToDisplayString();
        if (InvalidTypeReason(type) is { } reason)
        {
            Report(Diagnostics.InvalidConfigurationType, _declarationLocation ?? LocationOf(type), typeName, reason);
            return null;
        }

        var path = section ?? DefaultSectionName(type.Name);
        if (InvalidSectionReason(path, section is null) is { } sectionReason)
        {
            Report(Diagnostics.InvalidConfigurationSection, _declarationLocation ?? LocationOf(type), path, typeName, sectionReason);
            return null;
        }

        var schema = BuildObject((INamedTypeSymbol)type, 0, null, out _);
        return schema is null
            ? null
            : new(path, order, registerOptions, validateOnStart, type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), schema);
    }

    private static string? InvalidTypeReason(ITypeSymbol type) => type switch
    {
        { TypeKind: TypeKind.Interface } => "it's an interface",
        { IsStatic: true } => "it's static",
        { IsAbstract: true } => "it's abstract",
        not INamedTypeSymbol { TypeKind: TypeKind.Class } => "it isn't a class",
        INamedTypeSymbol named when named.IsUnboundGenericType || named.TypeArguments.Any(z => z is ITypeParameterSymbol) => "it's an open generic type",
        _ => null,
    };

    private static string? InvalidSectionReason(string path, bool derived) =>
        derived && path.Length == 0
            ? "the type name is only a suffix, so pass a section path"
            : path.Trim().Length == 0
                ? "section paths can't be empty"
                : path.Split(':').Any(z => z.Trim().Length == 0)
                    ? "section paths can't have empty segments"
                    : null;

    /// <summary>
    ///     The schema of an object type, or <see langword="null" /> when it can't be created from configuration (reported, and
    ///     <paramref name="reported" /> set).
    /// </summary>
    private SchemaNode? BuildObject(INamedTypeSymbol type, int depth, ISymbol? viaMember, out bool reported)
    {
        reported = false;
        if (!_visiting.Add(type))
        {
            if (viaMember is not null && _reportedRecursion.Add(type))
            {
                Report(Diagnostics.RecursiveConfigurationType, LocationOf(viaMember), type.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString(), MemberName(viaMember));
            }

            return new() { Kind = SchemaKind.Object, Truncated = true };
        }

        try
        {
            if (depth > MaxDepth)
            {
                return new() { Kind = SchemaKind.Object, Truncated = true };
            }

            if (!TryResolveConstructor(type, viaMember, out var constructor))
            {
                reported = true;
                return null;
            }

            var properties = new List<SchemaProperty>();
            var keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var member in BindableMembers(type, constructor))
            {
                _cancellationToken.ThrowIfCancellationRequested();
                var node = BuildMember(member, depth + 1);
                if (node is null)
                {
                    continue;
                }

                if (keys.TryGetValue(member.Key, out var existing))
                {
                    Report(Diagnostics.ConfigurationKeyCollision, LocationOf(member.Property), existing, MemberName(member.Property), member.Key);
                    continue;
                }

                keys[member.Key] = MemberName(member.Property);
                properties.Add(new(member.Key, node, member.Required));
            }

            return Describe(new SchemaNode { Kind = SchemaKind.Object, Properties = properties.ToEquatableArray() }, type, null);
        }
        finally
        {
            _visiting.Remove(type);
        }
    }

    /// <summary>
    ///     The constructor whose parameters bind from configuration, or <see langword="null" /> when properties are set after a
    ///     parameterless one. Returns <see langword="false" /> (reported) when the type can't be created.
    /// </summary>
    private bool TryResolveConstructor(INamedTypeSymbol type, ISymbol? viaMember, out IMethodSymbol? constructor)
    {
        constructor = null;
        if (type.IsValueType)
        {
            return true;
        }

        var constructors = type.InstanceConstructors.Where(z => z.DeclaredAccessibility == Accessibility.Public).ToArray();
        if (constructors.Any(z => z.Parameters.IsEmpty))
        {
            return true;
        }

        var location = viaMember is null ? _declarationLocation ?? LocationOf(type) : LocationOf(viaMember);
        var parameterized = constructors.Where(z => !z.Parameters.IsEmpty).ToArray();
        if (parameterized.Length != 1)
        {
            Report(
                Diagnostics.UnsupportedConfigurationConstructor,
                location,
                type.ToDisplayString(),
                parameterized.Length == 0 ? "it has no public constructor" : "it has several public constructors with parameters and no parameterless one"
            );
            return false;
        }

        var properties = AllProperties(type);
        foreach (var parameter in parameterized[0].Parameters)
        {
            if (!properties.Any(z => string.Equals(z.Name, parameter.Name, StringComparison.OrdinalIgnoreCase)))
            {
                Report(Diagnostics.UnsupportedConfigurationConstructor, location, type.ToDisplayString(), $"constructor parameter '{parameter.Name}' has no matching property");
                return false;
            }
        }

        constructor = parameterized[0];
        return true;
    }

    private readonly record struct Member(string Key, IPropertySymbol Property, IParameterSymbol? Parameter, bool Required);

    private static IEnumerable<Member> BindableMembers(INamedTypeSymbol type, IMethodSymbol? constructor)
    {
        foreach (var property in AllProperties(type))
        {
            var parameter = constructor?.Parameters.FirstOrDefault(z => string.Equals(z.Name, property.Name, StringComparison.OrdinalIgnoreCase));
            var bindable = parameter is not null
             || property.SetMethod is { DeclaredAccessibility: Accessibility.Public }
             || IsBoundInPlace(property.Type);
            if (!bindable)
            {
                continue;
            }

            var key = ConfigurationKeyName(property) ?? property.Name;
            var required = property.IsRequired
             || HasAttribute(property, DataAnnotations + "RequiredAttribute")
             || ( parameter is { HasExplicitDefaultValue: false } && parameter.Type is { IsValueType: true } && parameter.Type.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T );
            yield return new(key, property, parameter, required);
        }
    }

    /// <summary>Public instance properties with a public getter, base types first; a redeclared property keeps its base position.</summary>
    private static List<IPropertySymbol> AllProperties(INamedTypeSymbol type)
    {
        var chain = new List<INamedTypeSymbol>();
        for (var current = type; current is { SpecialType: not SpecialType.System_Object }; current = current.BaseType)
        {
            chain.Add(current);
        }

        chain.Reverse();
        var properties = new List<IPropertySymbol>();
        var index = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var property in chain.SelectMany(z => z.GetMembers().OfType<IPropertySymbol>()))
        {
            if (property is { IsStatic: true } or { IsIndexer: true } || property.DeclaredAccessibility != Accessibility.Public || property.GetMethod is not { DeclaredAccessibility: Accessibility.Public })
            {
                continue;
            }

            if (index.TryGetValue(property.Name, out var existing))
            {
                properties[existing] = property;
            }
            else
            {
                index[property.Name] = properties.Count;
                properties.Add(property);
            }
        }

        return properties;
    }

    /// <summary>Get-only properties the binder still fills: mutable collections and objects, updated in place.</summary>
    private static bool IsBoundInPlace(ITypeSymbol type) => type switch
    {
        INamedTypeSymbol { IsGenericType: true } named when IsCollection(named, out var kind) => kind is "List`1" or "IList`1" or "ICollection`1" or "ISet`1" or "HashSet`1" or "Dictionary`2" or "IDictionary`2",
        INamedTypeSymbol { TypeKind: TypeKind.Class, SpecialType: SpecialType.None } named => !IsSystemType(named) && !named.IsAbstract,
        _ => false,
    };

    private SchemaNode? BuildMember(Member member, int depth)
    {
        var node = BuildType(member.Property.Type, depth, member.Property, out var reported);
        if (node is null)
        {
            if (!reported)
            {
                Report(Diagnostics.UnsupportedConfigurationMember, LocationOf(member.Property), MemberName(member.Property), member.Property.Type.ToDisplayString());
            }

            return null;
        }

        node = ApplyAnnotations(node, member);
        return node with { DefaultJson = DefaultValue(member, node) };
    }

    private SchemaNode? BuildType(ITypeSymbol type, int depth, ISymbol via, out bool reported)
    {
        reported = false;
        var nullable = false;
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullableType)
        {
            type = nullableType.TypeArguments[0];
            nullable = true;
        }
        else if (type is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated })
        {
            nullable = true;
        }

        var node = Scalar(type) ?? WellKnown(type) ?? Enumeration(type);
        if (node is null && type is IArrayTypeSymbol or INamedTypeSymbol { IsGenericType: true })
        {
            node = Collection(type, depth, via, out reported);
            if (node is null && type is IArrayTypeSymbol)
            {
                return null;
            }
        }

        if (node is null && !reported && type is INamedTypeSymbol named && HasAttribute(named, "System.ComponentModel.TypeConverterAttribute"))
        {
            // The binder converts it from a string with its TypeConverter.
            node = new() { Kind = SchemaKind.String };
        }

        if (node is null && !reported && type is INamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Struct, IsAbstract: false } objectType && !IsSystemType(objectType))
        {
            node = BuildObject(objectType, depth, via, out reported);
        }

        return node is null ? null : node with { Nullable = nullable };
    }

    // A lookup rather than a switch: SpecialType has dozens of members, and the scalars are a handful of them.
#pragma warning disable IDE0028
    private static readonly Dictionary<SpecialType, SchemaNode> Scalars = new()
    {
        [SpecialType.System_String] = new() { Kind = SchemaKind.String },
        [SpecialType.System_Boolean] = new() { Kind = SchemaKind.Boolean },
        [SpecialType.System_Char] = new() { Kind = SchemaKind.String, MinLength = 1, MaxLength = 1 },
        [SpecialType.System_SByte] = Integer("-128", "127"),
        [SpecialType.System_Byte] = Integer("0", "255"),
        [SpecialType.System_Int16] = Integer("-32768", "32767"),
        [SpecialType.System_UInt16] = Integer("0", "65535"),
        [SpecialType.System_Int32] = Integer(null, null),
        [SpecialType.System_Int64] = Integer(null, null),
        [SpecialType.System_UInt32] = Integer("0", null),
        [SpecialType.System_UInt64] = Integer("0", null),
        [SpecialType.System_Single] = new() { Kind = SchemaKind.Number },
        [SpecialType.System_Double] = new() { Kind = SchemaKind.Number },
        [SpecialType.System_Decimal] = new() { Kind = SchemaKind.Number },
    };
#pragma warning restore IDE0028

    private static SchemaNode? Scalar(ITypeSymbol type) => Scalars.TryGetValue(type.SpecialType, out var node) ? node : null;

    private static SchemaNode Integer(string? minimum, string? maximum) => new() { Kind = SchemaKind.Integer, Minimum = minimum, Maximum = maximum };

    private static SchemaNode? WellKnown(ITypeSymbol type) =>
        type.ContainingNamespace is not { Name: "System", ContainingNamespace.IsGlobalNamespace: true }
            ? null
            : type.MetadataName switch
            {
                "TimeSpan" => new()
                {
                    Kind = SchemaKind.String,
                    Pattern = @"^-?(?:\d+|(?:\d+\.)?\d{1,2}:\d{1,2}(?::\d{1,2}(?:\.\d{1,7})?)?)$",
                    FormatHint = "A time span such as 00:00:30 or 1.12:00:00.",
                },
                "DateTime" or "DateTimeOffset" => new() { Kind = SchemaKind.String, Format = "date-time" },
                "DateOnly" => new() { Kind = SchemaKind.String, Format = "date" },
                "TimeOnly" => new() { Kind = SchemaKind.String, Format = "time" },
                "Guid" => new() { Kind = SchemaKind.String, Pattern = @"^[{(]?[0-9A-Fa-f]{8}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{12}[)}]?$" },
                "Version" => new() { Kind = SchemaKind.String, Pattern = @"^\d+(?:\.\d+){1,3}$" },
                "Uri" => new() { Kind = SchemaKind.String, Format = "uri-reference" },
                _ => null,
            };

    private static SchemaNode? Enumeration(ITypeSymbol type) =>
        type is INamedTypeSymbol { TypeKind: TypeKind.Enum } enumType
            ? new()
            {
                Kind = SchemaKind.String,
                EnumValues = EnumNames(enumType).ToEquatableArray(),
                FlagsEnum = HasAttribute(enumType, "System.FlagsAttribute"),
            }
            : null;

    private static IEnumerable<string> EnumNames(INamedTypeSymbol enumType) =>
        enumType.GetMembers().OfType<IFieldSymbol>().Where(z => z.HasConstantValue).Select(z => z.Name);

    private SchemaNode? Collection(ITypeSymbol type, int depth, ISymbol via, out bool reported)
    {
        reported = false;
        if (type is IArrayTypeSymbol { Rank: 1 } array)
        {
            if (array.ElementType.SpecialType == SpecialType.System_Byte)
            {
                return new() { Kind = SchemaKind.String, ContentEncoding = "base64" };
            }

            var items = BuildType(array.ElementType, depth, via, out reported);
            return items is null ? null : new() { Kind = SchemaKind.Array, Items = items };
        }

        if (type is not INamedTypeSymbol named || !IsCollection(named, out var kind))
        {
            return null;
        }

        if (named.TypeArguments.Length == 1)
        {
            var items = BuildType(named.TypeArguments[0], depth, via, out reported);
            return items is null ? null : new() { Kind = SchemaKind.Array, Items = items };
        }

        var keyType = named.TypeArguments[0];
        var keyNames = keyType switch
        {
            { SpecialType: SpecialType.System_String } => EquatableArray<string>.Empty,
            INamedTypeSymbol { TypeKind: TypeKind.Enum } enumKey => EnumNames(enumKey).ToEquatableArray(),
            _ => (EquatableArray<string>?)null,
        };
        if (keyNames is null)
        {
            return null;
        }

        var values = BuildType(named.TypeArguments[1], depth, via, out reported);
        return values is null ? null : new() { Kind = SchemaKind.Map, Values = values, KeyNames = keyNames.Value };
    }

    private static bool IsCollection(INamedTypeSymbol type, out string kind)
    {
        kind = type.OriginalDefinition.MetadataName;
        return type.OriginalDefinition.ContainingNamespace?.ToDisplayString() == "System.Collections.Generic"
         && kind is "List`1" or "IList`1" or "ICollection`1" or "IEnumerable`1" or "IReadOnlyList`1" or "IReadOnlyCollection`1" or "ISet`1" or "HashSet`1" or "IReadOnlySet`1"
            or "Dictionary`2" or "IDictionary`2" or "IReadOnlyDictionary`2";
    }

    /// <summary>Types from the BCL bind through type converters (CultureInfo, IPAddress, …), so describing their properties would be wrong.</summary>
    private static bool IsSystemType(INamedTypeSymbol type)
    {
        var root = type.ContainingNamespace;
        while (root is { ContainingNamespace.IsGlobalNamespace: false })
        {
            root = root.ContainingNamespace;
        }

        return root is { Name: "System" } || type.SpecialType == SpecialType.System_Object;
    }

    private SchemaNode ApplyAnnotations(SchemaNode node, Member member)
    {
        string? description = null;
        foreach (var attribute in member.Property.GetAttributes().Concat(member.Parameter?.GetAttributes() ?? []))
        {
            var name = attribute.AttributeClass?.ToDisplayString();
            switch (name)
            {
                case "System.ComponentModel.DescriptionAttribute" when attribute.ConstructorArguments is [{ Value: string text }]:
                    description = text;
                    break;
                case "System.ObsoleteAttribute":
                    node = node with { Deprecated = true, DeprecationMessage = attribute.ConstructorArguments is [{ Value: string message }, ..] ? message : null };
                    break;
                case DataAnnotations + "RangeAttribute":
                    node = ApplyRange(node, attribute, member);
                    break;
                case DataAnnotations + "MinLengthAttribute" when attribute.ConstructorArguments is [{ Value: int length }]:
                    node = ApplyLength(node, attribute, member, length, null);
                    break;
                case DataAnnotations + "MaxLengthAttribute" when attribute.ConstructorArguments is [{ Value: int length }]:
                    node = ApplyLength(node, attribute, member, null, length);
                    break;
                case DataAnnotations + "LengthAttribute" when attribute.ConstructorArguments is [{ Value: int minimum }, { Value: int maximum }]:
                    node = ApplyLength(node, attribute, member, minimum, maximum);
                    break;
                case DataAnnotations + "StringLengthAttribute" when attribute.ConstructorArguments is [{ Value: int maximum }]:
                    var minimumLength = attribute.NamedArguments.FirstOrDefault(z => z.Key == "MinimumLength").Value.Value as int?;
                    node = node.Kind == SchemaKind.String
                        ? node with { MaxLength = maximum, MinLength = minimumLength is > 0 ? minimumLength : node.MinLength }
                        : Inexpressible(node, attribute, member, "string lengths only apply to strings");
                    break;
                case DataAnnotations + "RegularExpressionAttribute" when attribute.ConstructorArguments is [{ Value: string pattern }]:
                    node = node.Kind != SchemaKind.String
                        ? Inexpressible(node, attribute, member, "patterns only apply to strings")
                        : UsesNetOnlyRegexSyntax(pattern)
                            ? Inexpressible(node, attribute, member, "it uses regular expression syntax that JSON Schema (ECMA-262) doesn't support")
                            : node with { Pattern = "^(?:" + pattern + ")$" };
                    break;
                case DataAnnotations + "UrlAttribute" when node.Kind == SchemaKind.String:
                    node = node with { Format = "uri" };
                    break;
                case DataAnnotations + "EmailAddressAttribute" when node.Kind == SchemaKind.String:
                    node = node with { Format = "email" };
                    break;
                default:
                    break;
            }
        }

        return Describe(node, member.Property, description);
    }

    /// <summary>
    ///     Adds the description: an explicit <c>[Description]</c>, else the symbol's summary (read from source, or looked up later in
    ///     the package's XML docs). A member's description replaces its type's.
    /// </summary>
    private SchemaNode Describe(SchemaNode node, ISymbol symbol, string? explicitDescription)
    {
        var description = explicitDescription ?? ( symbol is INamedTypeSymbol type ? DescriptionAttribute(type) : null );
        if (description is null && symbol.DeclaringSyntaxReferences.Length > 0)
        {
            // A positional record's property is documented with <param> on the record.
            description = symbol.DeclaringSyntaxReferences[0].GetSyntax(_cancellationToken) is ParameterSyntax
                ? DocumentationReader.Parameter(symbol.ContainingType, symbol.Name, _cancellationToken)
                : DocumentationReader.Summary(symbol, _cancellationToken);
        }

        return description is not null
            ? node with { Description = description, DocumentationId = null, DocumentationAssembly = null }
            : symbol.DeclaringSyntaxReferences.Length == 0 && symbol.GetDocumentationCommentId() is { } id && symbol.ContainingAssembly is { } assembly
                ? node with { DocumentationId = id, DocumentationAssembly = assembly.Name }
                : node;
    }

    /// <summary>
    ///     Whether <paramref name="pattern" /> uses .NET-only syntax: inline options, atomic groups, .NET anchors, Unicode categories,
    ///     or character class subtraction (<c>[a-z-[aeiou]]</c>, which only counts inside a class: <c>[a-z]-[A-Z]</c> is fine).
    /// </summary>
    private static bool UsesNetOnlyRegexSyntax(string pattern)
    {
        if (NonPortableRegex.Any(z => pattern.Contains(z)))
        {
            return true;
        }

        var inClass = false;
        for (var i = 0; i < pattern.Length; i++)
        {
            switch (pattern[i])
            {
                case '\\':
                    i++;
                    break;
                case '[' when !inClass:
                    inClass = true;
                    break;
                case ']' when inClass:
                    inClass = false;
                    break;
                case '-' when inClass && i + 1 < pattern.Length && pattern[i + 1] == '[':
                    return true;
                default:
                    break;
            }
        }

        return false;
    }

    private static string? DescriptionAttribute(ISymbol symbol) =>
        symbol.GetAttributes().FirstOrDefault(z => z.AttributeClass?.ToDisplayString() == "System.ComponentModel.DescriptionAttribute") is { ConstructorArguments: [{ Value: string text }] }
            ? text
            : null;

    private SchemaNode ApplyRange(SchemaNode node, AttributeData attribute, Member member)
    {
        if (node.Kind is not ( SchemaKind.Integer or SchemaKind.Number ))
        {
            return Inexpressible(node, attribute, member, "ranges only apply to numbers");
        }

        string? minimum = null;
        string? maximum = null;
        if (attribute.ConstructorArguments is [{ Value: var low }, { Value: var high }] && NumberText(low) is { } lowText && NumberText(high) is { } highText)
        {
            minimum = lowText;
            maximum = highText;
        }
        else if (attribute.ConstructorArguments is [{ Kind: TypedConstantKind.Type }, { Value: string lowString }, { Value: string highString }]
              && double.TryParse(lowString, NumberStyles.Float, CultureInfo.InvariantCulture, out var lowValue)
              && double.TryParse(highString, NumberStyles.Float, CultureInfo.InvariantCulture, out var highValue))
        {
            minimum = NumberText(lowValue);
            maximum = NumberText(highValue);
        }

        return minimum is null || maximum is null
            ? Inexpressible(node, attribute, member, "its bounds aren't numbers")
            : node with
            {
                Minimum = minimum,
                Maximum = maximum,
                ExclusiveMinimum = attribute.NamedArguments.Any(z => z is { Key: "MinimumIsExclusive", Value.Value: true }),
                ExclusiveMaximum = attribute.NamedArguments.Any(z => z is { Key: "MaximumIsExclusive", Value.Value: true }),
            };
    }

    private SchemaNode ApplyLength(SchemaNode node, AttributeData attribute, Member member, int? minimum, int? maximum) =>
        node.Kind == SchemaKind.String
            ? node with { MinLength = minimum ?? node.MinLength, MaxLength = maximum ?? node.MaxLength }
            : node.Kind == SchemaKind.Array
                ? node with { MinItems = minimum ?? node.MinItems, MaxItems = maximum ?? node.MaxItems }
                : Inexpressible(node, attribute, member, "lengths only apply to strings and collections");

    private SchemaNode Inexpressible(SchemaNode node, AttributeData attribute, Member member, string reason)
    {
        Report(
            Diagnostics.InexpressibleConfigurationAnnotation,
            attribute.ApplicationSyntaxReference?.GetSyntax(_cancellationToken).GetLocation() ?? LocationOf(member.Property),
            "[" + ( attribute.AttributeClass?.Name is { } name && name.EndsWith("Attribute", StringComparison.Ordinal) ? name[..^9] : attribute.AttributeClass?.Name ) + "]",
            MemberName(member.Property),
            reason
        );
        return node;
    }

    /// <summary>A constant default: a record parameter's default value, or a literal property initializer.</summary>
    private string? DefaultValue(Member member, SchemaNode node)
    {
        if (member.Parameter is { HasExplicitDefaultValue: true, ExplicitDefaultValue: { } value })
        {
            return Literal(value, node);
        }

        foreach (var reference in member.Property.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax(_cancellationToken) is PropertyDeclarationSyntax { Initializer.Value: { } initializer })
            {
                return InitializerLiteral(initializer, node);
            }
        }

        return null;
    }

    private static string? InitializerLiteral(ExpressionSyntax expression, SchemaNode node) => expression switch
    {
        LiteralExpressionSyntax { Token.Value: { } value } => Literal(value, node),
        PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.UnaryMinusExpression, Operand: LiteralExpressionSyntax { Token.Value: { } value } } =>
            NumberText(value) is { } number && node.Kind is SchemaKind.Integer or SchemaKind.Number ? "-" + number : null,
        MemberAccessExpressionSyntax { Name.Identifier.ValueText: var name } when node.EnumValues.Items.Contains(name) => JsonSchemaWriter.String(name),
        _ => null,
    };

    private static string? Literal(object value, SchemaNode node) => value switch
    {
        string text when node.Kind == SchemaKind.String && node.EnumValues.IsEmpty => JsonSchemaWriter.String(text),
        char character when node.Kind == SchemaKind.String => JsonSchemaWriter.String(character.ToString()),
        bool boolean when node.Kind == SchemaKind.Boolean => boolean ? "true" : "false",
        _ when node.Kind is SchemaKind.Integer or SchemaKind.Number => NumberText(value),
        _ => null,
    };

    private static string? NumberText(object? value) => value switch
    {
        double number when !double.IsNaN(number) && !double.IsInfinity(number) => number.ToString("R", CultureInfo.InvariantCulture),
        float number when !float.IsNaN(number) && !float.IsInfinity(number) => number.ToString("R", CultureInfo.InvariantCulture),
        decimal number => number.ToString(CultureInfo.InvariantCulture),
        sbyte or byte or short or ushort or int or uint or long or ulong => Convert.ToString(value, CultureInfo.InvariantCulture),
        _ => null,
    };

    private static string? ConfigurationKeyName(IPropertySymbol property) =>
        property.GetAttributes().FirstOrDefault(z => z.AttributeClass?.ToDisplayString() == "Microsoft.Extensions.Configuration.ConfigurationKeyNameAttribute")
            is { ConstructorArguments: [{ Value: string { Length: > 0 } name }] }
            ? name
            : null;

    private static bool HasAttribute(ISymbol symbol, string fullName) => symbol.GetAttributes().Any(z => z.AttributeClass?.ToDisplayString() == fullName);

    private static string MemberName(ISymbol symbol) => symbol.ContainingType is { } type ? type.Name + "." + symbol.Name : symbol.Name;

    private Location? LocationOf(ISymbol? symbol) => symbol?.Locations.FirstOrDefault(z => z.IsInSource) ?? _declarationLocation;

    private void Report(DiagnosticDescriptor descriptor, Location? location, params string[] arguments) =>
        _diagnostics.Add(DiagnosticInfo.Create(descriptor, location, arguments));
}
