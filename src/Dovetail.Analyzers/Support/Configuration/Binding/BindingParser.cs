// Ported from dotnet/runtime (MIT): ConfigurationBindingGenerator.Parser and KnownTypeSymbols, minus the call-site
// interception; the roots here are the [DovetailConfiguration] types. See BindingModel.cs.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.CodeAnalysis;

namespace Dovetail.Support.Configuration.Binding;

/// <summary>Analyses an options type and every type reachable from it, the way Microsoft's binding generator does.</summary>
internal sealed class BindingParser
{
    private const string CannotBindToConstructorParameter =
        "Cannot create instance of type '{0}' because one or more parameters cannot be bound to. Constructor parameters cannot be declared as in, out, or ref. Invalid parameters are: '{1}'";

    private const string ConstructorParametersDoNotMatchProperties =
        "Cannot create instance of type '{0}' because one or more parameters cannot be bound to. Constructor parameters must have corresponding properties. Fields are not supported. Missing properties are: '{1}'";

    private const string MissingPublicInstanceConstructor = "Cannot create instance of type '{0}' because it is missing a public instance constructor.";
    private const string MultipleParameterizedConstructors = "Cannot create instance of type '{0}' because it has multiple public parameterized constructors.";

    /// <summary>
    ///     Types that keep producing new constructed types (<c>class Node&lt;T&gt; { Node&lt;Node&lt;T&gt;&gt; Next }</c>) would never
    ///     stop: past this generic nesting depth, or this many types, a type is treated as unsupported and isn't bound.
    /// </summary>
    private const int MaxGenericDepth = 10;

    private const int MaxTypes = 1000;

    private readonly KnownTypes _types;
    private readonly Queue<ITypeSymbol> _typesToParse = new();
    private readonly Dictionary<ITypeSymbol, TypeSpec> _createdTypeSpecs = new(SymbolEqualityComparer.Default);
    private readonly HashSet<ITypeSymbol> _visitedTypes = new(SymbolEqualityComparer.Default);

    public BindingParser(Compilation compilation, BinderVersion version)
    {
        _types = new(compilation, version);
    }

    /// <summary>The binder version a compilation follows: the major version of the Microsoft.Extensions.Configuration.Binder it references.</summary>
    public static BinderVersion VersionOf(Compilation compilation) =>
        compilation.ReferencedAssemblyNames.FirstOrDefault(z => z.Name == "Microsoft.Extensions.Configuration.Binder") is { Version.Major: < 11 }
            ? BinderVersion.V10
            : BinderVersion.V11;

    /// <summary>The specs for <paramref name="root" /> and everything it reaches, ordered by type name.</summary>
    public EquatableArray<TypeSpec> Parse(ITypeSymbol root, CancellationToken cancellationToken)
    {
        _typesToParse.Enqueue(root);
        while (_typesToParse.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var type = _typesToParse.Dequeue();
            if (!_createdTypeSpecs.ContainsKey(type))
            {
                _createdTypeSpecs.Add(
                    type,
                    _createdTypeSpecs.Count >= MaxTypes || GenericDepth(type) > MaxGenericDepth ? new UnsupportedTypeSpec(type) : CreateTypeSpec(type)
                );
            }
        }

        return _createdTypeSpecs.Values.OrderBy(z => z.TypeRef.FullyQualifiedName, StringComparer.Ordinal).ToEquatableArray();
    }

    private static int GenericDepth(ITypeSymbol type) => type switch
    {
        IArrayTypeSymbol array => GenericDepth(array.ElementType),
        INamedTypeSymbol { IsGenericType: true } named => 1
          + Math.Max(named.TypeArguments.Select(GenericDepth).DefaultIfEmpty(0).Max(), named.ContainingType is { } containing ? GenericDepth(containing) - 1 : 0),
        _ => 0,
    };

    private TypeRef EnqueueTransitiveType(ITypeSymbol type, TypeRef? knownTypeRef = null)
    {
        if (_createdTypeSpecs.TryGetValue(type, out var spec))
        {
            return spec.TypeRef;
        }

        _typesToParse.Enqueue(type);
        return knownTypeRef ?? TypeRef.From(type);
    }

    private TypeSpec CreateTypeSpec(ITypeSymbol type)
    {
        if (IsNullable(type, out var underlyingType))
        {
            return new NullableSpec(type, EnqueueTransitiveType(underlyingType));
        }

        if (IsParsableFromString(type, out var kind))
        {
            return new ParsableFromStringSpec(type, kind);
        }

        if (type.TypeKind is TypeKind.Array)
        {
            return CreateArraySpec((IArrayTypeSymbol)type);
        }

        if (IsCollection(type))
        {
            return CreateCollectionSpec((INamedTypeSymbol)type) ?? CreateObjectSpec((INamedTypeSymbol)type);
        }

        if (SymbolEqualityComparer.Default.Equals(type, _types.IConfigurationSection))
        {
            return new ConfigurationSectionSpec(type);
        }

        return type is INamedTypeSymbol named ? CreateObjectSpec(named) : new UnsupportedTypeSpec(type);
    }

    private static bool IsNullable(ITypeSymbol type, [NotNullWhen(true)] out ITypeSymbol? underlyingType)
    {
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable)
        {
            underlyingType = nullable.TypeArguments[0];
            return true;
        }

        underlyingType = null;
        return false;
    }

    private bool IsParsableFromString(ITypeSymbol type, out StringParsableTypeKind kind)
    {
        kind = StringParsableTypeKind.None;
        if (type is IArrayTypeSymbol { ElementType.SpecialType: SpecialType.System_Byte })
        {
            kind = StringParsableTypeKind.ByteArray;
            return true;
        }

        if (type is not INamedTypeSymbol named)
        {
            return false;
        }

        if (IsEnum(named))
        {
            kind = StringParsableTypeKind.Enum;
            return true;
        }

        kind = named.SpecialType switch
        {
            SpecialType.System_String or SpecialType.System_Object => StringParsableTypeKind.AssignFromSectionValue,
            SpecialType.System_Boolean or SpecialType.System_Char => StringParsableTypeKind.Parse,
            SpecialType.System_Single or SpecialType.System_Double or SpecialType.System_Decimal => StringParsableTypeKind.Float,
            SpecialType.System_Byte
             or SpecialType.System_Int16
             or SpecialType.System_Int32
             or SpecialType.System_Int64
             or SpecialType.System_SByte
             or SpecialType.System_UInt16
             or SpecialType.System_UInt32
             or SpecialType.System_UInt64 => StringParsableTypeKind.Integer,
            SpecialType.System_DateTime => StringParsableTypeKind.ParseInvariant,
            SpecialType.None when Is(type, _types.CultureInfo) => StringParsableTypeKind.CultureInfo,
            SpecialType.None when Is(type, _types.DateTimeOffset)
             || Is(type, _types.DateOnly)
             || Is(type, _types.TimeOnly)
             || Is(type, _types.TimeSpan)
             || Is(type, _types.Int128)
             || Is(type, _types.Half)
             || Is(type, _types.UInt128) => StringParsableTypeKind.ParseInvariant,
            SpecialType.None when Is(type, _types.Uri) => StringParsableTypeKind.Uri,
            SpecialType.None when Is(type, _types.Version) || Is(type, _types.Guid) => StringParsableTypeKind.Parse,
            _ => StringParsableTypeKind.None,
        };
        return kind != StringParsableTypeKind.None;

        static bool Is(ITypeSymbol type, ITypeSymbol? known) => SymbolEqualityComparer.Default.Equals(type, known);
    }

    private TypeSpec CreateArraySpec(IArrayTypeSymbol type) =>
        type.Rank > 1 || IsUnsupportedType(type.ElementType)
            ? new UnsupportedTypeSpec(type)
            : new ArraySpec(type, EnqueueTransitiveType(type.ElementType));

    private TypeSpec? CreateCollectionSpec(INamedTypeSymbol type) =>
        IsCandidateDictionary(type, out var keyType, out var elementType)
            ? CreateDictionarySpec(type, keyType, elementType)
            : CreateEnumerableSpec(type);

    private TypeSpec? CreateDictionarySpec(INamedTypeSymbol type, ITypeSymbol keyType, ITypeSymbol elementType)
    {
        // Unsupported when it implements IDictionary<,>; otherwise it's treated as an object instead.
        var isDictionary = GetInterface(type, _types.GenericIDictionaryUnbound) is not null;
        if (IsUnsupportedType(keyType) || IsUnsupportedType(elementType))
        {
            return isDictionary ? new UnsupportedTypeSpec(type) : null;
        }

        CollectionInstantiationStrategy strategy;
        CollectionInstantiationConcreteType concreteType;
        CollectionPopulationCastType castType;
        var shouldTryCast = false;
        if (HasPublicParameterlessCtor(type))
        {
            strategy = CollectionInstantiationStrategy.ParameterlessConstructor;
            concreteType = CollectionInstantiationConcreteType.Self;
            if (HasAddMethod(type, keyType, elementType))
            {
                castType = CollectionPopulationCastType.NotApplicable;
            }
            else if (isDictionary)
            {
                castType = CollectionPopulationCastType.IDictionary;
            }
            else
            {
                return null;
            }
        }
        else if (IsInterfaceMatch(type, _types.GenericIDictionaryUnbound) || IsInterfaceMatch(type, _types.IDictionary))
        {
            strategy = CollectionInstantiationStrategy.ParameterlessConstructor;
            concreteType = CollectionInstantiationConcreteType.Dictionary;
            castType = CollectionPopulationCastType.NotApplicable;
        }
        else if (IsInterfaceMatch(type, _types.IReadOnlyDictionaryUnbound))
        {
            strategy = CollectionInstantiationStrategy.LinqToDictionary;
            concreteType = CollectionInstantiationConcreteType.Dictionary;
            castType = CollectionPopulationCastType.IDictionary;
            shouldTryCast = true;
        }
        else
        {
            return isDictionary ? new UnsupportedTypeSpec(type) : null;
        }

        var keyTypeRef = EnqueueTransitiveType(keyType);
        var elementTypeRef = EnqueueTransitiveType(elementType);
        return new DictionarySpec(type, keyTypeRef, elementTypeRef, strategy, concreteType, castType, shouldTryCast);
    }

    private TypeSpec? CreateEnumerableSpec(INamedTypeSymbol type)
    {
        var isCollection = GetInterface(type, _types.GenericICollectionUnbound) is not null;
        if (!TryGetElementType(type, out var elementType) || IsUnsupportedType(elementType))
        {
            return isCollection ? new UnsupportedTypeSpec(type) : null;
        }

        CollectionInstantiationStrategy strategy;
        CollectionInstantiationConcreteType concreteType;
        CollectionPopulationCastType castType;
        var shouldTryCast = false;
        if (HasPublicParameterlessCtor(type))
        {
            strategy = CollectionInstantiationStrategy.ParameterlessConstructor;
            concreteType = CollectionInstantiationConcreteType.Self;
            if (HasAddMethod(type, elementType))
            {
                castType = CollectionPopulationCastType.NotApplicable;
            }
            else if (isCollection)
            {
                castType = CollectionPopulationCastType.ICollection;
            }
            else
            {
                return null;
            }
        }
        else if (IsInterfaceMatch(type, _types.GenericICollectionUnbound) || IsInterfaceMatch(type, _types.GenericIListUnbound))
        {
            strategy = CollectionInstantiationStrategy.ParameterlessConstructor;
            concreteType = CollectionInstantiationConcreteType.List;
            castType = CollectionPopulationCastType.NotApplicable;
        }
        else if (IsInterfaceMatch(type, _types.GenericIEnumerableUnbound))
        {
            strategy = CollectionInstantiationStrategy.CopyConstructor;
            concreteType = CollectionInstantiationConcreteType.List;
            castType = CollectionPopulationCastType.ICollection;
            shouldTryCast = true;
        }
        else if (IsInterfaceMatch(type, _types.ISetUnbound))
        {
            strategy = CollectionInstantiationStrategy.ParameterlessConstructor;
            concreteType = CollectionInstantiationConcreteType.HashSet;
            castType = CollectionPopulationCastType.NotApplicable;
        }
        else if (IsInterfaceMatch(type, _types.IReadOnlySetUnbound))
        {
            strategy = CollectionInstantiationStrategy.CopyConstructor;
            concreteType = CollectionInstantiationConcreteType.HashSet;
            castType = CollectionPopulationCastType.ISet;
            shouldTryCast = true;
        }
        else if (IsInterfaceMatch(type, _types.IReadOnlyListUnbound) || IsInterfaceMatch(type, _types.IReadOnlyCollectionUnbound))
        {
            strategy = CollectionInstantiationStrategy.CopyConstructor;
            concreteType = CollectionInstantiationConcreteType.List;
            castType = CollectionPopulationCastType.ICollection;
            shouldTryCast = true;
        }
        else
        {
            return isCollection ? new UnsupportedTypeSpec(type) : null;
        }

        return new EnumerableSpec(type, EnqueueTransitiveType(elementType), strategy, concreteType, castType, shouldTryCast);
    }

    private bool IsAssignableTo(ITypeSymbol source, ITypeSymbol? destination)
    {
        if (destination is null)
        {
            return false;
        }

        var conversion = _types.Compilation.ClassifyCommonConversion(source, destination);
        return conversion is { IsReference: true, IsImplicit: true };
    }

    private bool IsUnsupportedType(ITypeSymbol type, HashSet<ITypeSymbol>? visitedTypes = null)
    {
        if (ContainsErrorType(type))
        {
            return true;
        }

        if (type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            type = ( (INamedTypeSymbol)type ).TypeArguments[0];
        }

        if (type.SpecialType is SpecialType.System_IntPtr or SpecialType.System_UIntPtr
         || SymbolEqualityComparer.Default.Equals(_types.SerializationInfo, type)
         || SymbolEqualityComparer.Default.Equals(_types.ParameterInfo, type)
         || IsAssignableTo(type, _types.MemberInfo)
         || IsAssignableTo(type, _types.Delegate))
        {
            return true;
        }

        // A recursive collection (class Tree : Dictionary<string, Tree>) is checked by the first call.
        if (visitedTypes?.Contains(type) is true)
        {
            return false;
        }

        var arrayType = type as IArrayTypeSymbol;
        if (arrayType is null && !IsCollection(type))
        {
            return false;
        }

        if (visitedTypes is null)
        {
            visitedTypes = _visitedTypes;
            visitedTypes.Clear();
        }

        visitedTypes.Add(type);
        if (arrayType is not null)
        {
            return arrayType.Rank > 1 || IsUnsupportedType(arrayType.ElementType, visitedTypes);
        }

        var collectionType = (INamedTypeSymbol)type;
        return IsCandidateDictionary(collectionType, out var keyType, out var elementType)
            ? IsUnsupportedType(keyType, visitedTypes) || IsUnsupportedType(elementType, visitedTypes)
            : TryGetElementType(collectionType, out var itemType) && IsUnsupportedType(itemType, visitedTypes);
    }

    private bool ConstructorParametersContainUnsupportedType(IMethodSymbol constructor) => constructor.Parameters.Any(z => IsUnsupportedType(z.Type));

    private static bool BacksConstructorParameter(IMethodSymbol? constructor, string propertyName) =>
        constructor?.Parameters.Any(z => string.Equals(z.Name, propertyName, StringComparison.OrdinalIgnoreCase)) is true;

    private ObjectSpec CreateObjectSpec(INamedTypeSymbol type)
    {
        var strategy = ObjectInstantiationStrategy.None;
        string? initExceptionMessage = null;
        string? initFailureReason = null;
        IMethodSymbol? constructor = null;
        if (!( type.IsAbstract || type.TypeKind is TypeKind.Interface ))
        {
            IMethodSymbol? parameterless = null;
            IMethodSymbol? parameterized = null;
            var hasMultipleParameterized = false;
            foreach (var candidate in type.InstanceConstructors)
            {
                if (candidate.DeclaredAccessibility is not Accessibility.Public)
                {
                    continue;
                }

                if (candidate.Parameters.Length == 0)
                {
                    parameterless = candidate;
                }
                else if (!ConstructorParametersContainUnsupportedType(candidate))
                {
                    if (parameterized is not null)
                    {
                        hasMultipleParameterized = true;
                    }
                    else
                    {
                        parameterized = candidate;
                    }
                }
            }

            var hasPublicParameterless = type.IsValueType || parameterless is not null;
            if (!hasPublicParameterless && hasMultipleParameterized)
            {
                initExceptionMessage = string.Format(CultureInfo.InvariantCulture, MultipleParameterizedConstructors, type.GetTypeNames().FullName);
                initFailureReason = "it has several public constructors with parameters and no parameterless one";
            }

            // Roslyn lists a struct's implicit parameterless constructor, unlike System.Reflection.
            constructor = type.IsValueType ? parameterized ?? parameterless : parameterless ?? parameterized;
        }

        if (constructor is null)
        {
            initExceptionMessage = string.Format(CultureInfo.InvariantCulture, MissingPublicInstanceConstructor, type.GetTypeNames().FullName);
            initFailureReason ??= "it has no public constructor the binder can use";
        }
        else
        {
            strategy = constructor.Parameters.Length == 0 ? ObjectInstantiationStrategy.ParameterlessConstructor : ObjectInstantiationStrategy.ParameterizedConstructor;
        }

        var properties = new Dictionary<string, PropertySpec>(StringComparer.OrdinalIgnoreCase);
        var order = new List<string>();
        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var member in current.GetMembers())
            {
                if (member is not IPropertySymbol { IsIndexer: false, IsImplicitlyDeclared: false } property)
                {
                    continue;
                }

                if (IsUnsupportedType(property.Type) || property.IsOverride || properties.ContainsKey(property.Name))
                {
                    continue;
                }

                var attributes = property.GetAttributes();
                var keyName = attributes.FirstOrDefault(z => SymbolEqualityComparer.Default.Equals(z.AttributeClass, _types.ConfigurationKeyNameAttribute))
                                        ?.ConstructorArguments.FirstOrDefault()
                                        .Value as string
                 ?? property.Name;
                var isIgnored = _types.ConfigurationIgnoreAttribute is { } ignore
                 && attributes.Any(z => SymbolEqualityComparer.Default.Equals(z.AttributeClass, ignore));
                var spec = new PropertySpec(property, TypeRef.From(property.Type), keyName, isIgnored);
                if (!spec.IsIgnored && ( spec.CanGet || spec.CanSet || BacksConstructorParameter(constructor, property.Name) ))
                {
                    EnqueueTransitiveType(property.Type, spec.TypeRef);
                }

                properties[property.Name] = spec;
                order.Add(property.Name);
            }
        }

        var parameters = new List<ParameterSpec>();
        if (strategy is ObjectInstantiationStrategy.ParameterizedConstructor)
        {
            List<string>? missing = null;
            List<string>? invalid = null;
            foreach (var parameter in constructor!.Parameters)
            {
                if (!properties.TryGetValue(parameter.Name, out var property) || property.IsIgnored)
                {
                    ( missing ??= [] ).Add(parameter.Name);
                }
                else if (parameter.RefKind is not RefKind.None)
                {
                    ( invalid ??= [] ).Add(parameter.Name);
                }
                else
                {
                    var parameterSpec = new ParameterSpec(parameter, property.TypeRef, property.ConfigurationKeyName);
                    property.MatchingCtorParam = parameterSpec;
                    parameters.Add(parameterSpec);
                }
            }

            if (invalid is { Count: > 0 })
            {
                initExceptionMessage = string.Format(CultureInfo.InvariantCulture, CannotBindToConstructorParameter, type.GetTypeNames().FullName, string.Join(",", invalid));
                initFailureReason = $"its constructor parameters can't be in, out or ref ('{string.Join("', '", invalid)}')";
            }
            else if (missing is { Count: > 0 })
            {
                if (type.IsValueType)
                {
                    strategy = ObjectInstantiationStrategy.ParameterlessConstructor;
                }
                else
                {
                    initExceptionMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        ConstructorParametersDoNotMatchProperties,
                        type.GetTypeNames().FullName,
                        string.Join(",", missing)
                    );
                    initFailureReason = $"its constructor parameters '{string.Join("', '", missing)}' have no matching bound property";
                }
            }
        }

        var requiredMembers = new List<string>();
        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var member in current.GetMembers())
            {
                if (member is IPropertySymbol { IsRequired: true, IsStatic: false } or IFieldSymbol { IsRequired: true, IsStatic: false }
                 && !requiredMembers.Contains(member.Name))
                {
                    requiredMembers.Add(member.Name);
                }
            }
        }

        // Dovetail: a type that can only be created through an object initializer (see ObjectSpec.InitializerOnly).
        var initializerOnly = strategy is ObjectInstantiationStrategy.ParameterlessConstructor
         && ( requiredMembers.Count > 0 || properties.Values.Any(z => z is { SetOnInit: true, IsIgnored: false, IsStatic: false }) );
        if (initializerOnly)
        {
            strategy = ObjectInstantiationStrategy.ParameterizedConstructor;
        }

        return new(
            type,
            strategy,
            order.Select(z => properties[z]).ToEquatableArray(),
            parameters.ToEquatableArray(),
            initExceptionMessage,
            initFailureReason,
            initializerOnly,
            requiredMembers.ToEquatableArray()
        );
    }

    private bool TryGetElementType(INamedTypeSymbol type, [NotNullWhen(true)] out ITypeSymbol? elementType)
    {
        elementType = GetInterface(type, _types.GenericIEnumerableUnbound)?.TypeArguments[0];
        return elementType is not null;
    }

    private bool IsCandidateDictionary(INamedTypeSymbol type, [NotNullWhen(true)] out ITypeSymbol? keyType, [NotNullWhen(true)] out ITypeSymbol? elementType)
    {
        if (( GetInterface(type, _types.GenericIDictionaryUnbound) ?? GetInterface(type, _types.IReadOnlyDictionaryUnbound) ) is { } candidate)
        {
            keyType = candidate.TypeArguments[0];
            elementType = candidate.TypeArguments[1];
            return true;
        }

        if (IsInterfaceMatch(type, _types.IDictionary))
        {
            keyType = _types.String;
            elementType = _types.String;
            return true;
        }

        keyType = null;
        elementType = null;
        return false;
    }

    private bool IsCollection(ITypeSymbol type) => type is INamedTypeSymbol named && GetInterface(named, _types.IEnumerable) is not null;

    private static INamedTypeSymbol? GetInterface(INamedTypeSymbol type, INamedTypeSymbol? @interface)
    {
        if (@interface is null)
        {
            return null;
        }

        if (IsInterfaceMatch(type, @interface))
        {
            return type;
        }

        return @interface.IsGenericType
            ? type.AllInterfaces.FirstOrDefault(
                z => z.IsGenericType && SymbolEqualityComparer.Default.Equals(z.ConstructUnboundGenericType(), @interface)
            )
            : type.AllInterfaces.FirstOrDefault(z => SymbolEqualityComparer.Default.Equals(z, @interface));
    }

    private static bool IsInterfaceMatch(INamedTypeSymbol type, INamedTypeSymbol? @interface) =>
        @interface is not null
     && ( type.IsGenericType
            ? SymbolEqualityComparer.Default.Equals(type.ConstructUnboundGenericType(), @interface)
            : SymbolEqualityComparer.Default.Equals(type, @interface) );

    /// <summary>
    ///     Whether an unresolved type is involved, in the type, an array element, a type argument or a containing type. Binding
    ///     code that names one doesn't compile, so such members are skipped.
    /// </summary>
    private static bool ContainsErrorType(ITypeSymbol type) =>
        type.TypeKind is TypeKind.Error
     || ( type is IArrayTypeSymbol array && ContainsErrorType(array.ElementType) )
     || ( type is INamedTypeSymbol { IsGenericType: true } generic && generic.TypeArguments.Any(ContainsErrorType) )
     || ( type.ContainingType is { } containing && ContainsErrorType(containing) );

    private static bool HasPublicParameterlessCtor(INamedTypeSymbol type) =>
        type.InstanceConstructors.Count(z => z.DeclaredAccessibility is Accessibility.Public && z.Parameters.IsEmpty) == 1;

    private static bool HasAddMethod(INamedTypeSymbol type, ITypeSymbol element)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.GetMembers("Add")
                       .Any(z => z is IMethodSymbol { Parameters.Length: 1 } method && SymbolEqualityComparer.Default.Equals(element, method.Parameters[0].Type)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasAddMethod(INamedTypeSymbol type, ITypeSymbol key, ITypeSymbol element)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            if (current.GetMembers("Add")
                       .Any(
                            z => z is IMethodSymbol { Parameters.Length: 2 } method
                             && SymbolEqualityComparer.Default.Equals(key, method.Parameters[0].Type)
                             && SymbolEqualityComparer.Default.Equals(element, method.Parameters[1].Type)
                        ))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsEnum(ITypeSymbol type) => type is INamedTypeSymbol { EnumUnderlyingType: not null };

    /// <summary>The well-known types the analysis compares against.</summary>
    private sealed class KnownTypes
    {
        public KnownTypes(Compilation compilation, BinderVersion version)
        {
            Compilation = compilation;
            String = compilation.GetSpecialType(SpecialType.System_String);
            CultureInfo = compilation.GetTypeByMetadataName("System.Globalization.CultureInfo");
            DateOnly = compilation.GetTypeByMetadataName("System.DateOnly");
            DateTimeOffset = compilation.GetTypeByMetadataName("System.DateTimeOffset");
            Guid = compilation.GetTypeByMetadataName("System.Guid");
            Half = compilation.GetTypeByMetadataName("System.Half");
            Int128 = compilation.GetTypeByMetadataName("System.Int128");
            TimeOnly = compilation.GetTypeByMetadataName("System.TimeOnly");
            TimeSpan = compilation.GetTypeByMetadataName("System.TimeSpan");
            UInt128 = compilation.GetTypeByMetadataName("System.UInt128");
            Uri = compilation.GetTypeByMetadataName("System.Uri");
            Version = compilation.GetTypeByMetadataName("System.Version");

            // [ConfigurationIgnore] is new in .NET 11; version 10 of the generator doesn't know it.
            ConfigurationIgnoreAttribute = version >= BinderVersion.V11
                ? compilation.GetTypeByMetadataName("Microsoft.Extensions.Configuration.ConfigurationIgnoreAttribute")
                : null;
            ConfigurationKeyNameAttribute = compilation.GetTypeByMetadataName("Microsoft.Extensions.Configuration.ConfigurationKeyNameAttribute");
            IConfigurationSection = compilation.GetTypeByMetadataName("Microsoft.Extensions.Configuration.IConfigurationSection");

            IEnumerable = compilation.GetSpecialType(SpecialType.System_Collections_IEnumerable);
            IDictionary = compilation.GetTypeByMetadataName("System.Collections.IDictionary");
            GenericICollectionUnbound = compilation.GetSpecialType(SpecialType.System_Collections_Generic_ICollection_T).ConstructUnboundGenericType();
            GenericIDictionaryUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.IDictionary`2")?.ConstructUnboundGenericType();
            GenericIEnumerableUnbound = compilation.GetSpecialType(SpecialType.System_Collections_Generic_IEnumerable_T).ConstructUnboundGenericType();
            GenericIListUnbound = compilation.GetSpecialType(SpecialType.System_Collections_Generic_IList_T).ConstructUnboundGenericType();
            IReadOnlyDictionaryUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyDictionary`2")?.ConstructUnboundGenericType();
            IReadOnlyCollectionUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyCollection`1")?.ConstructUnboundGenericType();
            IReadOnlyListUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyList`1")?.ConstructUnboundGenericType();
            IReadOnlySetUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlySet`1")?.ConstructUnboundGenericType();
            ISetUnbound = compilation.GetTypeByMetadataName("System.Collections.Generic.ISet`1")?.ConstructUnboundGenericType();

            SerializationInfo = compilation.GetTypeByMetadataName("System.Runtime.Serialization.SerializationInfo");
            MemberInfo = compilation.GetTypeByMetadataName("System.Reflection.MemberInfo");
            ParameterInfo = compilation.GetTypeByMetadataName("System.Reflection.ParameterInfo");
            Delegate = compilation.GetSpecialType(SpecialType.System_Delegate);
        }

        public Compilation Compilation { get; }
        public INamedTypeSymbol String { get; }
        public INamedTypeSymbol? CultureInfo { get; }
        public INamedTypeSymbol? DateOnly { get; }
        public INamedTypeSymbol? DateTimeOffset { get; }
        public INamedTypeSymbol? Guid { get; }
        public INamedTypeSymbol? Half { get; }
        public INamedTypeSymbol? Int128 { get; }
        public INamedTypeSymbol? TimeOnly { get; }
        public INamedTypeSymbol? TimeSpan { get; }
        public INamedTypeSymbol? UInt128 { get; }
        public INamedTypeSymbol? Uri { get; }
        public INamedTypeSymbol? Version { get; }
        public INamedTypeSymbol? ConfigurationIgnoreAttribute { get; }
        public INamedTypeSymbol? ConfigurationKeyNameAttribute { get; }
        public INamedTypeSymbol? IConfigurationSection { get; }
        public INamedTypeSymbol IEnumerable { get; }
        public INamedTypeSymbol? IDictionary { get; }
        public INamedTypeSymbol GenericICollectionUnbound { get; }
        public INamedTypeSymbol? GenericIDictionaryUnbound { get; }
        public INamedTypeSymbol GenericIEnumerableUnbound { get; }
        public INamedTypeSymbol GenericIListUnbound { get; }
        public INamedTypeSymbol? IReadOnlyDictionaryUnbound { get; }
        public INamedTypeSymbol? IReadOnlyCollectionUnbound { get; }
        public INamedTypeSymbol? IReadOnlyListUnbound { get; }
        public INamedTypeSymbol? IReadOnlySetUnbound { get; }
        public INamedTypeSymbol? ISetUnbound { get; }
        public INamedTypeSymbol? SerializationInfo { get; }
        public INamedTypeSymbol? MemberInfo { get; }
        public INamedTypeSymbol? ParameterInfo { get; }
        public INamedTypeSymbol Delegate { get; }
    }
}
