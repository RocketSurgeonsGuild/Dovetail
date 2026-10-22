using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

[TargetFrameworks]
public class SdkTests(string targetFramework)
{
    [Test]
    public async Task BaseSdk_AppliesSharedDefaults_AndInjectsAnalyzers()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create("Directory.Build.props")
                   .Sdk("Dovetail.Sdk")
            )
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .ItemInclude(
                        "PackageReference",
                        include: "Microsoft.Extensions.Logging",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.CentralPackageVersion("Microsoft.Extensions.Logging", targetFramework) }
                    )
                   .ItemInclude(
                        "DovetailJoint",
                        include: "Logging",
                        metadata: new Dictionary<string, string?>
                        {
                            ["ParameterType"] = "global::Microsoft.Extensions.Logging.ILoggingBuilder",
                            ["ParameterName"] = "builder"
                        }
                    )
            );

        await project.VerifyProjects();
    }
}
