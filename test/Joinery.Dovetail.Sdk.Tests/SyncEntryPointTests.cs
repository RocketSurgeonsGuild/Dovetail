using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Covers the internal <c>SyncEntryPoint</c> <c>DovetailHost</c> metadata (<c>src/Joinery.Dovetail/build/Joinery.Dovetail.Hosts.targets</c>):
/// a host that sets it also gets the blocking <c>UseDovetail</c> overloads from <c>DovetailHostSync.cs.template</c>, which
/// the scaffolded program calls so the build proves they compile. Hosts without it (every other <see cref="HostTypeTests" />
/// snapshot) get no <c>.Sync.g.cs</c> file.
/// </summary>
[TargetFrameworks]
public class SyncEntryPointTests(string targetFramework)
{
    [Test]
    public async Task SyncEntryPointHost_GeneratesUseDovetailOverloads()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "worker/worker.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   // The generic host's own package, which a HostApplicationBuilder application references (see HostingHelperTests).
                   .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                   .ItemInclude(
                        "DovetailHost",
                        include: "SyncHost",
                        metadata: new Dictionary<string, string?>
                        {
                            ["HostBuilderType"] = "global::Microsoft.Extensions.Hosting.HostApplicationBuilder",
                            ["HostBuilderName"] = "builder",
                            ["HostType"] = "global::Microsoft.Extensions.Hosting.IHost",
                            ["HelperName"] = "global::Joinery.Dovetail.DovetailHostApplicationHelpers",
                            ["SyncEntryPoint"] = "true",
                        }
                    )
            )
           .AddFile(
                "worker/Program.cs",
                """
                using worker;

                using var host = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args).UseDovetail();
                """
            );

        await project.VerifyProjects();
    }
}
