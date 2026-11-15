using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     The helpers <see cref="ConfigurationGenerator" /> uses, copied from the joint generator's <c>Helpers</c> and
///     <c>MsBuildExtensions</c> so the configuration code depends on nothing outside this folder.
/// </summary>
internal static class ConfigurationHelpers
{
    public static T? GetBuildProperty<T>(this AnalyzerConfigOptions options, string propertyName, Func<string, T?> transform) =>
        options.TryGetValue($"build_property.{propertyName}", out var value) ? transform(value) : default;

    public static CompilationUnitSyntax AddSharedTrivia(this CompilationUnitSyntax source) => source
                                                                                             .WithLeadingTrivia(
                                                                                                  Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true)),
                                                                                                  Trivia(
                                                                                                      PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true)
                                                                                                         .WithErrorCodes(SeparatedList(List(DisabledWarnings.Value)))
                                                                                                  )
                                                                                              )
                                                                                             .WithTrailingTrivia(
                                                                                                  Trivia(
                                                                                                      PragmaWarningDirectiveTrivia(Token(SyntaxKind.RestoreKeyword), true)
                                                                                                         .WithErrorCodes(SeparatedList(List(DisabledWarnings.Value)))
                                                                                                  ),
                                                                                                  Trivia(NullableDirectiveTrivia(Token(SyntaxKind.RestoreKeyword), true)),
                                                                                                  CarriageReturnLineFeed
                                                                                              );

    public static bool IsOpenGenericType(this INamedTypeSymbol type) =>
        type.IsGenericType && ( type.IsUnboundGenericType || type.TypeArguments.All(z => z.TypeKind == TypeKind.TypeParameter) );

    internal static readonly AttributeListSyntax CompilerGeneratedAttributes =
        AttributeList(
            SeparatedList(
                [
                    Attribute(ParseName("System.CodeDom.Compiler.GeneratedCode"))
                       .WithArgumentList(
                            AttributeArgumentList(
                                SeparatedList(
                                    [
                                        AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(AssemblyInfo.Project.AssemblyName))),
                                        AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(AssemblyInfo.Attributes.FileVersion))),
                                    ]
                                )
                            )
                        ),
                    Attribute(ParseName("System.Runtime.CompilerServices.CompilerGenerated")),
                    Attribute(ParseName("System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage")),
                ]
            )
        );

    internal static AttributeListSyntax AddAssemblyAttribute(string key, string? value) => AttributeList(
            SingletonSeparatedList(
                Attribute(QualifiedName(QualifiedName(IdentifierName("System"), IdentifierName("Reflection")), IdentifierName("AssemblyMetadata")))
                   .WithArgumentList(
                        AttributeArgumentList(
                            SeparatedList(
                                [
                                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(key))),
                                    AttributeArgument(
                                        value is null
                                            ? LiteralExpression(SyntaxKind.NullLiteralExpression)
                                            : LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value))
                                    ),
                                ]
                            )
                        )
                    )
            )
        )
       .WithTarget(AttributeTargetSpecifier(Token(SyntaxKind.AssemblyKeyword)));

    private static readonly string[] _disabledWarnings =
    [
        "CA1002",
        "CA1034",
        "CA1822",
        "CS0105",
        "CS1573",
        "CS8602",
        "CS8603",
        "CS8618",
        "CS8669",
    ];

    private static readonly Lazy<ImmutableArray<ExpressionSyntax>> DisabledWarnings = new(
        () => [.. _disabledWarnings.Select(z => (ExpressionSyntax)IdentifierName(z))]
    );
}
