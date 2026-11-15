using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The seam Optionary takes over: the configuration generator emits <c>OptionaryConfigurationOptions.AddOptionaryConfigurationOptions</c>
///     in the <c>RootNamespace</c> build property, and the joint generator exports a joint, listed first, that calls it. The joint
///     generator decides on its own whether there is anything to call, from the <c>[DovetailConfiguration]</c> declarations.
/// </summary>
public class OptionsRegistrationMethodTests() : GeneratorTest()
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

    [Test]
    public async Task Emits_the_registration_method_in_the_root_namespace_and_a_joint_that_calls_it()
    {
        // The exports class lives somewhere else on purpose: the method follows RootNamespace, not DovetailExportNamespace.
        var result = await Options()
                          .AddGlobalOption("build_property.RootNamespace", "Sample.App")
                          .AddGlobalOption("build_property.DovetailExportNamespace", "Sample.Exports")
                          .AddSources(OptionsClass)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Emits_the_registration_method_in_the_global_namespace_without_a_root_namespace()
    {
        var result = await Options().AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Emits_an_empty_registration_method_when_no_section_registers_options()
    {
        var result = await Options()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration(RegisterOptions = false)]
                               public class SchemaOnlyOptions
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
    public async Task Emits_an_empty_registration_method_when_every_declaration_is_rejected()
    {
        var result = await Options()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class RefOptions
                               {
                                   public RefOptions(ref int value) => Value = value;

                                   public int Value { get; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Exports_the_joint_for_an_assembly_declaration()
    {
        var result = await Options()
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
    public async Task Emits_neither_without_declarations()
    {
        var result = await Options()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .AddSources("namespace Sample; public class PlainOptions { public string? Value { get; set; } }")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Emits_neither_below_CSharp12()
    {
        var result = await Options()
                          .AddGlobalOption("build_property.RootNamespace", "Sample")
                          .WithLanguageVersion(LanguageVersion.CSharp11)
                          .AddSources(OptionsClass)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    private GeneratorTestContextBuilder Options() => Builder
                                                    .AddConfigurationOptionsReferences()
                                                    .AddReferences(typeof(IValidateOptions<>))
                                                    .WithDiagnosticSeverity(DiagnosticSeverity.Warning);
}
