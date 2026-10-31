using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
    ///     Whether the assembly gets a generated <c>SpecularProvider</c> that the seeding can name.
    /// </summary>
    /// <param name="compilation">The compilation the generated code is added to.</param>
    /// <param name="emitsProvider">
    ///     The <c>SpecularEmitProvider</c> MSBuild property, which Specular reads as a boolean that defaults to <see langword="true" />.
    ///     A library that sets it to <see langword="false" /> generates no provider and keeps receiving the application's through the context.
    /// </param>
    public static bool IsAvailable(Compilation compilation, bool emitsProvider) =>
        emitsProvider
     && compilation.GetTypeByMetadataName(ProviderInterfaceMetadataName) is { TypeKind: TypeKind.Interface, DeclaredAccessibility: Accessibility.Public, };

    /// <summary>
    ///     Adds the provider to the builder the generated <c>Imports</c> creates, unless it was already added.
    /// </summary>
    /// <param name="importsClass">The generated <c>Imports</c> class, with its context builder factory.</param>
    public static ClassDeclarationSyntax Seed(ClassDeclarationSyntax importsClass)
    {
        var factory = importsClass
                     .Members.OfType<MethodDeclarationSyntax>()
                     .Single(z => z.Identifier.ValueText == ImportDovetails.ContextBuilderFactoryName);
        if (factory.ExpressionBody is not { } body) throw new InvalidOperationException("The context builder factory is expected to be expression-bodied.");

        var seeded = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    body.Expression,
                    GenericName(Identifier("AddIfMissing"))
                       .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(ParseTypeName(ProviderInterfaceName))))
                )
            )
           .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(ParseExpression(GeneratedProviderInstanceName)))));
        return importsClass.ReplaceNode(factory, factory.WithExpressionBody(body.WithExpression(seeded)));
    }

    /// <summary>
    ///     Allows the generated code to name the provider of this assembly when another assembly shares its internals with this one.
    /// </summary>
    /// <remarks>
    ///     A test host, say, sees the internals of the application it tests, which has a <c>SpecularProvider</c> of its own. The
    ///     compiler then picks the one in this assembly, which is the one meant, and warns (CS0436) that it did.
    /// </remarks>
    /// <param name="compilationUnit">The generated <c>Imports</c> file.</param>
    public static CompilationUnitSyntax AllowNameConflict(CompilationUnitSyntax compilationUnit) =>
        compilationUnit
           .WithLeadingTrivia(compilationUnit.GetLeadingTrivia().Add(Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true).WithErrorCodes(NameConflictWarning))))
           .WithTrailingTrivia(compilationUnit.GetTrailingTrivia().Insert(0, Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.RestoreKeyword), true).WithErrorCodes(NameConflictWarning))));

    private static SeparatedSyntaxList<ExpressionSyntax> NameConflictWarning => SingletonSeparatedList<ExpressionSyntax>(IdentifierName("CS0436"));
}
