using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
///     Every project that references the generic host gets its own, self-contained copy of <c>OnHostStarting</c>, <c>OnHostStarted</c>,
///     <c>OnHostStopping</c> and <c>OnHostStopped</c> and of the hosted service behind them (Joinery.Dovetail.Hosting emits them from
///     <c>DovetailHostLifecycleHelpers.cs.template</c>, as internal types), and nothing is shared between the copies. These run an
///     application and a library that each register handlers.
/// </summary>
[TargetFrameworks]
public class HostLifecycleRuntimeTests(string targetFramework)
{
    [Test]
    public async Task LibraryAndApplication_EachRunTheirOwnHandlers_ThroughTheirOwnHostedService()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests).
        var lib = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework)
                 .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"))
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework));
        project
           .AddProject("lib/lib.csproj", lib, track: false)
           .AddFile(
                "lib/LibraryLifecycle.cs",
                """
                using Joinery.Dovetail;
                using Joinery.Dovetail.Attributes;
                using Joinery.Dovetail.Joints;
                using Microsoft.Extensions.DependencyInjection;

                namespace lib;

                [DovetailExport]
                public sealed class LibraryLifecycle : IServiceJoint
                {
                    public void Register(IDovetailContext context, IServiceCollection services) =>
                        services
                           .OnHostStarted(_ => System.Console.WriteLine("lib started"))
                           .OnHostStopped(_ => System.Console.WriteLine("lib stopped"));
                }
                """
            );

        // The generic host's own package, which a HostApplicationBuilder application references (see HostingHelperTests).
        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                 .ItemProjectReference(lib);
        project
           .AddProject("app/app.csproj", app)
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using System.Linq;
                using Joinery.Dovetail;
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.Hosting;

                var builder = Host.CreateApplicationBuilder(args);
                builder
                   .OnHostStarted(_ => System.Console.WriteLine("app started"))
                   .OnHostStopped(_ => System.Console.WriteLine("app stopped"));
                using var host = await builder.ConfigureDovetail();
                await host.StartAsync();
                await host.StopAsync();
                System.Console.WriteLine("lifecycle services=" + host.Services.GetServices<IHostedService>().Count(service => service.GetType().Name == "DovetailHostLifecycleService"));
                """
            );

        var lines = ( await project.Run("app") ).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Each project's handlers run once, through that project's own hosted service: two services, not one shared by both.
        lines.Count(line => line == "app started").ShouldBe(1);
        lines.Count(line => line == "app stopped").ShouldBe(1);
        lines.Count(line => line == "lib started").ShouldBe(1);
        lines.Count(line => line == "lib stopped").ShouldBe(1);
        lines.ShouldContain("lifecycle services=2");
    }
}
