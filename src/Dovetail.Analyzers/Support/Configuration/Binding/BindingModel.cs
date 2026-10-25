// The binding model and the rules in this folder are ported from the configuration binding source generator in
// dotnet/runtime (src/libraries/Microsoft.Extensions.Configuration.Binder/gen), Copyright (c) .NET Foundation and
// Contributors, licensed under the MIT license. Dovetail emits its own binder because a generator can't see another
// generator's output, so Microsoft's generator never intercepts calls in Dovetail's generated code.

using Microsoft.CodeAnalysis;

namespace Dovetail.Support.Configuration.Binding;

/// <summary>The binder rules to follow: Microsoft's generator changed them between versions 10 and 11.</summary>
internal enum BinderVersion
{
    V10 = 10,
    V11 = 11,
}

/// <summary>An equatable reference to a type. Identity is the fully qualified name.</summary>
internal sealed record TypeRef(string FullyQualifiedName, bool IsValueType, SpecialType SpecialType)
{
    public static TypeRef From(ITypeSymbol type) =>
        new(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat), type.IsValueType, type.OriginalDefinition.SpecialType);

    public bool CanBeNull => !IsValueType || SpecialType == SpecialType.System_Nullable_T;

    public bool Equals(TypeRef? other) => other is not null && string.Equals(FullyQualifiedName, other.FullyQualifiedName, StringComparison.Ordinal);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(FullyQualifiedName);
}

internal abstract record TypeSpec
{
    protected TypeSpec(ITypeSymbol type)
    {
        TypeRef = TypeRef.From(type);
        EffectiveTypeRef = TypeRef;
        (DisplayString, FullName) = type.GetTypeNames();
        IdentifierCompatibleSubstring = type.ToIdentifierCompatibleSubstring();
        IsValueType = type.IsValueType;
        if (type is INamedTypeSymbol named)
        {
            IsValueTuple = named.IsTupleType;
            IsExactIEnumerableOfT = named.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T;
        }
    }

    public TypeRef TypeRef { get; }

    /// <summary>The type that's bound: the underlying type for <c>Nullable&lt;T&gt;</c>, otherwise the type itself.</summary>
    public TypeRef EffectiveTypeRef { get; protected init; }

    /// <summary>A <c>Type.FullName</c>-like rendering, used in exception messages.</summary>
    public string FullName { get; }

    /// <summary>The name without the namespace.</summary>
    public string DisplayString { get; }

    public string IdentifierCompatibleSubstring { get; }
    public bool IsValueType { get; }
    public bool IsValueTuple { get; }
    public bool IsExactIEnumerableOfT { get; }
}

internal abstract record ComplexTypeSpec : TypeSpec
{
    protected ComplexTypeSpec(ITypeSymbol type) : base(type) { }
}

internal sealed record NullableSpec : TypeSpec
{
    public NullableSpec(ITypeSymbol type, TypeRef underlyingTypeRef) : base(type) => EffectiveTypeRef = underlyingTypeRef;
}

internal sealed record UnsupportedTypeSpec : TypeSpec
{
    public UnsupportedTypeSpec(ITypeSymbol type) : base(type) { }
}

internal abstract record SimpleTypeSpec : TypeSpec
{
    protected SimpleTypeSpec(ITypeSymbol type) : base(type) { }
}

internal sealed record ConfigurationSectionSpec : SimpleTypeSpec
{
    public ConfigurationSectionSpec(ITypeSymbol type) : base(type) { }
}

internal sealed record ParsableFromStringSpec : SimpleTypeSpec
{
    public ParsableFromStringSpec(ITypeSymbol type, StringParsableTypeKind kind) : base(type) => StringParsableTypeKind = kind;

    public StringParsableTypeKind StringParsableTypeKind { get; }
}

internal enum StringParsableTypeKind
{
    None = 0,

    /// <summary><c>string</c> and <c>object</c>: assigned the section value as is.</summary>
    AssignFromSectionValue = 1,
    Enum = 2,
    ByteArray = 3,
    Integer = 4,
    Float = 5,
    Parse = 6,
    ParseInvariant = 7,
    CultureInfo = 8,
    Uri = 9,
}

internal sealed record ObjectSpec : ComplexTypeSpec
{
    public ObjectSpec(
        INamedTypeSymbol type,
        ObjectInstantiationStrategy instantiationStrategy,
        EquatableArray<PropertySpec> properties,
        EquatableArray<ParameterSpec> constructorParameters,
        string? initExceptionMessage,
        string? initFailureReason,
        bool initializerOnly,
        EquatableArray<string> requiredMembers
    ) : base(type)
    {
        InstantiationStrategy = instantiationStrategy;
        Properties = properties;
        ConstructorParameters = constructorParameters;
        InitExceptionMessage = initExceptionMessage;
        InitFailureReason = initFailureReason;
        InitializerOnly = initializerOnly;
        RequiredMembers = requiredMembers;
    }

    public ObjectInstantiationStrategy InstantiationStrategy { get; }
    public EquatableArray<PropertySpec> Properties { get; }
    public EquatableArray<ParameterSpec> ConstructorParameters { get; }
    public string? InitExceptionMessage { get; }

    /// <summary>Why the type can't be created, worded for a diagnostic ("… because {reason}").</summary>
    public string? InitFailureReason { get; }

    /// <summary>
    ///     Dovetail's addition: a class with a parameterless constructor and init-only or <c>required</c> members. Microsoft's
    ///     generator never sets those (and can't create a type with required members at all), so Dovetail creates it with an
    ///     object initializer, as if through a constructor without parameters, and a missing key keeps the member's own
    ///     initial value, as the reflection binder does.
    /// </summary>
    public bool InitializerOnly { get; }

    /// <summary>
    ///     The names of the type's <c>required</c> properties and fields. Creating it has to set them all, so the ones that
    ///     aren't bound are set to <c>default</c>; Microsoft's generator doesn't, and its code doesn't compile then (CS9035).
    /// </summary>
    public EquatableArray<string> RequiredMembers { get; }
}

internal enum ObjectInstantiationStrategy
{
    None = 0,
    ParameterlessConstructor = 1,
    ParameterizedConstructor = 2,
}

internal abstract record CollectionSpec : ComplexTypeSpec
{
    protected CollectionSpec(ITypeSymbol type, TypeRef elementTypeRef) : base(type) => ElementTypeRef = elementTypeRef;

    public TypeRef ElementTypeRef { get; }
}

internal abstract record CollectionWithCtorInitSpec : CollectionSpec
{
    protected CollectionWithCtorInitSpec(
        ITypeSymbol type,
        TypeRef elementTypeRef,
        CollectionInstantiationStrategy instantiationStrategy,
        CollectionInstantiationConcreteType instantiationConcreteType,
        CollectionPopulationCastType populationCastType,
        bool shouldTryCast
    ) : base(type, elementTypeRef)
    {
        InstantiationStrategy = instantiationStrategy;
        InstantiationConcreteType = instantiationConcreteType;
        PopulationCastType = populationCastType;
        ShouldTryCast = shouldTryCast;
    }

    public CollectionInstantiationStrategy InstantiationStrategy { get; }
    public CollectionInstantiationConcreteType InstantiationConcreteType { get; }
    public CollectionPopulationCastType PopulationCastType { get; }
    public bool ShouldTryCast { get; }
}

internal sealed record ArraySpec : CollectionSpec
{
    public ArraySpec(ITypeSymbol type, TypeRef elementTypeRef) : base(type, elementTypeRef) { }
}

internal sealed record EnumerableSpec : CollectionWithCtorInitSpec
{
    public EnumerableSpec(
        ITypeSymbol type,
        TypeRef elementTypeRef,
        CollectionInstantiationStrategy instantiationStrategy,
        CollectionInstantiationConcreteType instantiationConcreteType,
        CollectionPopulationCastType populationCastType,
        bool shouldTryCast
    ) : base(type, elementTypeRef, instantiationStrategy, instantiationConcreteType, populationCastType, shouldTryCast) { }
}

internal sealed record DictionarySpec : CollectionWithCtorInitSpec
{
    public DictionarySpec(
        INamedTypeSymbol type,
        TypeRef keyTypeRef,
        TypeRef elementTypeRef,
        CollectionInstantiationStrategy instantiationStrategy,
        CollectionInstantiationConcreteType instantiationConcreteType,
        CollectionPopulationCastType populationCastType,
        bool shouldTryCast
    ) : base(type, elementTypeRef, instantiationStrategy, instantiationConcreteType, populationCastType, shouldTryCast) => KeyTypeRef = keyTypeRef;

    public TypeRef KeyTypeRef { get; }
}

internal enum CollectionInstantiationStrategy
{
    NotApplicable = 0,
    ParameterlessConstructor = 1,
    CopyConstructor = 2,
    LinqToDictionary = 3,
}

internal enum CollectionInstantiationConcreteType
{
    Self = 0,
    Dictionary = 1,
    List = 2,
    HashSet = 3,
}

internal enum CollectionPopulationCastType
{
    NotApplicable = 0,
    IDictionary = 1,
    ICollection = 2,
    ISet = 3,
}

internal abstract record MemberSpec
{
    protected MemberSpec(ISymbol member, TypeRef typeRef, string configurationKeyName)
    {
        Name = member.Name;
        TypeRef = typeRef;
        ConfigurationKeyName = configurationKeyName;
    }

    public string Name { get; }
    public string DefaultValueExpr { get; protected init; } = "default";
    public TypeRef TypeRef { get; }
    public string ConfigurationKeyName { get; }
    public abstract bool CanGet { get; }
    public abstract bool CanSet { get; }
}

internal sealed record ParameterSpec : MemberSpec
{
    public ParameterSpec(IParameterSymbol parameter, TypeRef typeRef, string configurationKeyName) : base(parameter, typeRef, configurationKeyName)
    {
        RefKind = parameter.RefKind;
        if (parameter.HasExplicitDefaultValue)
        {
            DefaultValueExpr = BindingSyntax.FormatLiteral(parameter.ExplicitDefaultValue, typeRef);
        }
        else
        {
            ErrorOnFailedBinding = true;
        }
    }

    public bool ErrorOnFailedBinding { get; }
    public RefKind RefKind { get; }
    public override bool CanGet => false;
    public override bool CanSet => true;
}

internal sealed record PropertySpec : MemberSpec
{
    public PropertySpec(IPropertySymbol property, TypeRef typeRef, string configurationKeyName, bool isIgnored) : base(property, typeRef, configurationKeyName)
    {
        var setterIsPublic = property.SetMethod?.DeclaredAccessibility is Accessibility.Public;
        var isInitOnly = property.SetMethod?.IsInitOnly is true;
        IsIgnored = isIgnored;
        IsStatic = property.IsStatic;
        IsRequired = property.IsRequired;
        SetOnInit = setterIsPublic && ( property.IsRequired || isInitOnly );
        CanSet = setterIsPublic && !isInitOnly;
        CanGet = property.GetMethod?.DeclaredAccessibility is Accessibility.Public;
    }

    /// <summary>The constructor parameter the property is bound through, set while the containing type is analysed.</summary>
    public ParameterSpec? MatchingCtorParam { get; set; }

    public bool IsIgnored { get; }
    public bool IsStatic { get; }
    public bool IsRequired { get; }

    /// <summary>Required or init-only with a public setter: it can only be set while the object is created.</summary>
    public bool SetOnInit { get; }

    public override bool CanGet { get; }
    public override bool CanSet { get; }
}
