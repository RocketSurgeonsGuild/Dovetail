using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The joint Dovetail exports for an assembly's <c>[Optionary]</c> declarations: listed first in the exports, it calls
///     <c>OptionaryConfigurationOptions.AddOptionaryConfigurationOptions</c> in the <c>RootNamespace</c> build property (not
///     <c>DovetailExportNamespace</c>). There is no joint without declarations, below C# 12, or when the attribute comes from an
///     assembly that isn't named <c>Optionary</c>.
/// </summary>
/// <remarks>
///     The attributes come from a stub assembly named <c>Optionary</c>, and the registration method Optionary's generator would emit
///     is a plain source file, so these tests don't depend on Optionary's generator.
/// </remarks>
public class OptionsJointTests() : GeneratorTest()
{
    private const string OptionaryStub = """
        namespace Optionary;

        [System.AttributeUsage(System.AttributeTargets.Class)]
        public sealed class OptionaryAttribute : System.Attribute
        {
            public OptionaryAttribute() { }
            public OptionaryAttribute(string section) { }
        }

        [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = true)]
        public sealed class OptionaryAttribute<T> : System.Attribute where T : class
        {
            public OptionaryAttribute() { }
            public OptionaryAttribute(string section) { }
        }
        """;

    private const string OptionsClass = """
        using Optionary;

        namespace Sample;

        [Optionary]
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

    private static readonly MetadataReference Optionary = Stub("Optionary");
    private static readonly MetadataReference NotOptionary = Stub("NotOptionary");

    [Test]
    public async Task Exports_a_joint_first_that_calls_the_registration_method_in_the_root_namespace()
    {
        // The exports class lives somewhere else on purpose: the joint calls the method in RootNamespace, not DovetailExportNamespace.
        var result = await Joints(Optionary)
                          .AddGlobalOption("build_property.RootNamespace", "Sample.App")
                          .AddGlobalOption("build_property.DovetailExportNamespace", "Sample.Exports")
                          .AddSources(OptionsClass, OtherJoint, RegistrationMethod("Sample.App"))
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_a_joint_that_calls_the_registration_method_in_the_global_namespace_without_a_root_namespace()
    {
        var result = await Joints(Optionary).AddSources(OptionsClass, RegistrationMethod(null)).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_the_joint_for_an_assembly_declaration()
    {
        var result = await Joints(Optionary)
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources(
                               """
                               [assembly: Optionary.Optionary<Sample.ExternalOptions>("External")]

                               namespace Sample;

                               public class ExternalOptions
                               {
                                   public string Value { get; set; } = "";
                               }
                               """,
                               RegistrationMethod("Sample")
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_no_joint_without_declarations()
    {
        var result = await Joints(Optionary)
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources("namespace Sample; public class PlainOptions { public string Value { get; set; } = \"\"; }", OtherJoint)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_no_joint_below_CSharp12()
    {
        var result = await Joints(Optionary)
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .WithLanguageVersion(LanguageVersion.CSharp11)
                          .AddSources(OptionsClass)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_no_joint_for_an_attribute_from_another_assembly()
    {
        var result = await Joints(NotOptionary)
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources(OptionsClass, OtherJoint)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Keeps_the_declaration_check_cached_when_unrelated_code_changes()
    {
        var result = await Joints(Optionary).AddSources(OptionsClass, RegistrationMethod(null)).Build().GenerateAsync(TestContext.CancellationToken);

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
        await steps["dovetail:optionary_declarations"]
             .SelectMany(z => z.Outputs)
             .Select(z => z.Reason)
             .Should()
             .All(z => z is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged);
    }

    /// <summary>What Optionary's generator emits in the consumer, reduced to the method's shape.</summary>
    private static string RegistrationMethod(string? rootNamespace) =>
        $$"""
        {{( rootNamespace is null ? "" : $"namespace {rootNamespace};" )}}

        internal static class OptionaryConfigurationOptions
        {
            internal static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddOptionaryConfigurationOptions(
                this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services) => services;
        }
        """;

    private static MetadataReference Stub(string assemblyName)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(OptionaryStub)],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
            new(OutputKind.DynamicallyLinkedLibrary)
        );
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        return !result.Success
            ? throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics.Select(z => z.ToString())))
            : (MetadataReference)MetadataReference.CreateFromImage(stream.ToArray());
    }

    private GeneratorTestContextBuilder Joints(MetadataReference optionary) => Builder
                                                                              .AddConfigurationOptionsReferences()
                                                                              .AddReferences(optionary)
                                                                              .WithDiagnosticSeverity(DiagnosticSeverity.Warning);
}
