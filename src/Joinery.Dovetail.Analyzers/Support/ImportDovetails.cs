using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;
using static Joinery.Dovetail.SyntaxLayout;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support;

internal static class ImportDovetails
{
    /// <summary>
    ///     What the imports need from the compilation: the export methods of the referenced assemblies, and whether Specular's
    ///     provider interface is there to seed. Read on every compilation, but cheap, and equal from one edit to the next unless
    ///     the references change, so the imports file isn't generated again.
    /// </summary>
    public sealed record ReferencedExports(EquatableArray<string> ExportMethods, bool HasSpecularProviderInterface)
    {
        public static ReferencedExports From(Compilation compilation) => new(
            compilation.GetDovetailReferences().Select(reference => reference.ToString()).ToEquatableArray(),
            SpecularProviderSeed.HasProviderInterface(compilation)
        );
    }

    public static void HandleDovetailImports(SourceProductionContext context, Request request)
    {
        var buildConfig = request.BuildConfig;
        if (!buildConfig.ImportAssembly) return;

        var exportConfig = buildConfig.ExportConfiguration;
        var referenceMethods = request
                              .References.ExportMethods
                              .Concat(request.HasExports || request.HasConfigurationOptions ? [$"{( exportConfig.Namespace is { Length: > 0 } ? $"{exportConfig.Namespace}." : "" )}{exportConfig.ClassName}.{exportConfig.MethodName}"] : [])
                              .ToList();

        // The class sits in the namespace when there is one; its members one level in, and their statements one more.
        var classDepth = buildConfig.ImportConfiguration is { Namespace.Length: > 0 } ? 1 : 0;
        var memberDepth = classDepth + 1;
        var statementDepth = classDepth + 2;

        var functionBody = BlockAt(
            memberDepth,
            referenceMethods.Count == 0
                ? [YieldStatement(SyntaxKind.YieldBreakStatement).WithYieldKeyword(SpaceAfter(SyntaxKind.YieldKeyword)).OnLine(statementDepth)]
                : referenceMethods.Select(StatementSyntax (reference) => EnumerateExport(reference, statementDepth))
        );

        // Only pin the host type when the build says what it is: an explicit DovetailHostType first, then test projects.
        // Pinning it unconditionally (it used to always be UnitTest) wins over the hosting helpers'
        // AddIfMissing(DovetailHostType.Live), so real applications ran UnitTest-only joints and skipped Live-only ones.
        var hostTypeName = PinnableHostTypes.FirstOrDefault(z => string.Equals(z, buildConfig.HostType.Trim(), StringComparison.OrdinalIgnoreCase))
         ?? ( buildConfig.IsTestProject ? "UnitTest" : null );
        ArgumentSyntax[] hostTypeArguments = hostTypeName is null
            ? []
            : [Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailHostType"), IdentifierName(hostTypeName)))];

        var members = new List<MemberDeclarationSyntax>
        {
            FieldDeclaration(
                    VariableDeclaration(IdentifierName("DovetailContextBuilderFactory").WithTrailingTrivia(Space))
                       .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(Identifier(buildConfig.ImportConfiguration.MethodName))
                                   .WithInitializer(EqualsValueClause(Spaced(SyntaxKind.EqualsToken), IdentifierName(ContextBuilderFactoryName)))
                            )
                        )
                )
               .WithModifiers(Modifiers(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword))
               .Member(memberDepth),
            // The parameterless factory delegate assigned to ImportHelpers.Rivets. It seeds a builder to
            // resolve any convention dependencies, then materializes the final builder from the imported parts.
            MethodDeclaration(IdentifierName("DovetailContextBuilder").WithTrailingTrivia(Space), Identifier(ContextBuilderFactoryName))
               .WithModifiers(Modifiers(SyntaxKind.PrivateKeyword, SyntaxKind.StaticKeyword))
               .WithParameterList(
                    ParameterList(
                        CommaSeparated(
                            OptionalParameter("properties", GenericType("IDictionary", ObjectType, ObjectType)),
                            OptionalParameter("categories", GenericType("IEnumerable", IdentifierName("DovetailCategory")))
                        )
                    )
                )
               .WithExpressionBody(
                    ArrowExpressionClause(
                        Spaced(SyntaxKind.EqualsGreaterThanToken),
                        InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailContextBuilder"), IdentifierName("Create")))
                           .WithArgumentList(
                                ArgumentList(
                                    CommaSeparated(
                                        [
                                            Argument(InvocationExpression(IdentifierName("LoadDovetailJointsMethod"))),
                                            Argument(
                                                Coalesce(
                                                    IdentifierName("properties"),
                                                    ObjectCreationExpression(SpaceAfter(SyntaxKind.NewKeyword), GenericType("Dictionary", ObjectType, ObjectType), ArgumentList(), null)
                                                )
                                            ),
                                            .. hostTypeArguments,
                                            Argument(Coalesce(IdentifierName("categories"), CollectionExpression())),
                                        ]
                                    )
                                )
                            )
                    )
                )
               .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
               .Member(memberDepth, "Creates the context builder populated with the Dovetail parts imported into this assembly"),
            MethodDeclaration(GenericType("IEnumerable", IdentifierName("IDovetailJointMetadata")).WithTrailingTrivia(Space), Identifier("LoadDovetailJointsMethod"))
               .WithModifiers(Modifiers(SyntaxKind.PrivateKeyword, SyntaxKind.StaticKeyword))
               .WithBody(functionBody)
               .Member(memberDepth, "The Dovetail parts imported into this assembly"),
        };

        if (buildConfig.IsTestProject || buildConfig.AssignExternal)
        {
            members.Add(
                MethodDeclaration(PredefinedType(SpaceAfter(SyntaxKind.VoidKeyword)), Identifier("Init"))
                   .WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                CommaSeparated(
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
                                        )
                                )
                            )
                        )
                    )
                   .WithModifiers(Modifiers(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword))
                   .WithBody(
                        BlockAt(
                            memberDepth,
                            ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("ImportHelpers"), IdentifierName("Rivets")),
                                        Spaced(SyntaxKind.EqualsToken),
                                        IdentifierName(buildConfig.ImportConfiguration.MethodName)
                                    )
                                )
                               .OnLine(statementDepth)
                        )
                    )
                   .Member(memberDepth)
            );
        }

        var importsClass =
            ClassDeclaration(buildConfig.ImportConfiguration.ClassName)
               .WithAttributeLists(SingletonList(CompilerGeneratedAttributes))
               .WithModifiers(Modifiers(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword))
               .WithKeyword(SpaceAfter(SyntaxKind.ClassKeyword))
               .WithOpenBraceToken(OpenBrace(classDepth))
               .WithMembers(Members(members))
               .WithCloseBraceToken(CloseBrace(classDepth))
               .Member(classDepth, "The class defined for importing Dovetail parts into this assembly");

        // An application that references Specular gets its provider in the context without setting it by hand.
        var seedsSpecularProvider = buildConfig.SpecularEmitsProvider && request.References.HasSpecularProviderInterface;
        if (seedsSpecularProvider)
        {
            importsClass = SpecularProviderSeed.Seed(importsClass);
        }

        var cu = CompilationUnit()
                .WithUsings(
                     List(
                         [
                             Using("System").WithLeadingTrivia(SharedHeaderTrivia),
                             Using("System.Collections.Generic"),
                             Using("System.Runtime.Loader"),
                             Using("Microsoft.Extensions.DependencyInjection"),
                             Using("Joinery.Dovetail"),
                             Using("Joinery.Dovetail.Infrastructure"),
                         ]
                     )
                 )
                .WithAttributeLists(buildConfig.ImportConfiguration.ToAttributes())
                .WithMembers(
                     SingletonList<MemberDeclarationSyntax>(
                         buildConfig.ImportConfiguration is { Namespace: { Length: > 0 } relativeNamespace }
                             ? InNamespace(relativeNamespace, importsClass)
                             : importsClass
                     )
                 )
                .WithEndOfFileToken(EndOfFile(SharedFooterTrivia))
                .SeparateUsings();

        if (seedsSpecularProvider) cu = SpecularProviderSeed.AllowNameConflict(cu);

        context.AddSource("Imported_Assembly_Dovetails.g.cs", cu.GetText(Encoding.UTF8));

        static ForEachStatementSyntax EnumerateExport(string reference, int depth) =>
            ForEachStatement(
                    IdentifierName("var").WithTrailingTrivia(Space),
                    Identifier("part").WithTrailingTrivia(Space),
                    InvocationExpression(ParseExpression(reference)),
                    YieldStatement(SyntaxKind.YieldReturnStatement, IdentifierName("part"))
                       .WithYieldKeyword(SpaceAfter(SyntaxKind.YieldKeyword))
                       .WithReturnOrBreakKeyword(SpaceAfter(SyntaxKind.ReturnKeyword))
                       .OnLine(depth + 1)
                )
               .WithForEachKeyword(SpaceAfter(SyntaxKind.ForEachKeyword))
               .WithInKeyword(SpaceAfter(SyntaxKind.InKeyword))
               .WithCloseParenToken(Token(default, SyntaxKind.CloseParenToken, TriviaList(NewLine)))
               .OnLine(depth);

        static ParameterSyntax OptionalParameter(string name, TypeSyntax type) =>
            Parameter(Identifier(name))
               .WithType(NullableType(type).WithTrailingTrivia(Space))
               .WithDefault(EqualsValueClause(Spaced(SyntaxKind.EqualsToken), LiteralExpression(SyntaxKind.NullLiteralExpression)));

        static GenericNameSyntax GenericType(string name, params IEnumerable<TypeSyntax> typeArguments) =>
            GenericName(Identifier(name)).WithTypeArgumentList(TypeArgumentList(CommaSeparated(typeArguments)));

        static BinaryExpressionSyntax Coalesce(ExpressionSyntax left, ExpressionSyntax right) =>
            BinaryExpression(SyntaxKind.CoalesceExpression, left, Spaced(SyntaxKind.QuestionQuestionToken), right);
    }

    /// <summary>The name of the generated method that creates the context builder.</summary>
    internal const string ContextBuilderFactoryName = "CreateDovetailContextBuilder";

    /// <summary>The <c>DovetailHostType</c> values a build may pin; anything else (e.g. <c>Undefined</c>) leaves it to the host.</summary>
    private static readonly string[] PinnableHostTypes = ["Live", "UnitTest"];

    private static readonly PredefinedTypeSyntax ObjectType = PredefinedType(Token(SyntaxKind.ObjectKeyword));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="BuildConfig">The build properties.</param>
    /// <param name="References">What the imports need from the compilation.</param>
    /// <param name="HasExports">Whether this assembly exports any joint, so its own exports are imported too.</param>
    /// <param name="HasConfigurationOptions">Whether this assembly declares Optionary options, which its exports register.</param>
    public record Request(MsBuildConfig BuildConfig, ReferencedExports References, bool HasExports, bool HasConfigurationOptions);
}
