using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Joinery.Dovetail.Helpers;
using static Joinery.Dovetail.SyntaxLayout;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Joinery.Dovetail.Support;

internal static class ExportDovetails
{
    /// <summary>
    ///     What the exports need to know about a joint, read from its symbol in the pipeline's transform. It holds no symbol, so an
    ///     edit elsewhere leaves it equal and the exports file isn't generated again.
    /// </summary>
    /// <param name="Name">The joint's name, which the cycle diagnostic names it by.</param>
    /// <param name="TypeName">The joint's display string, which the generated code names it by.</param>
    /// <param name="MetadataName">The joint's metadata name, which orders the exports.</param>
    /// <param name="Location">Where the joint is declared, for its diagnostics.</param>
    /// <param name="Rejection">Why the joint is left out of the exports, or <see langword="null" /> when it is exported.</param>
    /// <param name="Factory">The factory that creates the joint when its constructor takes parameters, or <see langword="null" /> to create it with <c>new</c>.</param>
    /// <param name="HostType">The host type the joint declares, or <see langword="null" /> when it declares none.</param>
    /// <param name="Category">The category the joint declares, or <see langword="null" /> for <c>Application</c>.</param>
    /// <param name="Edges">The joint's dependencies.</param>
    /// <param name="Issues">The malformed dependency attributes on the joint.</param>
    public sealed record ExportedJoint(
        string Name,
        string TypeName,
        string MetadataName,
        Location? Location,
        DiagnosticDescriptor? Rejection,
        string? Factory,
        string? HostType,
        string? Category,
        EquatableArray<DovetailDependencyParser.DependencyEdge> Edges,
        EquatableArray<DovetailDependencyParser.DependencyIssue> Issues
    );

    /// <summary>Reads what the exports need to know about <paramref name="convention" />.</summary>
    public static ExportedJoint Describe(INamedTypeSymbol convention)
    {
        var typeName = convention.ToDisplayString();
        var location = convention.Locations.FirstOrDefault();

        // Only the constructors the generated code can call count: a private one is there for something else to use.
        var (callableConstructorCount, callableConstructor) = CallableConstructor(convention);
        var rejection = callableConstructorCount > 1
            ? Diagnostics.DovetailHasToManyConstructors
            // Left out of the exports like any other joint that is reported, so nothing else fails to compile because of it.
            : callableConstructorCount == 0 && convention.InstanceConstructors.Length > 0
                ? Diagnostics.DovetailHasNoCallableConstructor
                : convention.IsGenericType
                    ? Diagnostics.DovetailCannotBeGeneric
                    : null;
        if (rejection is { }) return new(convention.Name, typeName, convention.MetadataName, location, rejection, null, null, null, default, default);

        var (edges, issues) = DovetailDependencyParser.ParseDependencies(convention);
        return new(
            convention.Name,
            typeName,
            convention.MetadataName,
            location,
            null,
            Factory(typeName, callableConstructor),
            GetDeclaredHostType(convention),
            GetDeclaredCategory(convention),
            edges.ToEquatableArray(),
            issues.ToEquatableArray()
        );
    }

    public static void HandleDovetailExports(SourceProductionContext context, Request request)
    {
        (var msBuildConfig, var joints, var hasConfigurationOptions) = request;
        if (joints.Count == 0 && !hasConfigurationOptions) return;

        // The class sits in the namespace when there is one; its method one level in, and the method's statements one more.
        var classDepth = msBuildConfig.ExportConfiguration is { Namespace.Length: > 0 } ? 1 : 0;
        var statementDepth = classDepth + 2;

        var statements = new List<StatementSyntax>();
        if (hasConfigurationOptions)
        {
            // First, so options are bound from configuration before other joints configure them in code.
            statements.Add(
                YieldReturn(
                        NewObject(
                            IdentifierName("DovetailJointMetadata"),
                            Argument(NewObject(IdentifierName(OptionaryJoint.JointClassName))),
                            Argument(_hostTypeUndefined),
                            Argument(_categoryApplication)
                        )
                    )
                   .OnLine(statementDepth)
            );
        }

        var dependencyGraph = new List<(string Joint, IEnumerable<DovetailDependencyParser.DependencyEdge> Edges)>();
        var jointsByName = new Dictionary<string, ExportedJoint>(StringComparer.Ordinal);

        foreach (var joint in joints)
        {
            if (joint.Rejection is { } rejection)
            {
                context.ReportDiagnostic(
                    rejection == Diagnostics.DovetailHasNoCallableConstructor
                        ? Diagnostic.Create(rejection, joint.Location, joint.TypeName)
                        : Diagnostic.Create(rejection, joint.Location)
                );
                continue;
            }

            foreach (var (location, descriptor) in joint.Issues)
            {
                context.ReportDiagnostic(Diagnostic.Create(descriptor, location ?? joint.Location));
            }

            dependencyGraph.Add((joint.TypeName, joint.Edges));
            jointsByName[joint.TypeName] = joint;

            var usesFactory = joint.Factory is { };
            var createDovetail = joint.Factory is { } factory ? ParseExpression(factory) : NewObject(ParseTypeName(joint.TypeName));
            // What the joint declares wins; only a joint that declares nothing takes the project's default.
            var hostType = joint.HostType is { } declaredHostType
                ? HostTypeExpression(declaredHostType)
                : msBuildConfig.IsTestProject
                    ? _hostTypeUnitTestHost
                    : _hostTypeUndefined;

            var metadataArguments = new[] { Argument(createDovetail), Argument(hostType), Argument(CategoryExpression(joint.Category)), };
            // A joint that needs its constructor parameters resolved is a factory, which has its own entry point rather than a
            // constructor overload; see DovetailJointMetadata.
            ExpressionSyntax withDependencies = usesFactory
                ? InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("DovetailJointMetadata"), IdentifierName("FromFactory"))
                    )
                   .WithArgumentList(ArgumentList(CommaSeparated(metadataArguments)))
                : NewObject(IdentifierName("DovetailJointMetadata"), metadataArguments);

            foreach (var edge in joint.Edges)
            {
                var direction = edge.Direction == DovetailDependencyParser.DependencyKind.DependsOn ? _dependencyDirectionDependsOn : _dependencyDirectionDependentOf;
                withDependencies = InvocationExpression(
                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, withDependencies, IdentifierName("WithDependency"))
                    )
                   .WithArgumentList(ArgumentList(CommaSeparated(Argument(direction), Argument(TypeOfExpression(ParseName(edge.Target))))));
            }

            statements.Add(YieldReturn(withDependencies).OnLine(statementDepth));
        }

        // Every joint may have been left out, and an iterator without a yield is not a valid method.
        if (statements.Count == 0) statements.Add(YieldBreak().OnLine(statementDepth));

        foreach (var (joint, cycle) in DovetailDependencyParser.DetectCycles(dependencyGraph))
        {
            var chain = string.Join(" -> ", cycle.Select(c => jointsByName[c].Name));
            var declaration = jointsByName[joint];
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.CircularJointDependency, declaration.Location, declaration.Name, chain));
        }

        var exportsMethod = MethodDeclaration(
                GenericName(Identifier("IEnumerable"))
                   .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName("IDovetailJointMetadata"))))
                   .WithTrailingTrivia(Space),
                msBuildConfig.ExportConfiguration.MethodName
            )
           .WithModifiers(Modifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword))
           .WithParameterList(ParameterList())
           .WithBody(BlockAt(classDepth + 1, statements))
           .Member(classDepth + 1, "The conventions exports from this assembly");

        var helperClass =
            ClassDeclaration(msBuildConfig.ExportConfiguration.ClassName)
               .WithAttributeLists(SingletonList(CompilerGeneratedAttributes))
               .WithModifiers(Modifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword))
               .WithKeyword(SpaceAfter(SyntaxKind.ClassKeyword))
               .WithOpenBraceToken(OpenBrace(classDepth))
               .WithMembers(
                    Members(hasConfigurationOptions ? [exportsMethod, OptionaryJoint.Declaration(msBuildConfig.RootNamespace, classDepth + 1)] : [exportsMethod])
                )
               .WithCloseBraceToken(CloseBrace(classDepth))
               .Member(classDepth, "The class defined for exporting conventions from this assembly");

        var cu = CompilationUnit()
                .WithUsings(
                     List(
                         [
                             Using("System").WithLeadingTrivia(SharedHeaderTrivia),
                             Using("System.Collections.Generic"),
                             Using("Microsoft.Extensions.DependencyInjection"),
                             Using("Joinery.Dovetail"),
                             Using("Joinery.Dovetail.Infrastructure"),
                         ]
                     )
                 )
                // Without the Dovetail.Export.* metadata, referencing assemblies don't import these joints; the class is still
                // emitted so this assembly's own Imports keeps picking them up.
                .WithAttributeLists(msBuildConfig.ExportAssembly ? msBuildConfig.ExportConfiguration.ToAttributes() : default)
                .WithMembers(
                     SingletonList<MemberDeclarationSyntax>(
                         msBuildConfig.ExportConfiguration is { Namespace.Length: > 0 } data
                             ? InNamespace(data.Namespace, helperClass)
                             : helperClass
                     )
                 )
                .WithEndOfFileToken(EndOfFile(SharedFooterTrivia))
                .SeparateUsings();

        context.AddSource("Exported_Dovetails.g.cs", cu.GetText(Encoding.UTF8));
    }

    /// <summary>
    ///     The host type the joint declares, or <see langword="null" /> when it declares none. This mirrors what
    ///     <c>DovetailContextBuilder</c> reads from the joint's type at runtime: <c>[LiveJoint]</c> and <c>[UnitTestJoint]</c> first,
    ///     then <c>[DovetailHostType]</c>. All of them are inherited, so the attributes on base classes count, and the
    ///     nearest declaration wins.
    /// </summary>
    private static string? GetDeclaredHostType(INamedTypeSymbol joint)
    {
        var attributes = new List<AttributeData>();
        for (var type = joint; type is not null; type = type.BaseType)
        {
            foreach (var attribute in type.GetAttributes())
            {
                attributes.Add(attribute);
            }
        }

        foreach (var attribute in attributes)
        {
            switch (attribute.AttributeClass?.ToDisplayString())
            {
                case LiveJointAttributeName:
                    return "Live";
                case UnitTestJointAttributeName:
                    return "UnitTest";
                default:
                    break;
            }
        }

        foreach (var attribute in attributes)
        {
            if (attribute.AttributeClass?.ToDisplayString() != DovetailHostTypeAttributeName) continue;
            if (attribute.ConstructorArguments is not [{ Kind: TypedConstantKind.Enum, Type: INamedTypeSymbol enumType, Value: { } value }]) continue;

            // The generator doesn't reference Dovetail, so find the member the constructor argument names in the enum itself.
            if (EnumMemberName(enumType, value) is { } name) return name;
        }

        return null;
    }

    /// <summary>The name of the member of <paramref name="enumType" /> whose value is <paramref name="value" />, if there is one.</summary>
    private static string? EnumMemberName(INamedTypeSymbol enumType, object value)
    {
        foreach (var member in enumType.GetMembers())
        {
            if (member is IFieldSymbol { HasConstantValue: true } field && Equals(field.ConstantValue, value)) return field.Name;
        }

        return null;
    }

    /// <summary>How many of the joint's constructors the generated code can call, and that constructor when there is exactly one.</summary>
    private static (int Count, IMethodSymbol? Constructor) CallableConstructor(INamedTypeSymbol convention)
    {
        var count = 0;
        IMethodSymbol? callable = null;
        foreach (var constructor in convention.InstanceConstructors)
        {
            if (!CanBeCalled(constructor)) continue;
            count++;
            callable = constructor;
        }

        return (count, count == 1 ? callable : null);
    }

    private static MemberAccessExpressionSyntax HostTypeExpression(string name) => MemberAccessExpression(
        SyntaxKind.SimpleMemberAccessExpression,
        IdentifierName("DovetailHostType"),
        IdentifierName(name)
    );

    /// <summary>
    ///     The category the joint declares with <c>[DovetailCategory]</c>, or <see langword="null" /> when it declares none. This mirrors
    ///     what <c>DovetailContextBuilder</c> reads from the joint's type at runtime: the attribute is inherited from base classes (not
    ///     from interfaces, which attribute lookup on a type never reads) and the nearest declaration wins.
    /// </summary>
    private static string? GetDeclaredCategory(INamedTypeSymbol joint)
    {
        for (var type = joint; type is not null; type = type.BaseType)
        {
            foreach (var attribute in type.GetAttributes())
            {
                if (attribute.AttributeClass?.ToDisplayString() != DovetailCategoryAttributeName) continue;

                // The constructor takes a string, which the compiler has already folded to its value, whether it was written as
                // a literal, a constant such as DovetailCategory.Core or an expression of constants. An attribute that has no usable
                // value (null, or an argument that doesn't bind) declares nothing.
                if (attribute.ConstructorArguments is [{ Kind: TypedConstantKind.Primitive, Value: string category }]) return category;
            }
        }

        return null;
    }

    /// <summary>
    ///     The category as the generated code spells it. <c>DovetailCategory</c> converts implicitly from the string the attribute
    ///     takes, so a category without a constant of its own is written as its string literal. The well-known ones use their
    ///     constants, which is how a joint without the attribute has always been written.
    /// </summary>
    private static ExpressionSyntax CategoryExpression(string? category) => category switch
    {
        null or ApplicationCategory => _categoryApplication,
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
    ///     The factory that gives <c>DovetailJointMetadata</c> its joint, or <see langword="null" /> when the joint is created right
    ///     there with <c>new</c>, because it needs nothing (or is a struct that declares no constructor). A joint with constructor
    ///     parameters is handed over as a factory instead: the exported method has no parameters and no context to read from, so the
    ///     parameters are resolved when the context builder that ends up holding the metadata creates the joint, from its properties
    ///     (see <c>DovetailJointMetadata</c>). It is written as text and parsed when the exports are generated, which allocates less
    ///     than building it node by node.
    /// </summary>
    private static string? Factory(string typeName, IMethodSymbol? constructor)
    {
        if (constructor is not { Parameters.Length: > 0 }) return null;

        var arguments = constructor.Parameters.Select(parameter => ResolveParameter(typeName, parameter));
        return $"static {JointFactoryParameter} => new {typeName}({string.Join(", ", arguments)})";
    }

    private static ObjectCreationExpressionSyntax NewObject(TypeSyntax type, params IEnumerable<ArgumentSyntax> arguments) =>
        ObjectCreationExpression(SpaceAfter(SyntaxKind.NewKeyword), type, ArgumentList(CommaSeparated(arguments)), null);

    private static YieldStatementSyntax YieldReturn(ExpressionSyntax expression) =>
        YieldStatement(SyntaxKind.YieldReturnStatement, expression)
           .WithYieldKeyword(SpaceAfter(SyntaxKind.YieldKeyword))
           .WithReturnOrBreakKeyword(SpaceAfter(SyntaxKind.ReturnKeyword));

    private static YieldStatementSyntax YieldBreak() => YieldStatement(SyntaxKind.YieldBreakStatement).WithYieldKeyword(SpaceAfter(SyntaxKind.YieldKeyword));

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
        var isNullableValueType = parameterType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
        var type = parameterType.ToDisplayString();

        // GetService<T>() returns null for a missing value, which is what the fallbacks below key on. For a value type that can't hold
        // null it would return default(T), which can't be told apart from a value that was set to it, and `?? throw` doesn't compile
        // on it. Those are read as a T? instead: the same cast GetService<T>() makes, with a nullable result.
        var lookup = parameterType.IsValueType && !isNullableValueType
            ? $"({type}?){JointFactoryParameter}.GetService(typeof({type}))"
            : $"Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<{type}>({JointFactoryParameter})";

        if (!parameter.HasExplicitDefaultValue)
        {
            var message = $"Unable to resolve a value of type '{type}' for parameter '{parameter.Name}' of joint '{jointName}' from the Dovetail context. "
              + "Set it on the context builder before the joint is created, or give the parameter a default value.";
            return $"{lookup} ?? throw new System.InvalidOperationException({SymbolDisplay.FormatLiteral(message, quote: true)})";
        }

        // A missing value is already null, which is the default of a reference type or a nullable value type.
        var defaultValue = DefaultValue(parameter, parameterType, type);
        return defaultValue == "null" ? lookup : $"{lookup} ?? {defaultValue}";
    }

    /// <summary>
    ///     The default value the parameter declares, as an expression typed by the parameter and not by the literal, so the numeric
    ///     literals are cast to it. A negative number is parenthesized, so the cast reads as one for an enum too.
    /// </summary>
    private static string DefaultValue(IParameterSymbol parameter, ITypeSymbol parameterType, string type)
    {
        return parameter.ExplicitDefaultValue switch
        {
            string text => SymbolDisplay.FormatLiteral(text, quote: true),
            bool flag => flag ? "true" : "false",
            char character => $"({type}){SymbolDisplay.FormatLiteral(character, quote: true)}",
            sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal =>
                SymbolDisplay.FormatPrimitive(parameter.ExplicitDefaultValue, quoteStrings: false, useHexadecimalNumbers: false) is ['-', ..] negative
                    ? $"({type})({negative})"
                    : $"({type}){SymbolDisplay.FormatPrimitive(parameter.ExplicitDefaultValue, quoteStrings: false, useHexadecimalNumbers: false)}",
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

    public record Request(MsBuildConfig BuildConfig, EquatableArray<ExportedJoint> ExportedDovetails, bool HasConfigurationOptions);
}
