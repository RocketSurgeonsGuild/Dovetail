using Joinery.Dovetail.Configuration.Json;
using Joinery.Dovetail.Configuration.Toml;
using Joinery.Dovetail.Configuration.Yaml;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Tests;

/// <summary>
///     Covers loading configuration from embedded resources (<see cref="DovetailEmbeddedConfiguration" />): the per-format embedded joints, the
///     disk joints standing down in that mode, and the generated per-project joint falling back to the copy the SDK embedded. The fixtures under
///     <c>embedded/</c> follow the conventional file names; the <c>Some.Root.*</c> ones are named the way other tools (e.g. Uno) name embedded
///     resources, and are only found through the suffix match.
/// </summary>
public sealed class EmbeddedConfigurationTests
{
    [Test]
    [MethodDataSource(nameof(EmbeddedJoints))]
    public async Task EmbeddedJoint_LoadsConventionalFiles_InPrecedenceOrder(IConfigurationJoint joint)
    {
        var configuration = await BuildAsync(joint, embedded: true);

        await configuration["Sample:Name"].Should().BeEqualTo("development");
        await configuration["Sample:Count"].Should().BeEqualTo("3");
    }

    [Test]
    [MethodDataSource(nameof(EmbeddedJoints))]
    public async Task EmbeddedJoint_WithoutEmbeddedAssembly_AddsNothing(IConfigurationJoint joint)
    {
        var builder = new ConfigurationBuilder();
        joint.Register(await CreateContextAsync(embedded: false), builder);

        await builder.Sources.Should().BeEmpty();
    }

    [Test]
    [MethodDataSource(nameof(DiskJoints))]
    public async Task DiskJoint_WithEmbeddedAssembly_AddsNothing(IConfigurationJoint joint)
    {
        var builder = new ConfigurationBuilder();
        joint.Register(await CreateContextAsync(embedded: true), builder);

        await builder.Sources.Should().BeEmpty();
    }

    [Test]
    public async Task GeneratedJoint_WithEmbeddedAssembly_ReadsTheEmbeddedProjectFile()
    {
        var configuration = await BuildAsync(new JsonAppSettingsConfiguration(), embedded: true);

        await configuration["Generated:Name"].Should().BeEqualTo("development");
    }

    [Test]
    public async Task GeneratedJoint_WithoutEmbeddedAssembly_ReadsTheCopiedProjectFile()
    {
        var configuration = await BuildAsync(new JsonAppSettingsConfiguration(), embedded: false, environmentName: "Production");

        await configuration["Generated:Name"].Should().BeEqualTo("project");
    }

    [Test]
    public async Task OpenResource_PrefersEarlierNames_AndMatchesBySuffix()
    {
        var assembly = typeof(EmbeddedConfigurationTests).Assembly;

        using var exact = DovetailEmbeddedConfiguration.OpenResource(assembly, "Missing.json", "appsettings.json");
        await exact.Should().NotBeNull();
        using var suffix = DovetailEmbeddedConfiguration.OpenResource(assembly, "appsettings.local.json");
        await suffix.Should().NotBeNull();
        await DovetailEmbeddedConfiguration.OpenResource(assembly, "settings.json").Should().BeNull();
    }

    [Test]
    public async Task GetConventionalFileNames_OrdersByPrecedence()
    {
        var names = DovetailEmbeddedConfiguration.GetConventionalFileNames(await CreateContextAsync(embedded: false), "yaml", "yml");

        await names.Should().BeEquivalentTo([
                "appsettings.yaml", "appsettings.yml", "Sample.yaml", "Sample.yml",
                "appsettings.Development.yaml", "appsettings.Development.yml", "Sample.Development.yaml", "Sample.Development.yml",
                "appsettings.local.yaml", "appsettings.local.yml", "Sample.local.yaml", "Sample.local.yml",
            ], CollectionOrdering.Matching);
    }

    public static IEnumerable<Func<IConfigurationJoint>> EmbeddedJoints()
    {
        yield return () => new JsonEmbeddedJoint();
        yield return () => new YamlEmbeddedJoint();
        yield return () => new TomlEmbeddedJoint();
    }

    public static IEnumerable<Func<IConfigurationJoint>> DiskJoints()
    {
        yield return () => new JsonJoint();
        yield return () => new YamlJoint();
        yield return () => new TomlJoint();
    }

    private static async Task<IConfigurationRoot> BuildAsync(IConfigurationJoint joint, bool embedded, string environmentName = "Development")
    {
        var builder = new ConfigurationBuilder();
        joint.Register(await CreateContextAsync(embedded, environmentName), builder);
        return builder.Build();
    }

    private static async Task<IDovetailContext> CreateContextAsync(bool embedded, string environmentName = "Development")
    {
        var contextBuilder = DovetailContextBuilder
                            .Create([], new Dictionary<object, object>(), [])
                            .Set("ApplicationName", "Sample")
                            .Set("EnvironmentName", environmentName);
        if (embedded)
        {
            contextBuilder.Set(DovetailEmbeddedConfiguration.AssemblyKey, typeof(EmbeddedConfigurationTests).Assembly);
        }

        return await DovetailContext.FromAsync(contextBuilder);
    }
}
