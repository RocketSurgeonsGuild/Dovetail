using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The generated <c>Imports</c> used to pin <c>DovetailHostType.UnitTest</c> for every project, which beat the hosting
/// extensions' <c>AddIfMissing(DovetailHostType.Live)</c>, so real applications ran UnitTest-only joints and skipped
/// Live-only ones. These run an application and read the host type Dovetail records in its configuration.
/// </summary>
[TargetFrameworks]
public class HostTypeRuntimeTests(string targetFramework)
{
    [Test]
    [Arguments(null, "Live")]
    [Arguments("UnitTest", "UnitTest")]
    public async Task Application_RunsWithTheExpectedHostType(string? dovetailHostType, string expected)
    {
        var app = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe");
        if (dovetailHostType is not null)
        {
            app.Property("DovetailHostType", dovetailHostType);
        }

        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("app/app.csproj", app)
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using Microsoft.Extensions.Configuration;
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.Hosting;

                var builder = Host.CreateApplicationBuilder(args);
                using var host = await builder.ConfigureDovetail();
                System.Console.WriteLine("HostType=" + host.Services.GetRequiredService<IConfiguration>()["RocketSurgeryDovetails:HostType"]);
                """
            );

        var output = await project.Run("app");

        output.ShouldContain($"HostType={expected}");
    }
}
