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
                                        Argument(_categoryApplication),
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
            // Only the constructors the generated code can call count: a private one is there for something else to use.
            var callableConstructors = convention.InstanceConstructors.Where(CanBeCalled).ToArray();
            if (callableConstructors.Length > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DovetailHasToManyConstructors, convention.Locations.FirstOrDefault()));
                continue;
            }

            // Left out of the exports like any other joint that is reported, so nothing else fails to compile because of it.
            if (callableConstructors.Length == 0 && convention.InstanceConstructors.Length > 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DovetailHasNoCallableConstructor, convention.Locations.FirstOrDefault(), convention.ToDisplayString()));
                continue;
            }

            if (convention.IsGenericType)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DovetailCannotBeGeneric, convention.Locations.FirstOrDefault()));
                continue;
            }

            var createDovetail = NewDovetail(convention, callableConstructors.SingleOrDefault());

            // What the joint declares wins; only a joint that declares nothing takes the project's default.
            var hostType = GetDeclaredHostType(convention) ?? ( msBuildConfig.IsTestProject ? _hostTypeUnitTestHost : _hostTypeUndefined );
            var conventionCategory = GetDeclaredCategory(convention);

            var (edges, issues) = DovetailDependencyParser.ParseDependencies(convention);
            foreach (var (location, descriptor) in issues)
            {
                context.ReportDiagnostic(Diagnostic.Create(descriptor, location ?? convention.Locations.FirstOrDefault()));
            }

            dependencyGraph.Add((convention, edges));

            var metadataArguments = ArgumentList(SeparatedList(new[] { Argument(createDovetail), Argument(hostType), Argument(conventionCategory), }));
            // A joint that needs its constructor parameters resolved is a factory, which has its own entry point rather than a
            // constructor overload; see DovetailJointMetadata.
            ExpressionSyntax withDependencies = createDovetail is LambdaExpressionSyntax
                ? InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailJointMetadata"), IdentifierName("FromFactory"))
                    )
                   .WithArgumentList(metadataArguments)
                : ObjectCreationExpression(IdentifierName("DovetailJointMetadata")).WithArgumentList(metadataArguments);

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

        // Every joint may have been left out, and an iterator without a yield is not a valid method.
        if (helperClassBody.Statements.Count == 0) helperClassBody = helperClassBody.AddStatements(YieldStatement(SyntaxKind.YieldBreakStatement));

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

    /// <summary>
    ///     The category the joint declares with <c>[DovetailCategory]</c>, or <c>Application</c> when it declares none. This mirrors
    ///     what <c>DovetailContextBuilder</c> reads from the joint's type at runtime: the attribute is inherited from base classes (not
    ///     from interfaces, which attribute lookup on a type never reads) and the nearest declaration wins.
    /// </summary>
    private static ExpressionSyntax GetDeclaredCategory(INamedTypeSymbol joint)
    {
        for (var type = joint; type is not null; type = type.BaseType)
        {
            foreach (var attribute in type.GetAttributes())
            {
                if (attribute.AttributeClass?.ToDisplayString() != DovetailCategoryAttributeName) continue;

                // The constructor takes a string, which the compiler has already folded to its value, whether it was written as
                // a literal, a constant such as DovetailCategory.Core or an expression of constants. An attribute that has no usable
                // value (null, or an argument that doesn't bind) declares nothing.
                if (attribute.ConstructorArguments is [{ Kind: TypedConstantKind.Primitive, Value: string category }]) return CategoryExpression(category);
            }
        }

        return _categoryApplication;
    }

    /// <summary>
    ///     The category as the generated code spells it. <c>DovetailCategory</c> converts implicitly from the string the attribute
    ///     takes, so a category without a constant of its own is written as its string literal. The well-known ones use their
    ///     constants, which is how a joint without the attribute has always been written.
    /// </summary>
    private static ExpressionSyntax CategoryExpression(string category) => category switch
    {
        ApplicationCategory => _categoryApplication,
        CoreCategory => _categoryCore,
        _ => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(category)),
    };

    private const string ApplicationCategory = "Application";
    private const string CoreCategory = "Core";
    private const string DovetailCategoryAttributeName = "Joinery.Dovetail.Attributes.DovetailCategoryAttribute";
    private const string LiveJointAttributeName = "Joinery.Dovetail.Attributes.LiveJointAttribute";
    private const string UnitTestJointAttributeName = "Joinery.Dovetail.Attributes.UnitTestJointAttribute";
    private const string DovetailHostTypeAttributeName = "Joinery.Dovetail.Attributes.DovetailHostTypeAttribute";

    /// <summary>
    ///     The expression that gives <c>DovetailJointMetadata</c> its joint. A joint that needs nothing is created right there. A
    ///     joint with constructor parameters is handed over as a factory instead: the exported method has no parameters and no
    ///     context to read from, so the parameters are resolved when the context builder that ends up holding the metadata creates
    ///     the joint, from its properties (see <c>DovetailJointMetadata</c>).
    /// </summary>
    private static ExpressionSyntax NewDovetail(INamedTypeSymbol convention, IMethodSymbol? constructor)
    {
        var typeName = convention.ToDisplayString();
        // A joint with no constructor to find (a struct that declares none) or none that takes anything is created right here.
        if (constructor is not { Parameters.Length: > 0 }) return ObjectCreationExpression(ParseName(typeName), ArgumentList(), null);

        var arguments = constructor.Parameters.Select(parameter => ResolveParameter(typeName, parameter));
        return ParseExpression($"static {JointFactoryParameter} => new {typeName}({string.Join(", ", arguments)})");
    }

    /// <summary>
    ///     Whether the generated code, which lives in the exporting assembly, can call the constructor. That is every instance
    ///     constructor that is public or internal, and <c>protected internal</c> is internal to this assembly too.
    /// </summary>
    private static bool CanBeCalled(IMethodSymbol constructor) =>
        constructor.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal;

    /// <summary>The name of the <c>IServiceProvider</c> the joint factory receives, which is the context builder's properties.</summary>
    private const string JointFactoryParameter = "services";

    /// <summary>
    ///     Takes a constructor argument from the properties by the parameter's type, the way
    ///     <c>ActivatorUtilities.CreateInstance</c> does for a joint added with <c>AppendJoint&lt;T&gt;()</c>: the parameter's default
    ///     value when there is no such property and the parameter has one, otherwise an <see cref="InvalidOperationException" />.
    /// </summary>
    private static string ResolveParameter(string jointName, IParameterSymbol parameter)
    {
        // Looked up by the type as declared. For a nullable value type that is Nullable<T>, which is what Set<int?>() stores it under.
        var parameterType = parameter.Type.WithNullableAnnotation(NullableAnnotation.None);
        var isNullableValueType = parameterType is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };
        // Spelled out because the formatter that lays out the generated file puts a space between the ? and a ) that follows it.
        var type = isNullableValueType && parameterType is INamedTypeSymbol { TypeArguments: [var underlying] }
            ? $"System.Nullable<{underlying.ToDisplayString()}>"
            : parameterType.ToDisplayString();

        // GetService<T>() returns null for a missing value, which is what the fallbacks below key on. For a value type that can't hold
        // null it would return default(T), which can't be told apart from a value that was set to it, and `?? throw` doesn't compile
        // on it. Those are read as a Nullable<T> instead: the same cast GetService<T>() makes, with a nullable result.
        var lookup = parameterType.IsValueType && !isNullableValueType
            ? $"(System.Nullable<{type}>){JointFactoryParameter}.GetService(typeof({type}))"
            : $"Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<{type}>({JointFactoryParameter})";

        if (!parameter.HasExplicitDefaultValue)
        {
            var message = $"Unable to resolve a value of type '{parameterType.ToDisplayString()}' for parameter '{parameter.Name}' of joint '{jointName}' from the Dovetail context. "
              + "Set it on the context builder before the joint is created, or give the parameter a default value.";
            return $"{lookup} ?? throw new System.InvalidOperationException({SymbolDisplay.FormatLiteral(message, quote: true)})";
        }

        // A missing value is already null, which is the default of a reference type or a nullable value type.
        var defaultValue = DefaultValue(parameter, parameterType, type);
        return defaultValue == "null" ? lookup : $"{lookup} ?? {defaultValue}";
    }

    /// <summary>
    ///     The default value the parameter declares, as an expression typed by the parameter and not by the literal, so the numeric
    ///     literals are cast to it.
    /// </summary>
    private static string DefaultValue(IParameterSymbol parameter, ITypeSymbol parameterType, string type)
    {
        return parameter.ExplicitDefaultValue switch
        {
            string text => SymbolDisplay.FormatLiteral(text, quote: true),
            bool flag => flag ? "true" : "false",
            char character => $"({type}){SymbolDisplay.FormatLiteral(character, quote: true)}",
            sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal =>
                $"({type}){SymbolDisplay.FormatPrimitive(parameter.ExplicitDefaultValue, quoteStrings: false, useHexadecimalNumbers: false)}",
            // null, and anything the generator has no literal for, is the type's default: null for a reference type or a nullable
            // value type, default for any other value type.
            _ when parameterType.IsValueType && parameterType.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T => "default",
            _ => "null",
        };
    }

    private static readonly ExpressionSyntax _categoryApplication = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailCategory"),
        IdentifierName(ApplicationCategory)
    );
    private static readonly ExpressionSyntax _categoryCore = MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailCategory"),
        IdentifierName(CoreCategory)
    );

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
