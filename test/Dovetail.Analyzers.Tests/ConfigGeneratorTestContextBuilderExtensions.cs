using Dovetail.Configuration.Toml;
using Dovetail.Configuration.Yaml;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Dovetail.Analyzers.Tests;

/// <summary>
///     Mirrors `Rocket.Surgery.Dovetails.Analyzers.Tests.GeneratorTestContextBuilderExtensions`.
///     Adds references to `Dovetail` core (for `IConfigurationJoint` — now the interface implemented by
///     the hand-authored `JsonDovetail`/`YamlDovetail`/`TomlDovetail` runtime conventions,
///     not a generator-emitted type — and `IDovetailJoint`) and picks up every `IIncrementalGenerator`
///     in `Dovetail.Analyzers` for the surviving generator tests in this project (convention
///     export/import; the `dovetail-managed-configuration` config-discovery/type-inference/manifest
///     generator stages this class previously also wired up were removed in `1bd74928`).
/// </summary>
internal static class ConfigGeneratorTestContextBuilderExtensions
{
    public static GeneratorTestContextBuilder AddConfigCommonReferences(this GeneratorTestContextBuilder builder) => builder.AddReferences(
        typeof(IConfigurationJoint).Assembly,
        typeof(IDovetailJoint).Assembly,
        typeof(IServiceCollection).Assembly,
        typeof(IConfigurationBuilder).Assembly,
        typeof(OptionsServiceCollectionExtensions).Assembly,
        typeof(JsonConfigurationExtensions).Assembly,
        typeof(YamlConfigurationExtensions).Assembly,
        typeof(TomlConfigurationExtensions).Assembly,
        typeof(BinderOptions).Assembly
    );

    public static GeneratorTestContextBuilder AddConfigCommonGenerators(this GeneratorTestContextBuilder builder)
    {
        foreach (var generator in GetAllGenerators(typeof(ConfigGeneratorTestContextBuilderExtensions).Assembly.GetIndagoProvider()))
        {
            builder = builder.WithGenerator(generator);
        }

        return builder;
    }

    private static IEnumerable<Type> GetAllGenerators(IIndagoProvider provider) => provider.GetTypes(s => s
                                                                                              .FromAssemblyOf<DovetailAttributesGenerator>()
                                                                                              .GetTypes(f => f.WithAttribute<GeneratorAttribute>().AssignableTo<IIncrementalGenerator>())
        );
}
