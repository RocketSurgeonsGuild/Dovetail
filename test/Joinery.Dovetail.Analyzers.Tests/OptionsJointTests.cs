using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The joint Dovetail exports for an assembly's <c>[DovetailConfiguration]</c> declarations: listed first in the exports, it
///     calls <c>OptionaryConfigurationOptions.AddOptionaryConfigurationOptions</c> in the <c>RootNamespace</c> build property (not
///     <c>DovetailExportNamespace</c>). The joint generator decides on its own whether there is anything to call, from the
///     declarations, so there is no joint without them or below C# 12. Both generators run, as in a real build.
/// </summary>
public class OptionsJointTests() : GeneratorTest()
{
    private const string Usings = """
        using Joinery.Dovetail.Attributes;

        """;

    private const string OptionsClass = Usings
      + """
        namespace Sample;

        [DovetailConfiguration]
        public class GreetingsOptions
        {
            public string Message { get; set; } = "Hello";
        }
        """;

    // Another exported joint, so the snapshot shows the options joint ahead of it.
    private const string OtherJoint = """
        using Joinery.Dovetail.Attributes;
        using Joinery.Dovetail.Joints;
        using Microsoft.Extensions.DependencyInjection;

        namespace Sample;

        [DovetailExport]
        public class OtherJoint : IServiceJoint
        {
            public void Register(Joinery.Dovetail.IDovetailContext context, IServiceCollection services) { }
        }
        """;

    [Test]
    public async Task Exports_a_joint_first_that_calls_the_registration_method_in_the_root_namespace()
    {
        // The exports class lives somewhere else on purpose: the joint calls the method in RootNamespace, not DovetailExportNamespace.
        var result = await Joints()
                          .AddGlobalOption("build_property.RootNamespace", "Sample.App")
                          .AddGlobalOption("build_property.DovetailExportNamespace", "Sample.Exports")
                          .AddSources(OptionsClass, OtherJoint)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_a_joint_that_calls_the_registration_method_in_the_global_namespace_without_a_root_namespace()
    {
        var result = await Joints().AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_the_joint_for_an_assembly_declaration()
    {
        var result = await Joints()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources(
                               Usings
                             + """
                               [assembly: DovetailConfiguration<Sample.ExternalOptions>("External")]

                               namespace Sample;

                               public class ExternalOptions
                               {
                                   public string? Value { get; set; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_no_joint_without_declarations()
    {
        var result = await Joints()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources("namespace Sample; public class PlainOptions { public string? Value { get; set; } }", OtherJoint)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_no_joint_below_CSharp12()
    {
        var result = await Joints()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .WithLanguageVersion(LanguageVersion.CSharp11)
                          .AddSources(OptionsClass)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Keeps_the_declaration_check_cached_when_unrelated_code_changes()
    {
        var result = await Joints().AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        var compilation = result.InputCompilation;
        var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new DovetailAttributesGenerator().AsSourceGenerator()],
            parseOptions: parseOptions,
            driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
        );
        driver = driver.RunGenerators(compilation, TestContext.CancellationToken);
        driver = driver.RunGenerators(
            compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("namespace Sample; public class Unrelated { }", parseOptions, cancellationToken: TestContext.CancellationToken)),
            TestContext.CancellationToken
        );

        var steps = driver.GetRunResult().Results.Single().TrackedSteps;
        await steps["dovetail:configuration_declarations"]
           .SelectMany(z => z.Outputs)
           .Select(z => z.Reason).Should().All(z => z is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged);
    }

    private GeneratorTestContextBuilder Joints() => Builder
                                                   .AddConfigurationOptionsReferences()
                                                   .AddReferences(typeof(IValidateOptions<>))
                                                   .WithDiagnosticSeverity(DiagnosticSeverity.Warning);
}
