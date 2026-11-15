using Joinery.Dovetail.Support.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     <see cref="ConfigurationGenerator" /> on its own, without the joint generator: it describes and binds
///     <c>[DovetailConfiguration]</c> types by itself, and checks the language version by itself.
/// </summary>
public class ConfigurationGeneratorTests() : GeneratorTest()
{
    private const string OptionsClass = """
        using Joinery.Dovetail.Attributes;

        namespace Sample;

        [DovetailConfiguration]
        public class GreetingsOptions
        {
            public string Message { get; set; } = "Hello";
            public int Count { get; set; } = 1;
        }
        """;

    [Test]
    public async Task Generates_the_schema_and_options_without_the_joint_generator()
    {
        var result = await ConfigurationOnly().AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Generates_nothing_below_CSharp12_and_leaves_DT0006_to_the_joint_generator()
    {
        var result = await ConfigurationOnly()
                          .WithLanguageVersion(LanguageVersion.CSharp11)
                          .AddSources(OptionsClass)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    private static GeneratorTestContextBuilder ConfigurationOnly() => GeneratorTestContextBuilder
                                                                     .Create()
                                                                     .AddCommonReferences()
                                                                     .AddConfigurationOptionsReferences()
                                                                     .AddDovetailConfiguration("", "")
                                                                     .WithGenerator<ConfigurationGenerator>();
}
