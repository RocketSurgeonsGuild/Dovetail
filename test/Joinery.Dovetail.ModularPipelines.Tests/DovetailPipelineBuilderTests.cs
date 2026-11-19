using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

namespace Joinery.Dovetail.ModularPipelines.Tests;

public class DovetailPipelineBuilderTests
{
    [Test]
    public async Task ConfigureDovetail_BuildsThePipeline_WithTheModulesAndServicesOfTheExportedJoints()
    {
        await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail();

        var plan = await pipeline.PlanAsync();
        await plan.Modules.OfType<RecordedModule>().Count().Should().BeEqualTo(1);
        await pipeline.Services.GetService<PipelineRecorder>().Should().NotBeNull();
    }

    [Test]
    public async Task ConfigureDovetail_RunsTheModulesTheJointsAdded()
    {
        // The test runner has no terminal, whose zero width Spectre.Console refuses when the pipeline writes its output.
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureConsole(z => z with { Width = 120, ShowProgress = false, PrintLogo = false, PrintResults = false, })
                                        .ConfigureDovetail();

        await pipeline.RunAsync();

        await pipeline.Services.GetRequiredService<PipelineRecorder>().Ran.Should().BeEquivalentTo([nameof(RecordedModule)], CollectionOrdering.Matching);
    }

    [Test]
    public async Task ConfigureDovetail_HandsThePipelineBuilderJointsTheBuilder()
    {
        var joint = Mock.OfDelegate<PipelineBuilderJointDelegate>();
        var builder = Pipeline.CreateBuilder();

        await using var pipeline = await builder.ConfigureDovetail(z => z.ConfigurePipelineBuilder(joint.Object));

        joint.Invoke(Any(), Is(builder)).WasCalled(Times.Once);
    }

    [Test]
    public async Task ConfigureDovetail_AppliesPipelineBuilderJointsInPriorityOrder_SyncAndAsync()
    {
        var order = new List<string>();
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z
                                                 .ConfigurePipelineBuilder(
                                                      async (_, builder, cancellationToken) =>
                                                      {
                                                          await Task.Yield();
                                                          order.Add("async");
                                                          builder.AddModule<DelegateModule>();
                                                      },
                                                      priority: 10
                                                  )
                                                 .ConfigurePipelineBuilder((_, _) => order.Add("sync"), priority: -10)
                                         );

        await order.Should().BeEquivalentTo(["sync", "async"], CollectionOrdering.Matching);
        var plan = await pipeline.PlanAsync();
        await plan.Modules.OfType<DelegateModule>().Count().Should().BeEqualTo(1);
    }

    [Test]
    public async Task ConfigureDovetail_AddsTheConfigurationJointsToThePipelineConfiguration()
    {
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z.ConfigureConfiguration(
                                                 (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Pipeline"] = "joined", })
                                             )
                                         );

        await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:Pipeline"].Should().BeEqualTo("joined");
    }

    /// <summary>
    ///     ModularPipelines registers its IConfiguration only when it builds the pipeline, after the joints ran. A service joint that
    ///     TryAdds one of its own, as OpenTelemetry's <c>AddOpenTelemetry()</c> does (reading the environment only), must not take the
    ///     place of the pipeline configuration, which holds the sources of the configuration joints.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_KeepsThePipelineConfiguration_WhenAServiceJointTryAddsAnIConfiguration()
    {
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z
                                                 .ConfigureConfiguration(
                                                      (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Pipeline"] = "joined", })
                                                  )
                                                 .ConfigureServices(
                                                      (_, services) => services.TryAddSingleton<IConfiguration>(
                                                          _ => new ConfigurationBuilder().AddEnvironmentVariables().Build()
                                                      )
                                                  )
                                         );

        await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:Pipeline"].Should().BeEqualTo("joined");
    }

    /// <summary>
    ///     The joint Joinery.Dovetail.OpenTelemetry brings an application calls <c>AddOpenTelemetry()</c>.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_KeepsThePipelineConfiguration_WhenAServiceJointAddsOpenTelemetry()
    {
        await using var pipeline = await Pipeline
                                        .CreateBuilder()
                                        .ConfigureDovetail(
                                             z => z
                                                 .ConfigureConfiguration(
                                                      (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Pipeline"] = "joined", })
                                                  )
                                                 .ConfigureServices((_, services) => services.AddOpenTelemetry())
                                         );

        await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:Pipeline"].Should().BeEqualTo("joined");
    }

    [Test]
    public async Task ConfigureDovetail_ResolvesTheFilesOfTheConfigurationJoints_AgainstTheBasePathOfThePipelineConfiguration()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "file" } }""");
            var builder = Pipeline.CreateBuilder();
            builder.Configuration.SetBasePath(directory.FullName);

            await using var pipeline = await builder.ConfigureDovetail(
                z => z.ConfigureConfiguration((_, configuration) => configuration.AddJsonFile("appsettings.json", optional: false))
            );

            await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].Should().BeEqualTo("file");
        }
        finally
        {
            directory.Delete(true);
        }
    }

    [Test]
    public async Task ConfigureDovetail_KeepsTheCommandLineAboveTheFilesOfTheConfigurationJoints_ReadThroughTheFileProviderOfThePipelineConfiguration()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(directory.FullName, "appsettings.json"),
                """{ "Dovetail": { "FromFile": "file", "Overridden": "file" } }"""
            );
            var builder = Pipeline.CreateBuilder(["--Dovetail:Overridden=commandline"]);
            using var fileProvider = new PhysicalFileProvider(directory.FullName);
            builder.Configuration.SetFileProvider(fileProvider);

            await using var pipeline = await builder.ConfigureDovetail(
                z => z.ConfigureConfiguration((_, configuration) => configuration.AddJsonFile("appsettings.json", optional: false))
            );

            var configuration = pipeline.Services.GetRequiredService<IConfiguration>();
            await configuration["Dovetail:FromFile"].Should().BeEqualTo("file");
            await configuration["Dovetail:Overridden"].Should().BeEqualTo("commandline");
        }
        finally
        {
            directory.Delete(true);
        }
    }

    /// <summary>
    ///     A file-based app (<c>dotnet run build/Build.cs</c>) keeps its configuration files next to its entry point file, which the .NET SDK
    ///     names in the <c>EntryPointFileDirectoryPath</c> AppContext data; the pipeline's default file provider reads the current directory.
    /// </summary>
    [Test]
    [TUnit.Core.NotInParallel] // EntryPointFileDirectoryPath is process-wide, so no other pipeline may be built meanwhile.
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheDirectoryOfTheEntryPointFile()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "entry-point" } }""");
            using var _ = EntryPointFileDirectoryPath(directory.FullName);

            await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail(
                z => z.ConfigureConfiguration((_, configuration) => configuration.AddJsonFile("appsettings.json", optional: false))
            );

            await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].Should().BeEqualTo("entry-point");
        }
        finally
        {
            directory.Delete(true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel] // EntryPointFileDirectoryPath is process-wide, so no other pipeline may be built meanwhile.
    public async Task ConfigureDovetail_KeepsTheBasePathOfThePipelineConfiguration_InAFileBasedApp()
    {
        var entryPointDirectory = Directory.CreateTempSubdirectory();
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(entryPointDirectory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "entry-point" } }""");
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "base-path" } }""");
            using var _ = EntryPointFileDirectoryPath(entryPointDirectory.FullName);
            var builder = Pipeline.CreateBuilder();
            builder.Configuration.SetBasePath(directory.FullName);

            await using var pipeline = await builder.ConfigureDovetail(
                z => z.ConfigureConfiguration((_, configuration) => configuration.AddJsonFile("appsettings.json", optional: false))
            );

            await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].Should().BeEqualTo("base-path");
        }
        finally
        {
            directory.Delete(true);
            entryPointDirectory.Delete(true);
        }
    }

    /// <summary>
    ///     The SDK writes the directory into the runtimeconfig.json, so a published or copied file-based app still names the source directory
    ///     of the machine it was built on. Where that doesn't exist, the joints keep the pipeline's own file provider.
    /// </summary>
    [Test]
    [TUnit.Core.NotInParallel] // EntryPointFileDirectoryPath is process-wide, so no other pipeline may be built meanwhile.
    public async Task ConfigureDovetail_Builds_WhenTheDirectoryOfTheEntryPointFileDoesNotExist()
    {
        using var _ = EntryPointFileDirectoryPath(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail();

        await pipeline.Services.GetRequiredService<IConfiguration>().Should().NotBeNull();
    }

    /// <summary>
    ///     <see cref="DovetailSettings.ConfigurationDirectory" /> wins over the directory of the entry point file and over a base path
    ///     set on the pipeline configuration.
    /// </summary>
    [Test]
    [TUnit.Core.NotInParallel] // EntryPointFileDirectoryPath is process-wide, so no other pipeline may be built meanwhile.
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheConfigurationDirectorySetting()
    {
        var entryPointDirectory = Directory.CreateTempSubdirectory();
        var basePathDirectory = Directory.CreateTempSubdirectory();
        var configurationDirectory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(entryPointDirectory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "entry-point" } }""");
            await File.WriteAllTextAsync(Path.Combine(basePathDirectory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "base-path" } }""");
            await File.WriteAllTextAsync(Path.Combine(configurationDirectory.FullName, "appsettings.json"), """{ "Dovetail": { "FromFile": "setting" } }""");
            using var _ = EntryPointFileDirectoryPath(entryPointDirectory.FullName);
            var builder = Pipeline.CreateBuilder();
            builder.Configuration.SetBasePath(basePathDirectory.FullName);
            builder.Configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { [DovetailSettings.ConfigurationDirectory] = configurationDirectory.FullName, }
            );

            await using var pipeline = await builder.ConfigureDovetail(
                z => z.ConfigureConfiguration((_, configuration) => configuration.AddJsonFile("appsettings.json", optional: false))
            );

            await pipeline.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].Should().BeEqualTo("setting");
        }
        finally
        {
            configurationDirectory.Delete(true);
            basePathDirectory.Delete(true);
            entryPointDirectory.Delete(true);
        }
    }

    // What the .NET SDK sets for a file-based app; removed again on dispose, since it's process-wide.
    private static IDisposable EntryPointFileDirectoryPath(string path)
    {
        AppContext.SetData("EntryPointFileDirectoryPath", path);
        return new Reset();
    }

    private sealed class Reset : IDisposable
    {
        public void Dispose() => AppContext.SetData("EntryPointFileDirectoryPath", null);
    }

    [Test]
    public async Task ConfigureDovetail_HandsTheHostCreatedJointsThePipeline()
    {
        var joint = Mock.OfDelegate<HostCreatedJointDelegate<IPipeline>>();

        await using var pipeline = await Pipeline.CreateBuilder().ConfigureDovetail(z => z.ConfigureHostCreated(joint.Object));

        joint.Invoke(Any(), Is(pipeline)).WasCalled(Times.Once);
    }

    [Test]
    public async Task DovetailPipelinePlugin_AppliesTheJoints_WhenItIsAdded()
    {
        var plugin = new DovetailPipelinePlugin(DovetailContextBuilder.Create());
        var builder = Pipeline.CreateBuilder().AddPlugin(plugin);

        await plugin.Context.Should().NotBeNull();
        await plugin.Context.Get<PipelineBuilder>().Should().BeSameReferenceAs(builder);
        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        await plan.Modules.OfType<RecordedModule>().Count().Should().BeEqualTo(1);
    }
}
