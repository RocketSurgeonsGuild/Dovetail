using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

public class SdkTests
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
                   .Templates.SdkCsproj(targetFramework: "net10.0")
                   .ItemInclude(
                        "PackageReference",
                        include: "Microsoft.Extensions.Logging",
                        metadata: new Dictionary<string, string?> { ["Version"] = "10.0.9" }
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
