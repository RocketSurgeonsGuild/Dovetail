using System.Reflection;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
/// The metadata generated for an exported joint has to carry the category the joint declares with <c>[DovetailCategory]</c>, as the
/// runtime reflection path (<c>DovetailContextBuilder.AppendJoint(IDovetailJoint)</c>) does, and <c>Application</c> for a joint that
/// declares none. These tests read the category off the metadata that <c>Exports.Rivet()</c> returns from the compiled output.
/// </summary>
public class ExportedCategoryTests() : GeneratorTest()
{
    [Test]
    [Arguments("", "Application")]
    [Arguments("[DovetailCategory(DovetailCategory.Application)]", "Application")]
    [Arguments("[DovetailCategory(DovetailCategory.Core)]", "Core")]
    // The attribute takes a plain string, so a literal is the same category as the constant with that value.
    [Arguments("""[DovetailCategory("Core")]""", "Core")]
    [Arguments("""[DovetailCategory("Application")]""", "Application")]
    [Arguments("""[DovetailCategory("Custom")]""", "Custom")]
    // The value is whatever the attribute was given, so it needs no matching constant and keeps its spelling.
    [Arguments("""[DovetailCategory("core")]""", "core")]
    [Arguments("""[DovetailCategory("")]""", "")]
    [Arguments("""[DovetailCategory("Say \"hi\"\n\\")]""", "Say \"hi\"\n\\")]
    // A constant of the project's own is folded to its value by the compiler.
    [Arguments("[DovetailCategory(MyCategories.Billing)]", "Billing")]
    [Arguments("""[DovetailCategory(MyCategories.Billing + ".Core")]""", "Billing.Core")]
    public async Task Exports_The_Category_The_Joint_Declares(string attributes, string expected)
    {
        var result = await Generate($"[DovetailExport] {attributes} public class Contrib : IDovetailJoint {{ }}");

        CompileErrors(result).ShouldBeEmpty();
        Category(result).ToString().ShouldBe(expected);
        Category(result).ShouldBe((DovetailCategory)expected);
    }

    [Test]
    [Arguments("DovetailCategory.Core", "DovetailCategory.Core")]
    [Arguments("DovetailCategory.Application", "DovetailCategory.Application")]
    [Arguments("\"Custom\"", "\"Custom\"")]
    public async Task Writes_The_Category_Into_The_Metadata(string argument, string expected)
    {
        var result = await Generate($"[DovetailExport] [DovetailCategory({argument})] public class Contrib : IDovetailJoint {{ }}");

        ExportedSource(result).ShouldContain($"new Sample.Contrib(), DovetailHostType.Undefined, {expected})");
    }

    [Test]
    public async Task A_Joint_Without_The_Attribute_Is_Still_Exported_As_Application()
    {
        var result = await Generate("[DovetailExport] public class Contrib : IDovetailJoint { }");

        ExportedSource(result).ShouldContain("new Sample.Contrib(), DovetailHostType.Undefined, DovetailCategory.Application)");
        Category(result).ToString().ShouldBe("Application");
    }

    [Test]
    public async Task The_Category_Is_Independent_Of_The_Host_Type()
    {
        var result = await Generate("[DovetailExport] [LiveJoint] [DovetailCategory(DovetailCategory.Core)] public class Contrib : IDovetailJoint { }");

        ExportedSource(result).ShouldContain("new Sample.Contrib(), DovetailHostType.Live, DovetailCategory.Core)");
        var metadata = Rivet(result).Single();
        metadata.Category.ToString().ShouldBe("Core");
        metadata.HostType.ShouldBe(DovetailHostType.Live);
    }

    [Test]
    public async Task Every_Joint_Gets_Its_Own_Category()
    {
        var result = await Generate(
            """
            [DovetailExport] [DovetailCategory(DovetailCategory.Core)] public class FirstCore : IDovetailJoint { }
            [DovetailExport] public class Plain : IDovetailJoint { }
            [DovetailExport] [DovetailCategory("Custom")] public class Custom : IDovetailJoint { }
            [DovetailExport] [DovetailCategory(DovetailCategory.Core)] public class SecondCore : IDovetailJoint { }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
        // The metadata comes out in the generator's own order, which is not what is being checked here.
        CategoriesByJoint(result).ShouldBe(["Custom: Custom", "FirstCore: Core", "Plain: Application", "SecondCore: Core"]);
    }

    [Test]
    [Arguments("[DovetailCategory(DovetailCategory.Core)]", "Core")]
    [Arguments("""[DovetailCategory("Custom")]""", "Custom")]
    [Arguments("", "Application")]
    public async Task A_Joint_With_Constructor_Parameters_Carries_The_Category_Too(string attributes, string expected)
    {
        var result = await Generate($"[DovetailExport] {attributes} public class Contrib(WidgetOptions? options = null) : IDovetailJoint {{ public WidgetOptions? Options => options; }}");

        CompileErrors(result).ShouldBeEmpty();
        // This is the factory form, which has its own entry point rather than a constructor overload.
        ExportedSource(result).ShouldContain("DovetailJointMetadata.FromFactory(");
        Category(result).ToString().ShouldBe(expected);

        // And the joint still gets what its parameters ask for.
        var options = new WidgetOptions();
        var context = await DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []).Set(options).CreateAsync(TestContext.CancellationToken);
        var joint = context.Joints.Single();
        joint.GetType().GetProperty("Options")!.GetValue(joint).ShouldBeSameAs(options);
    }

    [Test]
    public async Task The_Category_Is_Inherited_From_A_Base_Class()
    {
        var result = await Generate(
            """
            [DovetailCategory(DovetailCategory.Core)] public abstract class Base : IDovetailJoint { }
            [DovetailExport] public class Contrib : Base { }
            """
        );

        ExportedSource(result).ShouldContain("new Sample.Contrib(), DovetailHostType.Undefined, DovetailCategory.Core)");
        Category(result).ToString().ShouldBe("Core");
        AssertCategoryMatchesTheRuntimeScan(result);
    }

    [Test]
    public async Task The_Nearest_Declaration_Wins()
    {
        var result = await Generate(
            """
            [DovetailCategory(DovetailCategory.Core)] public abstract class Base : IDovetailJoint { }
            [DovetailCategory("Custom")] public abstract class Middle : Base { }
            [DovetailExport] public class Contrib : Middle { }
            [DovetailExport] [DovetailCategory(DovetailCategory.Application)] public class Overriding : Middle { }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
        CategoriesByJoint(result).ShouldBe(["Contrib: Custom", "Overriding: Application"]);
        AssertCategoryMatchesTheRuntimeScan(result);
    }

    [Test]
    public async Task A_Category_On_An_Interface_The_Joint_Implements_Is_Not_Inherited()
    {
        // Attribute lookup on a type walks its base classes and never its interfaces, which is what the runtime scan sees.
        var result = await Generate(
            """
            [DovetailCategory(DovetailCategory.Core)] public interface IMarker : IDovetailJoint { }
            [DovetailExport] public class Contrib : IMarker { }
            """
        );

        Category(result).ToString().ShouldBe("Application");
        AssertCategoryMatchesTheRuntimeScan(result);
    }

    [Test]
    [Arguments("")]
    [Arguments("[DovetailCategory(DovetailCategory.Core)]")]
    [Arguments("""[DovetailCategory("Custom")]""")]
    [Arguments("[LiveJoint] [DovetailCategory(DovetailCategory.Core)]")]
    public async Task Generated_Metadata_Has_The_Category_The_Runtime_Scan_Reads(string attributes)
    {
        var result = await Generate($"[DovetailExport] {attributes} public class Contrib : IDovetailJoint {{ }}");

        AssertCategoryMatchesTheRuntimeScan(result);
    }

    [Test]
    public async Task Generates_The_Category_Of_Every_Kind_Of_Exported_Joint()
    {
        var result = await Generate(
            """
            [DovetailExport] public class Plain : IDovetailJoint { }
            [DovetailExport] [DovetailCategory(DovetailCategory.Core)] public class CoreJoint : IDovetailJoint { }
            [DovetailExport] [DovetailCategory("Custom")] public class CustomJoint : IDovetailJoint { }
            [DovetailExport] [DovetailCategory(DovetailCategory.Core)] public class CoreFactory(WidgetOptions? options = null) : IDovetailJoint { }
            [DovetailExport] public class PlainFactory(WidgetOptions? options = null) : IDovetailJoint { }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
        await Verify(ExportedSource(result), "cs");
    }

    private async Task<GeneratorTestResults> Generate(string declarations) =>
        await Builder
             .AddReferences(typeof(WidgetOptions))
             .AddSources(
                  $$"""
                    using Joinery.Dovetail;
                    using Joinery.Dovetail.Analyzers.Tests;
                    using Joinery.Dovetail.Attributes;
                    using Joinery.Dovetail.Infrastructure;

                    namespace Sample
                    {
                        public static class MyCategories { public const string Billing = "Billing"; }

                        {{declarations}}
                    }
                    """
              )
             .Build()
             .GenerateAsync(TestContext.CancellationToken);

    private static IEnumerable<Diagnostic> CompileErrors(GeneratorTestResults result) => result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error);

    private static string ExportedSource(GeneratorTestResults result) =>
        result
           .Results[typeof(DovetailAttributesGenerator)]
           .SyntaxTrees
           .Single(z => z.FilePath.EndsWith("Exported_Dovetails.g.cs", StringComparison.Ordinal))
           .ToString();

    /// <summary>
    /// The metadata <c>Exports.Rivet()</c> returns from the assembly the generator output was compiled into.
    /// </summary>
    private static IEnumerable<IDovetailJointMetadata> Rivet(GeneratorTestResults result)
    {
        var assembly = result.Assembly;
        assembly.ShouldNotBeNull(string.Join(Environment.NewLine, CompileErrors(result)));

        return (IEnumerable<IDovetailJointMetadata>)assembly.GetType("Exports")!.GetMethod("Rivet")!.Invoke(null, null)!;
    }

    private static DovetailCategory Category(GeneratorTestResults result) => Rivet(result).Single().Category;

    private static string[] CategoriesByJoint(GeneratorTestResults result) =>
        [.. Rivet(result).Select(z => $"{z.Joint.GetType().Name}: {z.Category}").Order(StringComparer.Ordinal)];

    /// <summary>
    /// Compares each generated category with the one the runtime path takes from the same joint's type. That is
    /// <c>DovetailContextBuilder.FromDovetail</c>: the category of the <c>[DovetailCategory]</c> attribute, otherwise <c>Application</c>.
    /// </summary>
    private static void AssertCategoryMatchesTheRuntimeScan(GeneratorTestResults result)
    {
        foreach (var metadata in Rivet(result))
        {
            var runtime = metadata.Joint.GetType().GetCustomAttribute<DovetailCategoryAttribute>()?.Category ?? DovetailCategory.Application;

            metadata.Category.ShouldBe(runtime, DovetailCategory.ValueComparer, metadata.Joint.GetType().Name);
        }
    }
}
