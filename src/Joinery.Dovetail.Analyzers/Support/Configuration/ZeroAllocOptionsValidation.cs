using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     ZeroAlloc.Validation.Options (https://validation.zeroalloc.net/docs/options): its source generator writes a
///     <c>ValidateWithZeroAlloc()</c> overload of <c>OptionsBuilder&lt;T&gt;</c> for each <c>[Validate]</c> class of the assembly,
///     which registers the class's generated validator as the options type's <c>IValidateOptions&lt;T&gt;</c>. The options joint
///     calls it for the <c>[DovetailConfiguration]</c> types that get one.
/// </summary>
/// <remarks>
///     The overloads are another generator's output, which this generator can't see, so whether one exists is worked out from the
///     rules that generator follows: the package is referenced, the type is a class of this assembly marked <c>[Validate]</c>, and
///     neither it nor a type it's nested in is private or protected (ZV0025). A model with asynchronous rules gets an overload, but
///     calling it is an error (ZV0034), since options validation is synchronous; those types are reported with DT0018 instead.
/// </remarks>
internal static class ZeroAllocOptionsValidation
{
    /// <summary>Set to <c>false</c>, Dovetail leaves ZeroAlloc.Validation's registrations to the project (as Joinery.Dovetail.ZeroAlloc.targets does).</summary>
    public const string EnableProperty = "DovetailEnableZeroAllocValidation";

    /// <summary>ZeroAlloc.Validation's own property: <c>Internal</c> makes the generated types, the overloads' class among them, internal.</summary>
    public const string AccessibilityProperty = "ZeroAllocGeneratedAccessibility";

    private const string Namespace = "ZeroAlloc.Validation";
    private const string ValidateAttributeMetadataName = "ZeroAlloc.Validation.ValidateAttribute";
    private const string OptionsValidatorMetadataName = "ZeroAlloc.Validation.Options.ZeroAllocOptionsValidator`1";

    /// <summary>
    ///     The class the overloads are generated into: <c>ZeroAllocOptionsValidationExtensions</c>, or
    ///     <c>InternalZeroAllocOptionsValidationExtensions</c> when <c>ZeroAllocGeneratedAccessibility</c> is <c>Internal</c>.
    /// </summary>
    public static string ExtensionsClass(string? accessibility) => string.Equals(accessibility?.Trim(), "Internal", StringComparison.OrdinalIgnoreCase)
        ? "global::ZeroAlloc.Validation.Options.InternalZeroAllocOptionsValidationExtensions"
        : "global::ZeroAlloc.Validation.Options.ZeroAllocOptionsValidationExtensions";

    /// <summary>Whether ZeroAlloc.Validation.Options generates a <c>ValidateWithZeroAlloc()</c> overload the joint can call for <paramref name="type" />.</summary>
    /// <param name="type">The options type.</param>
    /// <param name="compilation">The compilation.</param>
    /// <param name="asyncRule">The member with an asynchronous rule, when that is what rules it out.</param>
    public static bool Applies(ITypeSymbol type, Compilation compilation, out string? asyncRule)
    {
        asyncRule = null;
        if (type is not INamedTypeSymbol { TypeKind: TypeKind.Class } named
         || !SymbolEqualityComparer.Default.Equals(named.ContainingAssembly, compilation.Assembly)
         || compilation.GetTypeByMetadataName(OptionsValidatorMetadataName) is null
         || compilation.GetTypeByMetadataName(ValidateAttributeMetadataName) is not { } validateAttribute
         || ValidateAttributeOf(named, validateAttribute) is null)
        {
            return false;
        }

        for (var current = named; current is not null; current = current.ContainingType)
        {
            if (current.DeclaredAccessibility is not ( Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal ))
            {
                return false;
            }
        }

#pragma warning disable IDE0028 // A collection expression can't take the comparer here: the analyzer also builds against Roslyn 5.0 (C# 14).
        asyncRule = FindAsyncRule(named, validateAttribute, new(SymbolEqualityComparer.Default));
#pragma warning restore IDE0028
        return asyncRule is null;
    }

    private static AttributeData? ValidateAttributeOf(ITypeSymbol type, INamedTypeSymbol validateAttribute) => type
                                                                                                               .GetAttributes()
                                                                                                               .FirstOrDefault(
                                                                                                                    z => SymbolEqualityComparer.Default.Equals(z.AttributeClass, validateAttribute)
                                                                                                                );

    /// <summary>
    ///     The first member of <paramref name="type" />, or of a nested or collection <c>[Validate]</c> model it reaches, that has an
    ///     asynchronous rule (an <c>AsyncValidationAttribute&lt;T&gt;</c>), or <see langword="null" />.
    /// </summary>
    private static string? FindAsyncRule(INamedTypeSymbol type, INamedTypeSymbol validateAttribute, HashSet<ITypeSymbol> visited)
    {
        if (!visited.Add(type))
        {
            return null;
        }

        if (type.GetAttributes().Any(IsAsyncRule))
        {
            return type.ToDisplayString();
        }

        // Base properties are validated too, unless [Validate(IncludeBaseProperties = false)] says otherwise.
        var includeBaseProperties = ValidateAttributeOf(type, validateAttribute)
                                  ?.NamedArguments
                                   .Any(z => z is { Key: "IncludeBaseProperties", Value.Value: false })
                                 != true;
        for (var current = type; current is { SpecialType: not SpecialType.System_Object }; current = includeBaseProperties ? current.BaseType : null)
        {
            foreach (var property in current.GetMembers().OfType<IPropertySymbol>())
            {
                if (property is { IsStatic: true } or { IsIndexer: true })
                {
                    continue;
                }

                if (property.GetAttributes().Any(IsAsyncRule))
                {
                    return type.ToDisplayString() + "." + property.Name;
                }

                foreach (var model in Models(property.Type))
                {
                    if (ValidateAttributeOf(model, validateAttribute) is not null && FindAsyncRule(model, validateAttribute, visited) is { } nested)
                    {
                        return nested;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>The type of a member, and the element type when it's a collection.</summary>
    private static IEnumerable<INamedTypeSymbol> Models(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol named)
        {
            yield return named;
        }

        if (type is IArrayTypeSymbol { ElementType: INamedTypeSymbol element })
        {
            yield return element;
        }

        if (type.SpecialType is SpecialType.System_String)
        {
            yield break;
        }

        var interfaces = type is INamedTypeSymbol { TypeKind: TypeKind.Interface } self ? type.AllInterfaces.Add(self) : type.AllInterfaces;
        foreach (var enumerable in interfaces.Where(z => z is { OriginalDefinition.SpecialType: SpecialType.System_Collections_Generic_IEnumerable_T }))
        {
            if (enumerable.TypeArguments[0] is INamedTypeSymbol item)
            {
                yield return item;
            }
        }
    }

    private static bool IsAsyncRule(AttributeData attribute)
    {
        for (var current = attribute.AttributeClass; current is not null; current = current.BaseType)
        {
            if (current is { Name: "AsyncValidationAttribute", Arity: 1 } && current.ContainingNamespace.ToDisplayString() == Namespace)
            {
                return true;
            }
        }

        return false;
    }
}
