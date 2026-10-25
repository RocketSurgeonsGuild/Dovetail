using System.Globalization;
using Dovetail.Support.Configuration.Binding;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dovetail.Support.Configuration;

/// <summary>
///     Turns a <c>[DovetailConfiguration]</c> declaration into a <see cref="ConfigurationSection" />, describing what Microsoft's
///     configuration-binding source generator fills: settable, init-only and constructor-bound properties, plus get-only
///     collections and objects it binds in place. Each object type becomes one <see cref="SchemaDefinition" />.
/// </summary>
internal sealed class ModelBuilder
{
    /// <summary>A guard against type graphs that never close, e.g. <c>Node&lt;T&gt;</c> with a <c>Node&lt;Node&lt;T&gt;&gt;</c> member.</summary>
    private const int MaxDefinitions = 200;

    private const string DataAnnotations = "System.ComponentModel.DataAnnotations.";
    private static readonly string[] SectionSuffixes = ["Configuration", "Settings", "Options", "Config"];

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
        [SpecialType.System_Object] = new() { Kind = SchemaKind.Any },
    };

    // A collection expression can't take the comparer here: the analyzer also builds against Roslyn 5.0 (C# 14).
    private readonly Dictionary<ITypeSymbol, string> _definitionNames = new(SymbolEqualityComparer.Default);
    private readonly Dictionary<string, SchemaNode?> _definitions = new(StringComparer.Ordinal);
    private readonly HashSet<string> _referenced = new(StringComparer.Ordinal);
#pragma warning restore IDE0028

    private readonly List<string> _definitionOrder = [];
    private readonly List<DiagnosticInfo> _diagnostics = [];
    private readonly Location? _declarationLocation;
    private readonly CancellationToken _cancellationToken;

    /// <summary>How keys that have no <c>[ConfigurationKeyName]</c> are spelled.</summary>
    private readonly KeyStyle _keyStyle;

    /// <summary><c>[ConfigurationIgnore]</c>, when the binder honours it (version 11 on).</summary>
    private readonly INamedTypeSymbol? _ignoreAttribute;

    private string _rootTypeName = "";
    private bool _reportedTooLarge;

    private ModelBuilder(Location? declarationLocation, INamedTypeSymbol? ignoreAttribute, KeyStyle keyStyle, CancellationToken cancellationToken)
    {
        _declarationLocation = declarationLocation;
        _keyStyle = keyStyle;
        _ignoreAttribute = ignoreAttribute;
        _cancellationToken = cancellationToken;
    }

    /// <summary>Builds the section for one <c>[DovetailConfiguration]</c> attribute.</summary>
    /// <param name="attribute">The attribute.</param>
    /// <param name="attributedType">The class it's on, or <see langword="null" /> on the assembly.</param>
    /// <param name="compilation">The compilation, for the binder's view of the type.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public static ConfigurationDeclaration Build(AttributeData attribute, INamedTypeSymbol? attributedType, Compilation compilation, CancellationToken cancellationToken)
    {
        string? section = null;
        // The class form's options type is the class; the assembly form's is the type argument of DovetailConfiguration<T>.
        var type = attributedType ?? attribute.AttributeClass?.TypeArguments.FirstOrDefault();
        foreach (var argument in attribute.ConstructorArguments)
        {
            if (argument is { Kind: TypedConstantKind.Primitive, Value: string value })
            {
                section = value;
            }
        }

        var registerOptions = true;
        var validateOnStart = false;
        var keyStyle = KeyStyle.Undefined;
        foreach (var named in attribute.NamedArguments)
        {
            switch (named.Key)
            {
                case "RegisterOptions" when named.Value.Value is bool value:
                    registerOptions = value;
                    break;
                case "ValidateOnStart" when named.Value.Value is bool value:
                    validateOnStart = value;
                    break;
                case "KeyStyle" when named.Value.Value is int value && Enum.IsDefined(typeof(KeyStyle), value):
                    keyStyle = (KeyStyle)value;
                    break;
                default:
                    break;
            }
        }

        var version = BindingParser.VersionOf(compilation);
        var builder = new ModelBuilder(
            attribute.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation(),
            version >= BinderVersion.V11 ? compilation.GetTypeByMetadataName("Microsoft.Extensions.Configuration.ConfigurationIgnoreAttribute") : null,
            keyStyle,
            cancellationToken
        );
        ConfigurationSection? result = null;
        ConfigurationOptions? options = null;
        try
        {
            result = builder.BuildSection(type, section, registerOptions, validateOnStart);
            if (result is { RegisterOptions: true } && type is not null)
            {
                // The binder's own rules for creating the type are stricter in places (e.g. ref parameters).
                var types = new BindingParser(compilation, version, keyStyle).Parse(type, cancellationToken);
                var index = new BindingTypeIndex(types);
                if (index.TryGetTypeSpec(TypeRef.From(type), out var spec) && spec is ObjectSpec root && !index.CanInstantiate(root))
                {
                    builder.Report(
                        Diagnostics.UnsupportedConfigurationConstructor,
                        builder._declarationLocation,
                        type.ToDisplayString(),
                        root.InitFailureReason ?? "the configuration binder can't create it"
                    );
                }
                else
                {
                    options = new(result.Path, result.ValidateOnStart, TypeRef.From(type), version, types);
                }
            }
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // A bug here mustn't take down the rest of the generator (Imports, Exports) with it.
            builder.Report(Diagnostics.ConfigurationSchemaFailed, builder._declarationLocation, type?.ToDisplayString() ?? "?", exception.GetType().Name + ": " + exception.Message);
        }

        return new(
            result,
            builder._diagnostics.ToEquatableArray(),
            options,
            type is null or { TypeKind: TypeKind.Error } ? null : type.ToDisplayString(),
            LocationInfo.From(builder._declarationLocation)
        );
    }

    /// <summary>The type name without an Options/Settings/Configuration/Config suffix.</summary>
    public static string StripSectionSuffix(string typeName)
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

    private ConfigurationSection? BuildSection(ITypeSymbol? type, string? section, bool registerOptions, bool validateOnStart)
    {
        if (type is null or { TypeKind: TypeKind.Error })
        {
            return null;
        }

        _rootTypeName = type.ToDisplayString();
        if (InvalidTypeReason(type) is { } reason)
        {
            Report(Diagnostics.InvalidConfigurationType, _declarationLocation ?? LocationOf(type), _rootTypeName, reason);
            return null;
        }

        // An explicit section is used as written; a derived one follows the key style.
        var stripped = StripSectionSuffix(type.Name);
        var path = section ?? KeyNaming.Apply(stripped, _keyStyle);
        if (InvalidSectionReason(path, section is null) is { } sectionReason)
        {
            Report(Diagnostics.InvalidConfigurationSection, _declarationLocation ?? LocationOf(type), path, _rootTypeName, sectionReason);
            return null;
        }

        // The options factory creates the section's object itself, so its constructor has to work; nested objects don't.
        var root = (INamedTypeSymbol)type;
        if (!TryResolveConstructor(root, _declarationLocation ?? LocationOf(type), out _))
        {
            return null;
        }

        var rootName = Define(root, null)!;
        var definitions = _definitionOrder
                         .Where(z => z != rootName || _referenced.Contains(z))
                         .Select(z => new SchemaDefinition(z, _definitions[z]!))
                         .ToEquatableArray();
        return new(path, registerOptions, validateOnStart, type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), _definitions[rootName]!, definitions);
    }

    private static string? InvalidTypeReason(ITypeSymbol type) => type switch
    {
        { TypeKind: TypeKind.Interface } => "it's an interface",
        { IsStatic: true } => "it's static",
        { IsAbstract: true } => "it's abstract",
        not INamedTypeSymbol { TypeKind: TypeKind.Class } => "it isn't a class",
        INamedTypeSymbol named when IsOpenGeneric(named) => "it's an open generic type",
        _ when type.AllInterfaces.Any(z => z.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T) =>
            "it's a collection; declare the class that contains it",
        _ => null,
    };

    /// <summary>Unbound type parameters anywhere in the type or the types it's nested in.</summary>
    private static bool IsOpenGeneric(INamedTypeSymbol type)
    {
        for (var current = type; current is not null; current = current.ContainingType)
        {
            if (current.IsUnboundGenericType || current.TypeArguments.Any(z => z is ITypeParameterSymbol))
            {
                return true;
            }
        }

        return false;
    }

    private static string? InvalidSectionReason(string path, bool derived) =>
        derived && path.Length == 0
            ? "the type name is only a suffix, so pass a section path"
            : path.Trim().Length == 0
                ? "section paths can't be empty"
                : path.Split(':').Any(z => z.Trim().Length == 0)
                    ? "section paths can't have empty segments"
                    : null;

    /// <summary>
    ///     The definition name for an object type, describing it the first time it's seen. A recursive use gets the name of the
    ///     definition still being built. Returns <see langword="null" /> (reported) once the section has too many types.
    /// </summary>
    private string? Define(INamedTypeSymbol type, ISymbol? viaMember)
    {
        if (_definitionNames.TryGetValue(type, out var existing))
        {
            return existing;
        }

        if (_definitionNames.Count >= MaxDefinitions)
        {
            if (!_reportedTooLarge)
            {
                _reportedTooLarge = true;
                Report(
                    Diagnostics.ConfigurationTypeTooLarge,
                    LocationOf(viaMember),
                    _rootTypeName,
                    MaxDefinitions.ToString(CultureInfo.InvariantCulture),
                    viaMember is null ? type.Name : MemberName(viaMember)
                );
            }

            return null;
        }

        var name = DefinitionName(type);
        _definitionNames[type] = name;
        _definitions[name] = null;
        _definitionOrder.Add(name);
        _definitions[name] = BuildObjectSchema(type);
        return name;
    }

    /// <summary>A readable, JSON-pointer-safe name that's unique within the section.</summary>
    private string DefinitionName(INamedTypeSymbol type)
    {
        var display = type.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString();
        var name = new string([.. display.Select(z => char.IsLetterOrDigit(z) || z is '.' or '-' ? z : '_')]);
        var candidate = name;
        for (var index = 2; _definitions.ContainsKey(candidate); index++)
        {
            candidate = name + "-" + index.ToString(CultureInfo.InvariantCulture);
        }

        return candidate;
    }

    private SchemaNode BuildObjectSchema(INamedTypeSymbol type)
    {
        // Constructor problems only matter for the section's own type (checked before); a nested object is filled in place
        // when it's already there, so its settable properties are described either way.
        var constructor = TryResolveConstructor(type, null, out var resolved) ? resolved : null;
        var properties = new List<SchemaProperty>();
        var keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var member in BindableMembers(type, constructor, _ignoreAttribute, _keyStyle))
        {
            _cancellationToken.ThrowIfCancellationRequested();
            var node = BuildMember(member);
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

        return Describe(new() { Kind = SchemaKind.Object, Properties = properties.ToEquatableArray() }, type, null);
    }

    /// <summary>
    ///     The constructor whose parameters bind from configuration, or <see langword="null" /> when properties are set after a
    ///     parameterless one. Returns <see langword="false" /> when the type can't be created; that's reported only when
    ///     <paramref name="reportAt" /> is given.
    /// </summary>
    private bool TryResolveConstructor(INamedTypeSymbol type, Location? reportAt, out IMethodSymbol? constructor)
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

        var parameterized = constructors.Where(z => !z.Parameters.IsEmpty).ToArray();
        if (parameterized.Length != 1)
        {
            ReportConstructor(reportAt, type, parameterized.Length == 0 ? "it has no public constructor" : "it has several public constructors with parameters and no parameterless one");
            return false;
        }

        var properties = AllProperties(type, _ignoreAttribute);
        if (parameterized[0].Parameters.FirstOrDefault(z => !properties.Any(p => string.Equals(p.Name, z.Name, StringComparison.OrdinalIgnoreCase))) is { } orphan)
        {
            ReportConstructor(reportAt, type, $"constructor parameter '{orphan.Name}' has no matching property");
            return false;
        }

        constructor = parameterized[0];
        return true;
    }

    private void ReportConstructor(Location? reportAt, INamedTypeSymbol type, string reason)
    {
        if (reportAt is not null)
        {
            Report(Diagnostics.UnsupportedConfigurationConstructor, reportAt, type.ToDisplayString(), reason);
        }
    }

    private readonly record struct Member(string Key, IPropertySymbol Property, IParameterSymbol? Parameter, bool Required);

    private static IEnumerable<Member> BindableMembers(INamedTypeSymbol type, IMethodSymbol? constructor, INamedTypeSymbol? ignoreAttribute, KeyStyle keyStyle)
    {
        foreach (var property in AllProperties(type, ignoreAttribute))
        {
            var parameter = constructor?.Parameters.FirstOrDefault(z => string.Equals(z.Name, property.Name, StringComparison.OrdinalIgnoreCase));
            var bindable = parameter is not null
             || property.SetMethod is { DeclaredAccessibility: Accessibility.Public }
             || IsBoundInPlace(property.Type);
            if (!bindable)
            {
                continue;
            }

            var key = ConfigurationKeyName(property) ?? KeyNaming.Apply(property.Name, keyStyle);
            var required = property.IsRequired
             || HasAttribute(property, DataAnnotations + "RequiredAttribute")
             || ( parameter is { HasExplicitDefaultValue: false } && parameter.Type is { IsValueType: true } && parameter.Type.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T );
            yield return new(key, property, parameter, required);
        }
    }

    /// <summary>Public properties with a public getter, base types first; a redeclared property keeps its base position.</summary>
    private static List<IPropertySymbol> AllProperties(INamedTypeSymbol type, INamedTypeSymbol? ignoreAttribute)
    {
        var chain = new List<INamedTypeSymbol>();
        for (var current = type; current is { SpecialType: not SpecialType.System_Object }; current = current.BaseType)
        {
            chain.Add(current);
        }

        chain.Reverse();
        var properties = new List<IPropertySymbol>();
        var index = new Dictionary<string, int>(StringComparer.Ordinal);
        var ignored = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (var property in chain.SelectMany(z => z.GetMembers().OfType<IPropertySymbol>()))
        {
            // Static properties count: the binder sets them too.
            if (property is { IsIndexer: true } || property.DeclaredAccessibility != Accessibility.Public || property.GetMethod is not { DeclaredAccessibility: Accessibility.Public })
            {
                continue;
            }

            // The binder reads [ConfigurationIgnore] off the first declaration of an overridden property, and off the most derived
            // one of a redeclared (new) property.
            if (ignoreAttribute is not null)
            {
                var declaration = property;
                while (declaration.OverriddenProperty is { } overridden)
                {
                    declaration = overridden;
                }

                ignored[property.Name] = declaration.GetAttributes().Any(z => SymbolEqualityComparer.Default.Equals(z.AttributeClass, ignoreAttribute));
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

        return ignored.Count == 0 ? properties : [.. properties.Where(z => !ignored.TryGetValue(z.Name, out var value) || !value)];
    }

    /// <summary>Get-only properties the binder still fills: mutable collections and objects, updated in place.</summary>
    private static bool IsBoundInPlace(ITypeSymbol type) =>
        type is INamedTypeSymbol named
     && ( Dictionary(named) is not null || Element(named) is not null
            ? named.TypeKind == TypeKind.Class || named.OriginalDefinition.MetadataName is "IList`1" or "ICollection`1" or "ISet`1" or "IDictionary`2"
            : named is { TypeKind: TypeKind.Class or TypeKind.Interface, SpecialType: SpecialType.None } && !IsSystemType(named) );

    private SchemaNode? BuildMember(Member member)
    {
        var node = BuildType(member.Property.Type, member.Property);
        if (node is null)
        {
            Report(Diagnostics.UnsupportedConfigurationMember, LocationOf(member.Property), MemberName(member.Property), member.Property.Type.ToDisplayString());
            return null;
        }

        node = ApplyAnnotations(node, member);
        return node with { DefaultJson = DefaultValue(member, node) };
    }

    private SchemaNode? BuildType(ITypeSymbol type, ISymbol via)
    {
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

        var node = Scalar(type) ?? WellKnown(type) ?? Enumeration(type) ?? ( IsConfigurationSection(type) ? new SchemaNode { Kind = SchemaKind.Any } : null );
        if (node is null && type is IArrayTypeSymbol array)
        {
            node = array.Rank != 1
                ? null
                : array.ElementType.SpecialType == SpecialType.System_Byte
                    ? new() { Kind = SchemaKind.String, ContentEncoding = "base64" }
                    : BuildType(array.ElementType, via) is { } items
                        ? new() { Kind = SchemaKind.Array, Items = items }
                        : null;
        }
        else if (node is null && type is INamedTypeSymbol named)
        {
            node = Dictionary(named) is ({ } keyType, { } valueType)
                ? MapKeys(keyType) is { } keys && BuildType(valueType, via) is { } values ? keys with { Values = values } : null
                : Element(named) is { } elementType
                    ? BuildType(elementType, via) is { } items ? new() { Kind = SchemaKind.Array, Items = items } : null
                    // Interfaces and abstract classes can't be created, but the binder fills an existing instance's declared properties.
                    : named is { TypeKind: TypeKind.Class or TypeKind.Struct or TypeKind.Interface } && !IsSystemType(named)
                        ? Define(named, via) is { } name ? Reference(name) : new() { Kind = SchemaKind.Any }
                        : null;
        }

        return node is null ? null : node with { Nullable = nullable };
    }

    private SchemaNode Reference(string name)
    {
        _referenced.Add(name);
        return new() { Kind = SchemaKind.Object, Reference = name };
    }

    private static SchemaNode? Scalar(ITypeSymbol type) => Scalars.TryGetValue(type.SpecialType, out var node) ? node : null;

    private static SchemaNode Integer(string? minimum, string? maximum) => new() { Kind = SchemaKind.Integer, Minimum = minimum, Maximum = maximum };

    private static SchemaNode? WellKnown(ITypeSymbol type) =>
        type.ContainingNamespace?.ToDisplayString() switch
        {
            "System" => type.MetadataName switch
            {
                "TimeSpan" => new()
                {
                    Kind = SchemaKind.String,
                    Pattern = @"^-?(?:\d+|(?:\d+[.:])?\d{1,2}:\d{1,2}(?::\d{1,2}(?:\.\d{1,7})?)?)$",
                    FormatHint = "A time span such as 00:00:30 or 1.12:00:00.",
                },
                // The binder parses many date and time forms; ISO 8601 without the strictness of the RFC 3339 formats (which
                // demand a time zone, and which TimeOnly can't even express).
                "DateTime" or "DateTimeOffset" => new()
                {
                    Kind = SchemaKind.String,
                    Pattern = @"^\d{4}-\d{2}-\d{2}(?:[T ]\d{2}:\d{2}(?::\d{2}(?:\.\d{1,7})?)?)?(?:Z|[+-]\d{2}:?\d{2})?$",
                    FormatHint = "A date and time such as 2024-01-02T03:04:05Z.",
                },
                "DateOnly" => new() { Kind = SchemaKind.String, Pattern = @"^\d{4}-\d{2}-\d{2}$", FormatHint = "A date such as 2024-01-02." },
                "TimeOnly" => new() { Kind = SchemaKind.String, Pattern = @"^\d{1,2}:\d{2}(?::\d{2}(?:\.\d{1,7})?)?$", FormatHint = "A time of day such as 08:30." },
                "Guid" => new() { Kind = SchemaKind.String, Pattern = @"^[{(]?[0-9A-Fa-f]{8}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{4}-?[0-9A-Fa-f]{12}[)}]?$" },
                "Version" => new() { Kind = SchemaKind.String, Pattern = @"^\d+(?:\.\d+){1,3}$" },
                "Uri" => new() { Kind = SchemaKind.String, Format = "uri-reference" },
                "Half" => new() { Kind = SchemaKind.Number },
                "Int128" => Integer(null, null),
                "UInt128" => Integer("0", null),
                _ => null,
            },
            "System.Globalization" when type.MetadataName == "CultureInfo" => new() { Kind = SchemaKind.String, FormatHint = "A culture name such as en-US." },
            _ => null,
        };

    private static bool IsConfigurationSection(ITypeSymbol type) =>
        type is { Name: "IConfigurationSection", ContainingNamespace: { Name: "Configuration", ContainingNamespace: { Name: "Extensions", ContainingNamespace.Name: "Microsoft" } } };

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

    /// <summary>The key schema of a dictionary: free-form for strings, the names for enums, digits for integers.</summary>
    private static SchemaNode? MapKeys(ITypeSymbol keyType) =>
        keyType.SpecialType == SpecialType.System_String
            ? new() { Kind = SchemaKind.Map }
            : keyType is INamedTypeSymbol { TypeKind: TypeKind.Enum } enumKey
                ? new() { Kind = SchemaKind.Map, KeyNames = EnumNames(enumKey).ToEquatableArray() }
                : Scalar(keyType) is { Kind: SchemaKind.Integer } integer
                    ? new() { Kind = SchemaKind.Map, KeyPattern = integer.Minimum == "0" ? @"^\d+$" : @"^-?\d+$" }
                    : null;

    /// <summary>The key and value types of a dictionary interface, or of a class implementing one.</summary>
    private static (ITypeSymbol Key, ITypeSymbol Value)? Dictionary(INamedTypeSymbol type)
    {
        var dictionary = IsGenericCollectionInterface(type, "IDictionary`2", "IReadOnlyDictionary`2")
            ? type
            : type.TypeKind == TypeKind.Class
                ? type.AllInterfaces.FirstOrDefault(z => IsGenericCollectionInterface(z, "IDictionary`2"))
                : null;
        return dictionary is { TypeArguments: [var key, var value] } ? (key, value) : null;
    }

    /// <summary>The element type of a collection interface, or of a class implementing <c>ICollection&lt;T&gt;</c>.</summary>
    private static ITypeSymbol? Element(INamedTypeSymbol type)
    {
        if (type.SpecialType == SpecialType.System_String || Dictionary(type) is not null)
        {
            return null;
        }

        var collection = IsGenericCollectionInterface(type, "IEnumerable`1", "ICollection`1", "IList`1", "IReadOnlyList`1", "IReadOnlyCollection`1", "ISet`1", "IReadOnlySet`1")
            ? type
            : type.TypeKind == TypeKind.Class
                ? type.AllInterfaces.FirstOrDefault(z => IsGenericCollectionInterface(z, "ICollection`1"))
                : null;
        return collection is { TypeArguments: [var element] } ? element : null;
    }

    private static bool IsGenericCollectionInterface(INamedTypeSymbol type, params string[] names) =>
        type is { TypeKind: TypeKind.Interface, IsGenericType: true }
     && names.Contains(type.OriginalDefinition.MetadataName)
     && type.OriginalDefinition.ContainingNamespace?.ToDisplayString() == "System.Collections.Generic";

    /// <summary>
    ///     Types from the BCL that aren't handled above (IPAddress, Encoding, delegates, …): describing their properties as
    ///     configuration keys would be wrong.
    /// </summary>
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
    ///     the package's XML docs). A member's description is on its use; a type's is on its definition.
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
    ///     Whether <paramref name="pattern" /> uses .NET-only syntax: inline options and other <c>(?…)</c> forms ECMA-262 lacks,
    ///     <c>\A</c>/<c>\Z</c>/<c>\z</c>/<c>\G</c>, Unicode categories, or character class subtraction. Escapes are skipped, so
    ///     <c>\\Apps</c> or <c>\(?i</c> are fine.
    /// </summary>
    private static bool UsesNetOnlyRegexSyntax(string pattern)
    {
        var inClass = false;
        for (var i = 0; i < pattern.Length; i++)
        {
            var character = pattern[i];
            if (character == '\\')
            {
                var next = i + 1 < pattern.Length ? pattern[i + 1] : '\0';
                if (( !inClass && "AZzG".IndexOf(next) >= 0 ) || ( next is 'p' or 'P' && i + 2 < pattern.Length && pattern[i + 2] == '{' ))
                {
                    return true;
                }

                i++;
            }
            else if (inClass)
            {
                if (character == ']')
                {
                    inClass = false;
                }
                else if (character == '-' && i + 1 < pattern.Length && pattern[i + 1] == '[')
                {
                    return true;
                }
            }
            else if (character == '[')
            {
                inClass = true;
            }
            else if (character == '(' && i + 2 < pattern.Length && pattern[i + 1] == '?' && "imsnx-'>#(".IndexOf(pattern[i + 2]) >= 0)
            {
                return true;
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
        // An empty string tells an editor nothing (the description hint would read "Default: ."), so it is not a default.
        string { Length: 0 } => null,
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

    /// <summary>The <c>[ConfigurationKeyName]</c> of a property, or of the property it overrides.</summary>
    private static string? ConfigurationKeyName(IPropertySymbol property)
    {
        for (var current = property; current is not null; current = current.OverriddenProperty)
        {
            if (current.GetAttributes().FirstOrDefault(z => z.AttributeClass?.ToDisplayString() == "Microsoft.Extensions.Configuration.ConfigurationKeyNameAttribute")
                is { ConstructorArguments: [{ Value: string { Length: > 0 } name }] })
            {
                return name;
            }
        }

        return null;
    }

    private static bool HasAttribute(ISymbol symbol, string fullName) => symbol.GetAttributes().Any(z => z.AttributeClass?.ToDisplayString() == fullName);

    private static string MemberName(ISymbol symbol) => symbol.ContainingType is { } type ? type.Name + "." + symbol.Name : symbol.Name;

    private Location? LocationOf(ISymbol? symbol) => symbol?.Locations.FirstOrDefault(z => z.IsInSource) ?? _declarationLocation;

    private void Report(DiagnosticDescriptor descriptor, Location? location, params string[] arguments) =>
        _diagnostics.Add(DiagnosticInfo.Create(descriptor, location, arguments));
}
