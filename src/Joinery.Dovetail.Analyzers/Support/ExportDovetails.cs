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
    /// <param name="Parameters">
    ///     How the joint's constructor parameters are resolved. A joint with any is created by a factory; one without is created with <c>new</c>.
    /// </param>
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
        EquatableArray<JointParameter> Parameters,
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
        if (rejection is { }) return new(convention.Name, typeName, convention.MetadataName, location, rejection, default, null, null, default, default);

        var (edges, issues) = DovetailDependencyParser.ParseDependencies(convention);
        return new(
            convention.Name,
            typeName,
            convention.MetadataName,
            location,
            null,
            Parameters(typeName, callableConstructor),
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

            var usesFactory = joint.Parameters.Count > 0;
            ExpressionSyntax createDovetail = usesFactory ? FactoryLambda(joint.TypeName, joint.Parameters) : NewObject(ParseTypeName(joint.TypeName));
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

    /// <summary>How the factory resolves one of the joint's constructor parameters.</summary>
    /// <param name="Type">The parameter's type as the generated code names it, which is the type it is looked up by.</param>
    /// <param name="IsNonNullableValueType">Whether the type is a value type that can't hold null, which is read as a nullable one.</param>
    /// <param name="MissingMessage">The message thrown when there is no value and no default, or <see langword="null" /> when the parameter has a default.</param>
    /// <param name="DefaultKind">What the parameter's default value is.</param>
    /// <param name="DefaultValue">The default value, when <paramref name="DefaultKind" /> is <see cref="JointParameterDefault.Constant" />.</param>
    public sealed record JointParameter(string Type, bool IsNonNullableValueType, string? MissingMessage, JointParameterDefault DefaultKind, object? DefaultValue);

    public enum JointParameterDefault
    {
        /// <summary>A missing value is already the default: null for a reference type or a nullable value type.</summary>
        Null,

        /// <summary>The type's <c>default</c>, for a value type that can't hold null.</summary>
        TypeDefault,

        /// <summary>The constant the parameter declares.</summary>
        Constant,
    }

    /// <summary>
    ///     How the factory that gives <c>DovetailJointMetadata</c> its joint resolves each constructor parameter, or nothing when the
    ///     joint is created right there with <c>new</c>, because it needs nothing (or is a struct that declares no constructor). A joint
    ///     with constructor parameters is handed over as a factory instead: the exported method has no parameters and no context to
    ///     read from, so the parameters are resolved when the context builder that ends up holding the metadata creates the joint,
    ///     from its properties (see <c>DovetailJointMetadata</c>).
    /// </summary>
    private static EquatableArray<JointParameter> Parameters(string typeName, IMethodSymbol? constructor) =>
        constructor is { Parameters.Length: > 0 }
            ? constructor.Parameters.Select(parameter => DescribeParameter(typeName, parameter)).ToEquatableArray()
            : default;

    /// <summary>The factory: <c>static services =&gt; new Joint(...)</c>, with each argument resolved from <c>services</c>.</summary>
    private static SimpleLambdaExpressionSyntax FactoryLambda(string typeName, EquatableArray<JointParameter> parameters) =>
        SimpleLambdaExpression(Parameter(Identifier(JointFactoryParameter)), NewObject(ParseTypeName(typeName), parameters.Select(parameter => Argument(ResolveParameter(parameter)))))
           .WithModifiers(Modifiers(SyntaxKind.StaticKeyword))
           .WithArrowToken(Spaced(SyntaxKind.EqualsGreaterThanToken));

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
    private static JointParameter DescribeParameter(string jointName, IParameterSymbol parameter)
    {
        // Looked up by the type as declared. For a nullable value type that is Nullable<T>, which is what Set<int?>() stores it under.
        var parameterType = parameter.Type.WithNullableAnnotation(NullableAnnotation.None);
        var isNullableValueType = parameterType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
        var type = parameterType.ToDisplayString();
        var isNonNullableValueType = parameterType.IsValueType && !isNullableValueType;

        if (!parameter.HasExplicitDefaultValue)
        {
            var message = $"Unable to resolve a value of type '{type}' for parameter '{parameter.Name}' of joint '{jointName}' from the Dovetail context. "
              + "Set it on the context builder before the joint is created, or give the parameter a default value.";
            return new(type, isNonNullableValueType, message, JointParameterDefault.Null, null);
        }

        return parameter.ExplicitDefaultValue switch
        {
            string or bool or char or sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal =>
                new(type, isNonNullableValueType, null, JointParameterDefault.Constant, parameter.ExplicitDefaultValue),
            // null, and anything the generator has no literal for, is the type's default: null for a reference type or a nullable
            // value type, default for any other value type.
            _ => new(type, isNonNullableValueType, null, isNonNullableValueType ? JointParameterDefault.TypeDefault : JointParameterDefault.Null, null),
        };
    }

    /// <summary>The expression that resolves <paramref name="parameter" /> from the factory's <c>services</c>.</summary>
    private static ExpressionSyntax ResolveParameter(JointParameter parameter)
    {
        var type = ParseTypeName(parameter.Type);
        var services = IdentifierName(JointFactoryParameter);

        // GetService<T>() returns null for a missing value, which is what the fallbacks below key on. For a value type that can't hold
        // null it would return default(T), which can't be told apart from a value that was set to it, and `?? throw` doesn't compile
        // on it. Those are read as a T? instead: the same cast GetService<T>() makes, with a nullable result.
        ExpressionSyntax lookup = parameter.IsNonNullableValueType
            ? CastExpression(
                NullableType(type),
                InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, services, IdentifierName("GetService")))
                   .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(TypeOfExpression(type)))))
            )
            : InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        ParseName("Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions"),
                        GenericName(Identifier("GetService")).WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(type)))
                    )
                )
               .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(services))));

        return  parameter.MissingMessage is { } message 
            ?   Coalesce(
                lookup,
                ThrowExpression(
                    SpaceAfter(SyntaxKind.ThrowKeyword),
                    NewObject(ParseTypeName("System.InvalidOperationException"), Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(message))))
                )
            )  
            : parameter.DefaultKind switch
            {
                JointParameterDefault.TypeDefault => Coalesce(lookup, LiteralExpression(SyntaxKind.DefaultLiteralExpression)),
                JointParameterDefault.Constant => Coalesce(lookup, ConstantExpression(type, parameter.DefaultValue)),
                JointParameterDefault.Null => throw new NotImplementedException(),
                _ => lookup,
            };
    }

    /// <summary>
    ///     The default value the parameter declares, as an expression typed by the parameter and not by the literal, so the numeric
    ///     and character literals are cast to it. A negative number is parenthesized, so the cast reads as one for an enum too.
    /// </summary>
    private static ExpressionSyntax ConstantExpression(TypeSyntax type, object? value)
    {
        switch (value)
        {
            case string text:
                return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(text));
            case bool flag:
                return LiteralExpression(flag ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
            case char character:
                return CastExpression(type, LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal(character)));
            default:
                break;
        }

        var formatted = SymbolDisplay.FormatPrimitive(value!, quoteStrings: false, useHexadecimalNumbers: false) ?? "0";
        var negative = formatted.StartsWith("-", StringComparison.Ordinal);
        var digits = negative ? formatted[1..] : formatted;
        var token = value switch
        {
            uint number => Literal(digits, number),
            long number => Literal(digits, number),
            ulong number => Literal(digits, number),
            float number => Literal(digits, number),
            double number => Literal(digits, number),
            decimal number => Literal(digits, number),
            _ => Literal(digits, Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture)),
        };
        ExpressionSyntax literal = LiteralExpression(SyntaxKind.NumericLiteralExpression, token);
        if (negative) literal = ParenthesizedExpression(PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, literal));
        return CastExpression(type, literal);
    }

    private static BinaryExpressionSyntax Coalesce(ExpressionSyntax left, ExpressionSyntax right) =>
        BinaryExpression(SyntaxKind.CoalesceExpression, left, Spaced(SyntaxKind.QuestionQuestionToken), right);

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
