using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Dovetail.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Dovetail.Support;

internal static class ExportDovetails
{
    public static void HandleDovetailExports(SourceProductionContext context, Request request)
    {
        (var msBuildConfig, var conventions) = request;
        if (!conventions.Any()) return;

        var helperClassBody = Block();
        var dependencyGraph = ImmutableArray.CreateBuilder<(INamedTypeSymbol Joint, ImmutableArray<DovetailDependencyParser.DependencyEdge> Edges)>();

        foreach (var convention in conventions)
        {
            if (convention.Constructors.Length > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DovetailHasToManyConstructors, convention.Locations.FirstOrDefault()));
                continue;
            }

            if (convention.IsGenericType)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DovetailCannotBeGeneric, convention.Locations.FirstOrDefault()));
                continue;
            }

            var createDovetail = NewDovetailOrActivate(convention);

            var attributes = convention.GetAttributes();
            var hostType = _hostTypeUndefined;
            ExpressionSyntax conventionCategory = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("DovetailCategory"),
                IdentifierName("Application")
            );
            foreach (var attributeData in attributes)
            {
                if (msBuildConfig.IsTestProject)
                {
                    hostType = _hostTypeUnitTestHost;
                }
            }

            var (edges, issues) = DovetailDependencyParser.ParseDependencies(convention);
            foreach (var (location, descriptor) in issues)
            {
                context.ReportDiagnostic(Diagnostic.Create(descriptor, location ?? convention.Locations.FirstOrDefault()));
            }

            dependencyGraph.Add((convention, edges));

            ExpressionSyntax withDependencies = ObjectCreationExpression(IdentifierName("DovetailJointMetadata"))
               .WithArgumentList(
                    ArgumentList(
                        SeparatedList(
                            new[]
                            {
                                Argument(createDovetail), Argument(hostType), Argument(conventionCategory),
                            }
                        )
                    )
                );

            foreach (var edge in edges)
            {
                var direction = edge.Direction == DovetailDependencyParser.DependencyKind.DependsOn ? _dependencyDirectionDependsOn : _dependencyDirectionDependentOf;
                withDependencies = InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            withDependencies,
                            IdentifierName("WithDependency")
                        )
                    )
                   .WithArgumentList(
                        ArgumentList(
                            SeparatedList(
                                new[]
                                {
                                    Argument(direction),
                                    Argument(TypeOfExpression(ParseName(edge.Target.ToDisplayString()))),
                                }
                            )
                        )
                    );
            }


            helperClassBody = helperClassBody.AddStatements(
                YieldStatement(
                    SyntaxKind.YieldReturnStatement,
                    withDependencies
                )
            );
        }

        foreach (var (joint, cycle) in DovetailDependencyParser.DetectCycles(dependencyGraph.ToImmutable()))
        {
            var chain = string.Join(" -> ", cycle.Select(c => c.Name));
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.CircularJointDependency, joint.Locations.FirstOrDefault(), joint.Name, chain));
        }

        var helperClass =
            ClassDeclaration(msBuildConfig.ExportConfiguration.ClassName)
               .WithAttributeLists(
                    SingletonList(
                        CompilerGeneratedAttributes
                           .WithLeadingTrivia(GetXmlSummary("The class defined for exporting conventions from this assembly"))
                    )
                )
               .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword)))
               .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        MethodDeclaration(
                                GenericName(Identifier("IEnumerable"))
                                   .WithTypeArgumentList(
                                        TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName("IDovetailJointMetadata")))
                                    ),
                                msBuildConfig.ExportConfiguration.MethodName
                            )
                           .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                           .WithParameterList(ParameterList())
                           .WithBody(helperClassBody)
                           .WithLeadingTrivia(GetXmlSummary("The conventions exports from this assembly"))
                    )
                );

        var cu = CompilationUnit()
                .WithUsings(
                     List(
                         new[]
                         {
                             UsingDirective(ParseName("System")),
                             UsingDirective(ParseName("System.Collections.Generic")),
                             UsingDirective(ParseName("Microsoft.Extensions.DependencyInjection")),
                             UsingDirective(ParseName("Dovetail")),
                             UsingDirective(ParseName("Dovetail.Infrastructure")),
                         }
                     )
                 )
                .AddSharedTrivia()
                .WithAttributeLists(msBuildConfig.ExportConfiguration.ToAttributes());

        cu = cu.AddMembers(
            msBuildConfig.ExportConfiguration is { Namespace.Length: > 0 } data
             ? NamespaceDeclaration(ParseName(data.Namespace)).AddMembers(helperClass) : helperClass
        );

        context.AddSource(
            "Exported_Dovetails.g.cs",
            cu.NormalizeWhitespace().SyntaxTree.GetRoot().GetText(Encoding.UTF8)
        );
    }

    private static ExpressionSyntax NewDovetailOrActivate(INamedTypeSymbol convention)
    {
        if (convention.Constructors.Length is 0) return ObjectCreationExpression(ParseName(convention.ToDisplayString()));

        if (convention.Constructors.Count(z => z.DeclaredAccessibility is Accessibility.Internal or Accessibility.Public) == 1)
        {
            var constructor = convention.Constructors.First(z => z.DeclaredAccessibility is Accessibility.Internal or Accessibility.Public);
            var arguments = ArgumentList();
            foreach (var parameter in constructor.Parameters)
            {
                arguments = arguments.AddArguments(
                    Argument(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("builder"), IdentifierName("Properties")),
                                GenericName(Identifier("GetService"))
                                   .WithTypeArgumentList(
                                        TypeArgumentList(
                                            SingletonSeparatedList<TypeSyntax>(
                                                ParseName(parameter.Type.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString())
                                            )
                                        )
                                    )
                            )
                        )
                    )
                );
            }

            return ObjectCreationExpression(ParseName(convention.ToDisplayString()), arguments, null);
        }

        return InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("ActivatorUtilities"),
                    GenericName(Identifier("CreateInstance"))
                       .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(ParseName(convention.ToDisplayString()))))
                )
            )
           .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(IdentifierName("builder")))));
    }

    private static readonly ExpressionSyntax _hostTypeUndefined = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailHostType"),
        IdentifierName("Undefined")
    );
    private static readonly ExpressionSyntax _hostTypeUnitTestHost = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailHostType"),
        IdentifierName("UnitTest")
    );

    private static readonly ExpressionSyntax _dependencyDirectionDependsOn = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DependencyDirection"),
        IdentifierName("DependsOn")
    );

    private static readonly ExpressionSyntax _dependencyDirectionDependentOf = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DependencyDirection"),
        IdentifierName("DependentOf")
    );

    public record Request(
        MsBuildConfig BuildConfig,
        ImmutableArray<INamedTypeSymbol> ExportedDovetails);
}
