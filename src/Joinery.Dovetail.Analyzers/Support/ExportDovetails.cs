using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Joinery.Dovetail.Helpers;

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
        var (hostType, category) = GetDeclaredHostTypeAndCategory(convention);
        return new(
            convention.Name,
            typeName,
            convention.MetadataName,
            location,
            null,
            Factory(typeName, callableConstructor),
            hostType,
            category,
            edges.ToEquatableArray(),
            issues.ToEquatableArray()
        );
    }

    public static void HandleDovetailExports(SourceProductionContext context, Request request)
    {
        (var msBuildConfig, var joints, var hasConfigurationOptions) = request;
        if (joints.Count == 0 && !hasConfigurationOptions) return;

        var statements = new List<string>();
        if (hasConfigurationOptions)
        {
            // First, so options are bound from configuration before other joints configure them in code.
            statements.Add($"yield return new DovetailJointMetadata(new {OptionaryJoint.JointClassName}(), {HostTypeUndefined}, {CategoryApplication});");
        }

        var dependencyGraph = new List<(string Joint, IEnumerable<DovetailDependencyParser.DependencyEdge> Edges)>();
        var jointsByName = new Dictionary<string, ExportedJoint>(StringComparer.Ordinal);
        var statement = new StringBuilder();

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

            // What the joint declares wins; only a joint that declares nothing takes the project's default.
            var hostType = joint.HostType is { } declaredHostType
                ? $"DovetailHostType.{declaredHostType}"
                : msBuildConfig.IsTestProject
                    ? HostTypeUnitTest
                    : HostTypeUndefined;

            // A joint that needs its constructor parameters resolved is a factory, which has its own entry point rather than a
            // constructor overload; see DovetailJointMetadata.
            statement.Clear();
            if (joint.Factory is { } factory)
            {
                statement.Append("yield return DovetailJointMetadata.FromFactory(").Append(factory);
            }
            else
            {
                statement.Append("yield return new DovetailJointMetadata(new ").Append(joint.TypeName).Append("()");
            }

            statement.Append(", ").Append(hostType).Append(", ").Append(CategoryExpression(joint.Category)).Append(')');

            foreach (var edge in joint.Edges)
            {
                statement
                   .Append(".WithDependency(")
                   .Append(edge.Direction == DovetailDependencyParser.DependencyKind.DependsOn ? DependsOn : DependentOf)
                   .Append(", typeof(")
                   .Append(edge.Target)
                   .Append("))");
            }

            statements.Add(statement.Append(';').ToString());
        }

        // Every joint may have been left out, and an iterator without a yield is not a valid method.
        if (statements.Count == 0) statements.Add("yield break;");

        foreach (var (joint, cycle) in DovetailDependencyParser.DetectCycles(dependencyGraph))
        {
            var chain = string.Join(" -> ", cycle.Select(c => jointsByName[c].Name));
            var declaration = jointsByName[joint];
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.CircularJointDependency, declaration.Location, declaration.Name, chain));
        }

        var exportConfiguration = msBuildConfig.ExportConfiguration;
        var writer = new SourceWriter()
                    .WriteSharedHeader()
                    .Line("using System;")
                    .Line("using System.Collections.Generic;")
                    .Line("using Microsoft.Extensions.DependencyInjection;")
                    .Line("using Joinery.Dovetail;")
                    .Line("using Joinery.Dovetail.Infrastructure;")
                    .BlankLine();
        // Without the Dovetail.Export.* metadata, referencing assemblies don't import these joints; the class is still
        // emitted so this assembly's own Imports keeps picking them up.
        if (msBuildConfig.ExportAssembly) exportConfiguration.WriteAttributes(writer);

        var inNamespace = exportConfiguration.Namespace is { Length: > 0 };
        if (inNamespace) writer.Line($"namespace {exportConfiguration.Namespace}").OpenBlock();

        writer
           .Summary("The class defined for exporting conventions from this assembly")
           .Line(CompilerGeneratedAttributes)
           .Line($"public static partial class {exportConfiguration.ClassName}")
           .OpenBlock()
           .Summary("The conventions exports from this assembly")
           .Line($"public static IEnumerable<IDovetailJointMetadata> {exportConfiguration.MethodName}()")
           .OpenBlock();
        foreach (var line in statements)
        {
            writer.Line(line);
        }

        writer.CloseBlock();
        if (hasConfigurationOptions) OptionaryJoint.Write(writer.BlankLine(), msBuildConfig.RootNamespace);
        writer.CloseBlock();

        if (inNamespace) writer.CloseBlock();

        context.AddSource("Exported_Dovetails.g.cs", writer.WriteSharedFooter().ToSourceText());
    }

    /// <summary>
    ///     The host type and category the joint declares, each <see langword="null" /> when it declares none, read in one walk up its
    ///     base classes. This mirrors what <c>DovetailContextBuilder</c> reads from the joint's type at runtime. All of the attributes
    ///     are inherited, so the ones on base classes count (not those on interfaces, which attribute lookup on a type never reads),
    ///     and the nearest declaration wins. For the host type, <c>[LiveJoint]</c> and <c>[UnitTestJoint]</c> anywhere win over
    ///     <c>[DovetailHostType]</c>.
    /// </summary>
    private static (string? HostType, string? Category) GetDeclaredHostTypeAndCategory(INamedTypeSymbol joint)
    {
        string? marker = null, declaredHostType = null, category = null;
        for (var type = joint; type is not null; type = type.BaseType)
        {
            foreach (var attribute in type.GetAttributes())
            {
                if (attribute.AttributeClass is not { } attributeClass || !IsDovetailAttribute(attributeClass)) continue;
                switch (attributeClass.Name)
                {
                    case LiveJointAttributeName:
                        marker ??= "Live";
                        break;
                    case UnitTestJointAttributeName:
                        marker ??= "UnitTest";
                        break;
                    // The generator doesn't reference Dovetail, so find the member the constructor argument names in the enum itself.
                    case DovetailHostTypeAttributeName when declaredHostType is null
                                                         && attribute.ConstructorArguments is [{ Kind: TypedConstantKind.Enum, Type: INamedTypeSymbol enumType, Value: { } value }]:
                        declaredHostType = EnumMemberName(enumType, value);
                        break;
                    // The constructor takes a string, which the compiler has already folded to its value, whether it was written as
                    // a literal, a constant such as DovetailCategory.Core or an expression of constants. An attribute that has no
                    // usable value (null, or an argument that doesn't bind) declares nothing.
                    case DovetailCategoryAttributeName when category is null
                                                         && attribute.ConstructorArguments is [{ Kind: TypedConstantKind.Primitive, Value: string declared }]:
                        category = declared;
                        break;
                    default:
                        break;
                }
            }
        }

        return (marker ?? declaredHostType, category);
    }

    /// <summary>
    ///     Whether <paramref name="attributeClass" /> is declared in <c>Joinery.Dovetail.Attributes</c>, checked by walking its
    ///     namespaces rather than formatting its name, which this did for every attribute up every joint's base classes.
    /// </summary>
    private static bool IsDovetailAttribute(INamedTypeSymbol attributeClass) =>
        attributeClass is
        {
            ContainingType: null,
            ContainingNamespace:
            {
                Name: "Attributes",
                ContainingNamespace: { Name: "Dovetail", ContainingNamespace: { Name: "Joinery", ContainingNamespace.IsGlobalNamespace: true } },
            },
        };

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

    /// <summary>
    ///     The category as the generated code spells it. <c>DovetailCategory</c> converts implicitly from the string the attribute
    ///     takes, so a category without a constant of its own is written as its string literal. The well-known ones use their
    ///     constants, which is how a joint without the attribute has always been written.
    /// </summary>
    private static string CategoryExpression(string? category) => category switch
    {
        null or ApplicationCategory => CategoryApplication,
        CoreCategory => $"DovetailCategory.{CoreCategory}",
        _ => StringLiteral(category),
    };

    private const string ApplicationCategory = "Application";
    private const string CoreCategory = "Core";
    private const string DovetailCategoryAttributeName = "DovetailCategoryAttribute";
    private const string LiveJointAttributeName = "LiveJointAttribute";
    private const string UnitTestJointAttributeName = "UnitTestJointAttribute";
    private const string DovetailHostTypeAttributeName = "DovetailHostTypeAttribute";

    /// <summary>
    ///     The factory that gives <c>DovetailJointMetadata</c> its joint, or <see langword="null" /> when the joint is created right
    ///     there with <c>new</c>, because it needs nothing (or is a struct that declares no constructor). A joint with constructor
    ///     parameters is handed over as a factory instead: the exported method has no parameters and no context to read from, so the
    ///     parameters are resolved when the context builder that ends up holding the metadata creates the joint, from its properties
    ///     (see <c>DovetailJointMetadata</c>).
    /// </summary>
    private static string? Factory(string typeName, IMethodSymbol? constructor)
    {
        if (constructor is not { Parameters.Length: > 0 }) return null;

        var arguments = constructor.Parameters.Select(parameter => ResolveParameter(typeName, parameter));
        return $"static {JointFactoryParameter} => new {typeName}({string.Join(", ", arguments)})";
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

    private const string CategoryApplication = $"DovetailCategory.{ApplicationCategory}";
    private const string HostTypeUndefined = "DovetailHostType.Undefined";
    private const string HostTypeUnitTest = "DovetailHostType.UnitTest";
    private const string DependsOn = "DependencyDirection.DependsOn";
    private const string DependentOf = "DependencyDirection.DependentOf";

    public record Request(MsBuildConfig BuildConfig, EquatableArray<ExportedJoint> ExportedDovetails, bool HasConfigurationOptions);
}
