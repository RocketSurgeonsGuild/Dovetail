using System.Collections.Immutable;
using System.Text;
using Joinery.Dovetail.Support.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support;

internal static class ExportDovetails
{
    public static void HandleDovetailExports(SourceProductionContext context, Request request)
    {
        (var msBuildConfig, var conventions, var hasConfigurationOptions) = request;
        if (!conventions.Any() && !hasConfigurationOptions) return;

        var helperClassBody = Block();
        if (hasConfigurationOptions)
        {
            // First, so options are bound from configuration before other joints configure them in code.
            helperClassBody = helperClassBody.AddStatements(
                YieldStatement(
                    SyntaxKind.YieldReturnStatement,
                    ObjectCreationExpression(IdentifierName("DovetailJointMetadata"))
                       .WithArgumentList(
                            ArgumentList(
                                SeparatedList(
                                    new[]
                                    {
                                        Argument(ObjectCreationExpression(IdentifierName(ConfigurationOptionsEmitter.JointClassName)).WithArgumentList(ArgumentList())),
                                        Argument(_hostTypeUndefined),
                                        Argument(
                                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailCategory"), IdentifierName("Application"))
                                        ),
                                    }
                                )
                            )
                        )
                )
            );
        }

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

            // What the joint declares wins; only a joint that declares nothing takes the project's default.
            var hostType = GetDeclaredHostType(convention) ?? ( msBuildConfig.IsTestProject ? _hostTypeUnitTestHost : _hostTypeUndefined );
            ExpressionSyntax conventionCategory = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("DovetailCategory"),
                IdentifierName("Application")
            );

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
                             UsingDirective(ParseName("Joinery.Dovetail")),
                             UsingDirective(ParseName("Joinery.Dovetail.Infrastructure")),
                         }
                     )
                 )
                .AddSharedTrivia()
                // Without the Dovetail.Export.* metadata, referencing assemblies don't import these joints; the class is still
                // emitted so this assembly's own Imports keeps picking them up.
                .WithAttributeLists(msBuildConfig.ExportAssembly ? msBuildConfig.ExportConfiguration.ToAttributes() : default);

        cu = cu.AddMembers(
            msBuildConfig.ExportConfiguration is { Namespace.Length: > 0 } data
             ? NamespaceDeclaration(ParseName(data.Namespace)).AddMembers(helperClass) : helperClass
        );

        context.AddSource(
            "Exported_Dovetails.g.cs",
            cu.NormalizeWhitespace().SyntaxTree.GetRoot().GetText(Encoding.UTF8)
        );
    }

    /// <summary>
    ///     The host type the joint declares, or <see langword="null" /> when it declares none. This mirrors what
    ///     <c>DovetailContextBuilder</c> reads from the joint's type at runtime: <c>[LiveJoint]</c> and <c>[UnitTestJoint]</c> first,
    ///     then <c>[DovetailHostType]</c>. All of them are inherited, so the attributes on base classes count, and the
    ///     nearest declaration wins.
    /// </summary>
    private static MemberAccessExpressionSyntax? GetDeclaredHostType(INamedTypeSymbol joint)
    {
        var attributes = new List<AttributeData>();
        for (var type = joint; type is not null; type = type.BaseType)
        {
            attributes.AddRange(type.GetAttributes());
        }

        foreach (var attribute in attributes)
        {
            switch (attribute.AttributeClass?.ToDisplayString())
            {
                case LiveJointAttributeName:
                    return HostTypeExpression("Live");
                case UnitTestJointAttributeName:
                    return HostTypeExpression("UnitTest");
                default:
                    break;
            }
        }

        foreach (var attribute in attributes)
        {
            if (attribute.AttributeClass?.ToDisplayString() != DovetailHostTypeAttributeName) continue;
            if (attribute.ConstructorArguments is not [{ Kind: TypedConstantKind.Enum, Type: INamedTypeSymbol enumType, Value: { } value }]) continue;

            // The generator doesn't reference Dovetail, so find the member the constructor argument names in the enum itself.
            var member = enumType.GetMembers().OfType<IFieldSymbol>().FirstOrDefault(z => z.HasConstantValue && Equals(z.ConstantValue, value));
            if (member is not null) return HostTypeExpression(member.Name);
        }

        return null;
    }

    private static MemberAccessExpressionSyntax HostTypeExpression(string name) => MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailHostType"),
        IdentifierName(name)
    );

    private const string LiveJointAttributeName = "Joinery.Dovetail.Attributes.LiveJointAttribute";
    private const string UnitTestJointAttributeName = "Joinery.Dovetail.Attributes.UnitTestJointAttribute";
    private const string DovetailHostTypeAttributeName = "Joinery.Dovetail.Attributes.DovetailHostTypeAttribute";

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
        ImmutableArray<INamedTypeSymbol> ExportedDovetails,
        bool HasConfigurationOptions);
}
