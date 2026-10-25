// Ported from dotnet/runtime (MIT): TypeIndex and BindingHelperInfo.Builder. See BindingModel.cs.

namespace Dovetail.Support.Configuration.Binding;

/// <summary>Answers what can be bound, created and generated, over a set of type specs.</summary>
internal sealed class BindingTypeIndex
{
    private readonly Dictionary<TypeRef, TypeSpec> _index = [];

    public BindingTypeIndex(IEnumerable<TypeSpec> specs)
    {
        foreach (var spec in specs)
        {
            // The same type analysed for two sections gives the same spec; keep the first.
            if (!_index.ContainsKey(spec.TypeRef))
            {
                _index[spec.TypeRef] = spec;
            }
        }
    }

    public IEnumerable<TypeSpec> Specs => _index.Values;

    public bool CanBindTo(TypeRef typeRef) => GetEffectiveTypeSpec(typeRef) switch
    {
        SimpleTypeSpec => true,
        ComplexTypeSpec complex => CanInstantiate(complex) || HasBindableMembers(complex),
        _ => false,
    };

    public bool CanInstantiate(ComplexTypeSpec spec) => spec switch
    {
        ObjectSpec objectSpec => objectSpec is { InstantiationStrategy: not ObjectInstantiationStrategy.None, InitExceptionMessage: null },
        DictionarySpec dictionary => KeyIsSupported(dictionary),
        CollectionSpec collection => CanBindTo(collection.ElementTypeRef),
        _ => false,
    };

    public bool HasBindableMembers(ComplexTypeSpec spec) => spec switch
    {
        ObjectSpec objectSpec => objectSpec.Properties.Any(ShouldBindTo),
        DictionarySpec dictionary => KeyIsSupported(dictionary) && CanBindTo(dictionary.ElementTypeRef),
        CollectionSpec collection => CanBindTo(collection.ElementTypeRef),
        _ => false,
    };

    public bool ShouldBindTo(PropertySpec property)
    {
        if (property.IsIgnored || !( property.CanGet || property.CanSet ))
        {
            return false;
        }

        var propertyType = GetEffectiveTypeSpec(property.TypeRef);
        var isDictionaryWithUnsupportedKey = propertyType is DictionarySpec dictionary && !KeyIsSupported(dictionary);
        var isCollectionThatCantBeReplaced = !property.CanSet
         && propertyType is CollectionWithCtorInitSpec
            {
                InstantiationStrategy: CollectionInstantiationStrategy.CopyConstructor or CollectionInstantiationStrategy.LinqToDictionary,
            };
        return !isDictionaryWithUnsupportedKey && !isCollectionThatCantBeReplaced;
    }

    public TypeSpec GetEffectiveTypeSpec(TypeRef typeRef) => GetEffectiveTypeSpec(GetTypeSpec(typeRef));

    public TypeSpec GetEffectiveTypeSpec(TypeSpec spec) => spec.EffectiveTypeRef == spec.TypeRef ? spec : _index[spec.EffectiveTypeRef];

    public TypeSpec GetTypeSpec(TypeRef typeRef) => _index[typeRef];

    public bool TryGetTypeSpec(TypeRef typeRef, out TypeSpec spec) => _index.TryGetValue(typeRef, out spec!);

    /// <summary>Only types parsable from a string can be keys; nullable keys would break the dictionary's notnull constraint.</summary>
    public bool KeyIsSupported(DictionarySpec spec) => GetTypeSpec(spec.KeyTypeRef) is ParsableFromStringSpec;

    public static string GetInstantiationTypeDisplayString(CollectionWithCtorInitSpec type) =>
        type.InstantiationConcreteType is CollectionInstantiationConcreteType.Self
            ? type.TypeRef.FullyQualifiedName
            : GetGenericTypeDisplayString(type, type.InstantiationConcreteType.ToString());

    public static string GetPopulationCastTypeDisplayString(CollectionWithCtorInitSpec type) => GetGenericTypeDisplayString(type, type.PopulationCastType.ToString());

    private static string GetGenericTypeDisplayString(CollectionWithCtorInitSpec type, string proxyTypeName)
    {
        // The proxy type (List, HashSet, Dictionary, ICollection, …) lives in the same namespace as the collection type.
        var name = type.TypeRef.FullyQualifiedName;
        var genericIndex = name.IndexOf('<');
        var lastDotIndex = genericIndex > 0 ? name.LastIndexOf('.', genericIndex) : -1;
        var proxy = lastDotIndex >= 0 ? name[..( lastDotIndex + 1 )] + proxyTypeName : proxyTypeName;
        return type is DictionarySpec dictionary
            ? $"{proxy}<{dictionary.KeyTypeRef.FullyQualifiedName}, {type.ElementTypeRef.FullyQualifiedName}>"
            : $"{proxy}<{type.ElementTypeRef.FullyQualifiedName}>";
    }

    public static string GetParseMethodName(ParsableFromStringSpec type)
    {
        if (type.StringParsableTypeKind is StringParsableTypeKind.ByteArray)
        {
            return "ParseByteArray";
        }

        var name = type.TypeRef.FullyQualifiedName;
        if (name.StartsWith("global::", StringComparison.Ordinal))
        {
            name = name["global::".Length..];
        }

        if (char.IsLower(name[0]))
        {
            name = char.ToUpperInvariant(name[0]) + name[1..];
        }

        return "Parse" + name.Replace(".", "");
    }
}

/// <summary>Which helper methods the binder needs, found by walking the types the roots reach (BindingHelperInfo.Builder).</summary>
internal sealed class BindingMethods(BindingTypeIndex index)
{
    private readonly Dictionary<TypeRef, bool> _seenTransitiveTypes = [];

    public HashSet<ComplexTypeSpec> BindCore { get; } = [];
    public HashSet<ObjectSpec> Initialize { get; } = [];
    public HashSet<ParsableFromStringSpec> ParsePrimitive { get; } = [];
    public bool AsConfigWithChildren { get; private set; }
    public bool ParseEnum { get; private set; }
    public bool UsesLinq { get; private set; }

    /// <summary>Registers what a root needs; returns <see langword="false" /> when it can't be bound.</summary>
    public bool RegisterRoot(TypeRef root)
    {
        AsConfigWithChildren = true;
        return TryRegisterTransitiveTypes(root);
    }

    private bool TryRegisterTransitiveTypes(TypeRef typeRef)
    {
        if (_seenTransitiveTypes.TryGetValue(typeRef, out var isValid))
        {
            return isValid;
        }

        return _seenTransitiveTypes[typeRef] = TryRegisterCore();

        bool TryRegisterCore()
        {
            switch (index.GetTypeSpec(typeRef))
            {
                case NullableSpec nullable:
                    return TryRegisterTransitiveTypes(nullable.EffectiveTypeRef);
                case ParsableFromStringSpec parsable:
                    RegisterParsable(parsable);
                    return true;
                case DictionarySpec dictionary:
                {
                    // The base case for recursive object graphs.
                    _seenTransitiveTypes.Add(typeRef, true);
                    var shouldRegister = index.CanBindTo(typeRef)
                     && TryRegisterTransitiveTypes(dictionary.KeyTypeRef)
                     && TryRegisterTransitiveTypes(dictionary.ElementTypeRef)
                     && TryRegisterBindCore(dictionary);
                    if (shouldRegister && dictionary.InstantiationStrategy is CollectionInstantiationStrategy.LinqToDictionary)
                    {
                        UsesLinq = true;
                    }

                    return shouldRegister;
                }
                case CollectionSpec collection:
                {
                    _seenTransitiveTypes.Add(typeRef, true);
                    if (index.GetTypeSpec(collection.ElementTypeRef) is ComplexTypeSpec)
                    {
                        UsesLinq = true;
                    }

                    return TryRegisterTransitiveTypes(collection.ElementTypeRef) && TryRegisterBindCore(collection);
                }
                case ObjectSpec objectSpec:
                {
                    // Every object type is registered: some throw at run time.
                    _seenTransitiveTypes.Add(typeRef, true);
                    var hasBindableMembers = index.HasBindableMembers(objectSpec);

                    // A parameterized constructor binds its parameters in Initialize even without other bindable members.
                    var needsInitializeMethod = objectSpec is { InstantiationStrategy: ObjectInstantiationStrategy.ParameterizedConstructor, InitExceptionMessage: null };
                    if (hasBindableMembers || needsInitializeMethod)
                    {
                        foreach (var property in objectSpec.Properties)
                        {
                            // Types only reachable through members that aren't bound aren't needed, unless a constructor parameter
                            // binds them.
                            if (!index.ShouldBindTo(property) && property.MatchingCtorParam is null)
                            {
                                continue;
                            }

                            TryRegisterTransitiveTypes(property.TypeRef);
                            if (index.GetTypeSpec(property.TypeRef) is ComplexTypeSpec)
                            {
                                AsConfigWithChildren = true;
                            }
                        }

                        if (hasBindableMembers)
                        {
                            TryRegisterBindCore(objectSpec);
                        }

                        if (needsInitializeMethod)
                        {
                            Initialize.Add(objectSpec);
                        }
                    }

                    return true;
                }
                default:
                    return true;
            }
        }
    }

    private bool TryRegisterBindCore(ComplexTypeSpec type)
    {
        if (!index.HasBindableMembers(type))
        {
            return false;
        }

        BindCore.Add(type);
        return true;
    }

    private void RegisterParsable(ParsableFromStringSpec type)
    {
        if (type.StringParsableTypeKind is StringParsableTypeKind.AssignFromSectionValue)
        {
            return;
        }

        if (type.StringParsableTypeKind is StringParsableTypeKind.Enum)
        {
            ParseEnum = true;
        }
        else
        {
            ParsePrimitive.Add(type);
        }
    }
}
