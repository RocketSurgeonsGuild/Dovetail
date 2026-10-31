using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support;

internal static class ImportDovetails
{
    public static void HandleDovetailImports(
        SourceProductionContext context,
        Request request
    )
    {
        if (!request.BuildConfig.ImportAssembly) return;

        var exportConfig = request.BuildConfig.ExportConfiguration;
        var referenceMethods = request
                              .Compilation.GetDovetailReferences()
                              .Select(refer => refer.ToString())
                              .Concat(request.ExportedDovetails.Length > 0 || request.HasConfigurationOptions ? [$"{( exportConfig.Namespace is { Length: > 0 } ? $"{exportConfig.Namespace}." : "" )}{exportConfig.ClassName}.{exportConfig.MethodName}"] : [])
                              .ToImmutableList();

        var functionBody = referenceMethods.Count == 0 ? Block(YieldStatement(SyntaxKind.YieldBreakStatement)) : addEnumerateExportStatements(referenceMethods);

        // Only pin the host type when the build says what it is: an explicit DovetailHostType first, then test projects.
        // Pinning it unconditionally (it used to always be UnitTest) wins over the hosting helpers'
        // AddIfMissing(DovetailHostType.Live), so real applications ran UnitTest-only joints and skipped Live-only ones.
        var hostTypeName = PinnableHostTypes.FirstOrDefault(z => string.Equals(z, request.BuildConfig.HostType.Trim(), StringComparison.OrdinalIgnoreCase))
         ?? ( request.BuildConfig.IsTestProject ? "UnitTest" : null );
        ArgumentSyntax[] hostTypeArguments = hostTypeName is null
            ? []
            : [Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailHostType"), IdentifierName(hostTypeName)))];

        var importsClass =
            ClassDeclaration(request.BuildConfig.ImportConfiguration.ClassName)
               .WithAttributeLists(
                    SingletonList(
                        CompilerGeneratedAttributes
                           .WithLeadingTrivia(GetXmlSummary("The class defined for importing Dovetail parts into this assembly"))
                    )
                )
               .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
               .AddMembers(
                    FieldDeclaration(
                            VariableDeclaration(IdentifierName("DovetailContextBuilderFactory"))
                               .WithVariables(
                                    SingletonSeparatedList(
                                        VariableDeclarator(Identifier(request.BuildConfig.ImportConfiguration.MethodName))
                                           .WithInitializer(EqualsValueClause(IdentifierName(ContextBuilderFactoryName)))
                                    )
                                )
                        )
                       .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword))),
                    // The parameterless factory delegate assigned to ImportHelpers.Rivets. It seeds a builder to
                    // resolve any convention dependencies, then materializes the final builder from the imported parts.
                    MethodDeclaration(IdentifierName("DovetailContextBuilder"), Identifier(ContextBuilderFactoryName))
                       .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)))
                       .WithParameterList(ParameterList([
                        // new dictionary<object, object>
                        Parameter(Identifier("properties"))
                            .WithType(                            NullableType(GenericName("IDictionary").WithTypeArgumentList(TypeArgumentList(SeparatedList<TypeSyntax>([PredefinedType(Token(SyntaxKind.ObjectKeyword)), PredefinedType(Token(SyntaxKind.ObjectKeyword))]))))                        )
                            .WithDefault(EqualsValueClause(LiteralExpression(SyntaxKind.NullLiteralExpression))),
                        Parameter(Identifier("categories"))
                            .WithType(                            NullableType(GenericName("IEnumerable").WithTypeArgumentList(TypeArgumentList(SeparatedList<TypeSyntax>([IdentifierName("DovetailCategory")]))))                        )
                            .WithDefault(EqualsValueClause(LiteralExpression(SyntaxKind.NullLiteralExpression))),
                       ]))
                       .WithExpressionBody(
            ArrowExpressionClause(
                                    InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailContextBuilder"), IdentifierName("Create")))
                                       .WithArgumentList(
                                            ArgumentList(
                                                SeparatedList(
                                                    [
                                                        Argument(InvocationExpression(IdentifierName("LoadDovetailJointsMethod")).WithArgumentList(ArgumentList())),
                                                        Argument(BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        IdentifierName("properties"),
                        ObjectCreationExpression(
                            GenericName(Identifier("Dictionary"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>([PredefinedType(Token(SyntaxKind.ObjectKeyword)), PredefinedType(Token(SyntaxKind.ObjectKeyword))])
                                    )
                                )
                        ).WithArgumentList(ArgumentList())
                    )),
                                                        ..hostTypeArguments,
                                                        Argument(BinaryExpression(                        SyntaxKind.CoalesceExpression,                        IdentifierName("categories"),                        CollectionExpression()                    ))
                                                    ]
                                                )
                                            )
                                        )
                                    )
                            )
                       .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                       .WithLeadingTrivia(GetXmlSummary("Creates the context builder populated with the Dovetail parts imported into this assembly")),
                    MethodDeclaration(
                            GenericName(Identifier("IEnumerable"))
                               .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(IdentifierName("IDovetailJointMetadata"))
                                    )
                                ),
                            Identifier("LoadDovetailJointsMethod")
                        )
                       .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)))
                       .WithParameterList(ParameterList())
                       .WithBody(functionBody)
                       .WithLeadingTrivia(GetXmlSummary("The Dovetail parts imported into this assembly"))
                )
               .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        // An application that references Specular gets its provider in the context without setting it by hand.
        var seedsSpecularProvider = SpecularProviderSeed.IsAvailable(request.Compilation, request.BuildConfig.SpecularEmitsProvider);
        if (seedsSpecularProvider)
        {
            importsClass = SpecularProviderSeed.Seed(importsClass);
        }

        if (request.BuildConfig.IsTestProject || request.BuildConfig.AssignExternal)
        {
            importsClass = importsClass.AddMembers(
                MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("Init"))
                   .WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SeparatedList(
                                    [
                                        Attribute(ParseName("System.Runtime.CompilerServices.ModuleInitializer")),
                                        Attribute(ParseName("System.ComponentModel.EditorBrowsable"))
                                           .WithArgumentList(
                                                AttributeArgumentList(
                                                    SingletonSeparatedList(
                                                        AttributeArgument(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                ParseName("System.ComponentModel.EditorBrowsableState"),
                                                                IdentifierName("Never")
                                                            )
                                                        )
                                                    )
                                                )
                                            ),
                                    ]
                                )
                            )
                        )
                    )
                   .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword)))
                   .WithBody(
                        Block(
                            List<StatementSyntax>(
                                [
                                    ExpressionStatement(
                                        AssignmentExpression(
                                            SyntaxKind.SimpleAssignmentExpression,
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("ImportHelpers"),
                                                IdentifierName("Rivets")
                                            ),
                                            IdentifierName(request.BuildConfig.ImportConfiguration.MethodName)
                                        )
                                    ),
                                ]
                            )
                        )
                    )
            );
        }

        var cu = CompilationUnit()
                .WithAttributeLists(request.BuildConfig.ImportConfiguration.ToAttributes())
                .AddSharedTrivia()
                .WithUsings(
                     List(
                         [
                             UsingDirective(ParseName("System")),
                             UsingDirective(ParseName("System.Collections.Generic")),
                             UsingDirective(ParseName("System.Runtime.Loader")),
                             UsingDirective(ParseName("Microsoft.Extensions.DependencyInjection")),
                             UsingDirective(ParseName("Joinery.Dovetail")),
                             UsingDirective(ParseName("Joinery.Dovetail.Infrastructure")),
                         ]
                     )
                 );
        var members = new List<MemberDeclarationSyntax>
        {
            importsClass,
        };

        cu = cu
           .AddMembers(
                request.BuildConfig.ImportConfiguration is { Namespace: { Length: > 0 } relativeNamespace }
                    ? [NamespaceDeclaration(ParseName(relativeNamespace)).AddMembers(members.ToArray())]
                    : [.. members]
            );

        if (seedsSpecularProvider) cu = SpecularProviderSeed.AllowNameConflict(cu);

        context.AddSource(
            "Imported_Assembly_Dovetails.g.cs",
            cu.NormalizeWhitespace().SyntaxTree.GetRoot().GetText(Encoding.UTF8)
        );

        static BlockSyntax addEnumerateExportStatements(IReadOnlyCollection<string> references)
        {
            var block = Block();
            foreach (var reference in references)
            {
                block = block.AddStatements(
                    ForEachStatement(
                            IdentifierName("var"),
                            Identifier("part"),
                            InvocationExpression(ParseExpression(reference))
                               .WithArgumentList(ArgumentList()),
                            YieldStatement(SyntaxKind.YieldReturnStatement, IdentifierName("part"))
                        )
                       .NormalizeWhitespace()
                );
            }

            return block;
        }
    }

    /// <summary>The name of the generated method that creates the context builder.</summary>
    internal const string ContextBuilderFactoryName = "CreateDovetailContextBuilder";

    /// <summary>The <c>DovetailHostType</c> values a build may pin; anything else (e.g. <c>Undefined</c>) leaves it to the host.</summary>
    private static readonly string[] PinnableHostTypes = ["Live", "UnitTest"];

    public record Request
    (
        Compilation Compilation,
        MsBuildConfig BuildConfig,
        ImmutableArray<INamedTypeSymbol> ExportedDovetails,
        bool HasConfigurationOptions
    );

}
