using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail.Support;

/// <summary>
///     The checks that decide whether a joint can be exported. The generator uses them to leave a joint out, and
///     <see cref="DovetailJointAnalyzer" /> to report why, so the two always agree.
/// </summary>
internal static class JointChecks
{
    /// <summary>The metadata name of the attribute that exports a joint.</summary>
    public const string ExportAttributeMetadataName = "Joinery.Dovetail.Attributes.DovetailExportAttribute";

    /// <summary>
    ///     Why <paramref name="joint" /> can't be exported, or <see langword="null" /> when it can. When it can,
    ///     <paramref name="constructor" /> is the one constructor the generated code calls, if the joint declares one.
    /// </summary>
    public static DiagnosticDescriptor? Rejection(INamedTypeSymbol joint, out IMethodSymbol? constructor)
    {
        // Only the constructors the generated code can call count: a private one is there for something else to use.
        var count = 0;
        constructor = null;
        foreach (var candidate in joint.InstanceConstructors)
        {
            if (!CanBeCalled(candidate)) continue;
            count++;
            constructor = candidate;
        }

        if (count != 1) constructor = null;

        return count > 1
            ? Diagnostics.DovetailHasToManyConstructors
            // Left out of the exports like any other joint that is reported, so nothing else fails to compile because of it.
            : count == 0 && joint.InstanceConstructors.Length > 0
                ? Diagnostics.DovetailHasNoCallableConstructor
                : joint.IsGenericType
                    ? Diagnostics.DovetailCannotBeGeneric
                    : null;
    }

    /// <summary>
    ///     Whether <paramref name="attributeClass" /> is declared in <c>Joinery.Dovetail.Attributes</c>, checked by walking its
    ///     namespaces rather than formatting its name.
    /// </summary>
    public static bool IsDovetailAttribute(INamedTypeSymbol attributeClass) =>
        attributeClass is
        {
            ContainingType: null,
            ContainingNamespace:
            {
                Name: "Attributes",
                ContainingNamespace: { Name: "Dovetail", ContainingNamespace: { Name: "Joinery", ContainingNamespace.IsGlobalNamespace: true } },
            },
        };

    /// <summary>Whether <paramref name="type" /> carries <c>[DovetailExport]</c> itself, which is what the generator looks for.</summary>
    public static bool IsExported(INamedTypeSymbol type)
    {
        foreach (var attribute in type.GetAttributes())
        {
            if (attribute.AttributeClass is { Name: "DovetailExportAttribute", Arity: 0 } attributeClass && IsDovetailAttribute(attributeClass)) return true;
        }

        return false;
    }

    /// <summary>
    ///     Whether the generated code, which lives in the exporting assembly, can call the constructor. That is every instance
    ///     constructor that is public or internal, and <c>protected internal</c> is internal to this assembly too.
    /// </summary>
    private static bool CanBeCalled(IMethodSymbol constructor) =>
        constructor.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal;
}
