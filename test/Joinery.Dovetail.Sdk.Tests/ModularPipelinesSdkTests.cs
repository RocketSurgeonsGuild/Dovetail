using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// <c>Joinery.Dovetail.Sdk.ModularPipelines</c> chains the Dovetail SDK and adds ModularPipelines, so a build pipeline names one SDK and
/// nothing else: it gets Joinery.Dovetail.ModularPipelines, the ModularPipelines host (<c>ConfigureDovetail</c> on <c>PipelineBuilder</c>),
/// its helper (emitted by Joinery.Dovetail.Hosting) and ModularPipelines.OpenTelemetry with the joint that turns it on. These run
/// pipelines: what they print is only right when the pipeline was built from the application's joints and configuration.
/// </summary>
[TargetFrameworks]
public class ModularPipelinesSdkTests(string targetFramework)
{
    /// <summary>
    /// What a pipeline prints: the configuration the modules see, where the helper came from, and whether ModularPipelines'
    /// OpenTelemetry instrumentation was registered (its <c>AddOpenTelemetry()</c> registers <c>OpenTelemetryInitializer</c>).
    /// </summary>
    private const string PrintPipeline = """
        System.Console.WriteLine("Greeting=" + pipeline.Services.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>()["Greeting"]);
        System.Console.WriteLine("Helpers=" + typeof(Joinery.Dovetail.DovetailPipelineHelpers).Assembly.GetName().Name);
        var initializer = System.Type.GetType("ModularPipelines.OpenTelemetry.OpenTelemetryInitializer, ModularPipelines.OpenTelemetry");
        System.Console.WriteLine(
            "ModularPipelinesOpenTelemetry="
          + ( initializer is not null && pipeline.Services.GetRequiredService<IServiceProviderIsService>().IsService(initializer) )
        );
        """;

    private const string NoOpModule = """
        sealed class NoOpModule : Module<bool>
        {
            protected override System.Threading.Tasks.Task<bool> ExecuteAsync(IModuleContext context, System.Threading.CancellationToken cancellationToken) =>
                System.Threading.Tasks.Task.FromResult(true);
        }
        """;

    /// <summary>
    /// A file-based build script names the SDK and nothing else: no <c>#:package</c> directive, not even ModularPipelines. With the
    /// generic host's OpenTelemetry joint on too (a file-based app gets Joinery.Dovetail.OpenTelemetry), the modules still see
    /// <c>appsettings.yaml</c>.
    /// </summary>
    [Test]
    public async Task FileBasedBuildScript_NeedsOnlyTheSdk()
    {
        Skip.When(targetFramework != "net11.0", "ModularPipelines 4 targets net11.0 only.");
        using var project = new SdkTestProject();
        project.AddFile(
            "build/Build.cs",
            $$"""
            #:sdk Joinery.Dovetail.Sdk.ModularPipelines
            #:property TargetFramework={{targetFramework}}
            #:property DovetailEnableYamlConfiguration=true

            using Build;
            using Microsoft.Extensions.DependencyInjection;

            // A pipeline needs a module to build.
            var builder = Pipeline.CreateBuilder(args);
            builder.AddModule<NoOpModule>();
            await using var pipeline = await builder.ConfigureDovetail();
            {{PrintPipeline}}

            {{NoOpModule}}
            """
        );
        project.AddFile("build/appsettings.yaml", "Greeting: from-appsettings");

        var output = await project.RunFile("build/Build.cs", workingDirectory: ".");

        await output.Should().Contain("Greeting=from-appsettings");
        await output.Should().Contain("Helpers=Build");
        await output.Should().Contain("ModularPipelinesOpenTelemetry=True");
    }

    /// <summary>
    /// <c>&lt;Project Sdk="Joinery.Dovetail.Sdk.ModularPipelines"&gt;</c> (here once for every project, from a Directory.Build.props): the
    /// project needs no OutputType, as the SDK makes it an application, and no ModularPipelines reference. A module a joint adds runs.
    /// </summary>
    [Test]
    public async Task ProjectSdk_RunsTheModulesOfTheApplicationsJoints()
    {
        Skip.When(targetFramework != "net11.0", "ModularPipelines 4 targets net11.0 only.");
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk.ModularPipelines"))
           .AddProject("app/app.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile(
                "app/Program.cs",
                $$"""
                using app;
                using Joinery.Dovetail;
                using Joinery.Dovetail.Attributes;
                using Joinery.Dovetail.Joints;
                using Microsoft.Extensions.DependencyInjection;

                await using var pipeline = await Pipeline.CreateBuilder(args).ConfigureDovetail();
                var plan = await pipeline.PlanAsync();
                System.Console.WriteLine("Modules=" + string.Join(",", System.Linq.Enumerable.Select(plan.Modules, z => z.GetType().Name)));
                {{PrintPipeline}}

                [DovetailExport]
                sealed class AddModuleJoint : IPipelineBuilderJoint
                {
                    public void Register(IDovetailContext context, PipelineBuilder builder) => builder.AddModule<NoOpModule>();
                }

                {{NoOpModule}}
                """
            );

        var output = await project.Run("app");

        await output.Should().Contain("Modules=NoOpModule");
        await output.Should().Contain("Helpers=app");
        await output.Should().Contain("ModularPipelinesOpenTelemetry=True");
    }

    /// <summary>
    /// <c>DovetailEnableOpenTelemetry=false</c> leaves ModularPipelines.OpenTelemetry out, and with it the joint that turns it on.
    /// </summary>
    [Test]
    public async Task DovetailEnableOpenTelemetryFalse_LeavesModularPipelinesOpenTelemetryOut()
    {
        Skip.When(targetFramework != "net11.0", "ModularPipelines 4 targets net11.0 only.");
        using var project = new SdkTestProject();
        project.AddFile(
            "build/Build.cs",
            $$"""
            #:sdk Joinery.Dovetail.Sdk.ModularPipelines
            #:property TargetFramework={{targetFramework}}
            #:property DovetailEnableOpenTelemetry=false

            using Build;
            using Microsoft.Extensions.DependencyInjection;

            var builder = Pipeline.CreateBuilder(args);
            builder.AddModule<NoOpModule>();
            await using var pipeline = await builder.ConfigureDovetail();
            {{PrintPipeline}}

            {{NoOpModule}}
            """
        );

        var output = await project.RunFile("build/Build.cs", workingDirectory: ".");

        await output.Should().Contain("ModularPipelinesOpenTelemetry=False");
    }
}
