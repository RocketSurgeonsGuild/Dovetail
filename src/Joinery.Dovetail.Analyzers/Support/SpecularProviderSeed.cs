using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail.Support;

/// <summary>
///     Seeds Specular's provider into the context builder the generated <c>Imports</c> creates, so a Joint can read it with
///     <c>context.Get&lt;ISpecularProvider&gt;()</c> without the application setting it by hand.
/// </summary>
/// <remarks>
///     <para>
///         Dovetail takes no dependency on Specular, and <c>SpecularProvider</c> is an internal type that Specular generates in
///         each compilation's global namespace, so only the application's own compilation can name it. That makes the seeding
///         generated code that exists only where Specular does.
///     </para>
///     <para>
///         The provider can't be found as a type while generating: a generator doesn't see what another generator adds, so
///         Specular's <c>SpecularProvider</c> isn't in the compilation Dovetail's generator is given. What it can see is what makes
///         Specular generate one, which is the same two things Specular's generator looks at: the runtime package that defines
///         <c>Specular.ISpecularProvider</c> is referenced, and the <c>SpecularEmitProvider</c> MSBuild property isn't false.
///     </para>
///     <para>
///         The value is added only if the type isn't already there, so a provider the application passes in or sets itself wins.
///     </para>
/// </remarks>
internal static class SpecularProviderSeed
{
    private const string ProviderInterfaceMetadataName = "Specular.ISpecularProvider";
    private const string ProviderInterfaceName = "global::" + ProviderInterfaceMetadataName;
    private const string GeneratedProviderInstanceName = "global::SpecularProvider.Instance";

    /// <summary>
    ///     Whether <paramref name="assembly" /> is the runtime package that defines <c>Specular.ISpecularProvider</c>. The assembly
    ///     the generated code is added to gets a generated <c>SpecularProvider</c> that the seeding can name when it references this
    ///     package and the <c>SpecularEmitProvider</c> MSBuild property, which Specular reads as a boolean that defaults to
    ///     <see langword="true" />, isn't false. A library that sets it to <see langword="false" /> generates no provider and keeps
    ///     receiving the application's through the context.
    /// </summary>
    /// <remarks>
    ///     Checked for each reference while the references are read, so only the package's own assembly is searched for the type,
    ///     rather than every assembly of a new compilation on every edit.
    /// </remarks>
    /// <param name="assembly">An assembly the compilation references.</param>
    public static bool DefinesProviderInterface(IAssemblySymbol assembly) =>
        assembly.Name == ProviderAssemblyName
     && assembly.GetTypeByMetadataName(ProviderInterfaceMetadataName) is { TypeKind: TypeKind.Interface, DeclaredAccessibility: Accessibility.Public, };

    /// <summary>The assembly of the runtime package that defines <c>Specular.ISpecularProvider</c>.</summary>
    private const string ProviderAssemblyName = "Specular";

    /// <summary>
    ///     The call appended to the context builder the generated <c>Imports</c> creates, which adds the provider unless it was
    ///     already added.
    /// </summary>
    public const string SeedCall = $".AddIfMissing<{ProviderInterfaceName}>({GeneratedProviderInstanceName})";

    /// <summary>
    ///     The warning the generated <c>Imports</c> disables around its code, so it can name the provider of this assembly when another
    ///     assembly shares its internals with this one.
    /// </summary>
    /// <remarks>
    ///     A test host, say, sees the internals of the application it tests, which has a <c>SpecularProvider</c> of its own. The
    ///     compiler then picks the one in this assembly, which is the one meant, and warns (CS0436) that it did.
    /// </remarks>
    public const string NameConflictWarning = "CS0436";
}
