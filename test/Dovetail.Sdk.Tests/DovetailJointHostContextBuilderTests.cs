using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the combination no single existing test exercises: a detected <c>DovetailHost</c> (via
/// Dovetail.HostDetection.targets) together with a custom <c>DovetailJoint</c> item, wired up by a real
/// Program.cs that calls the generated <c>ConfigureDovetail</c>/<c>Configure{Joint}</c> extension
/// methods. That forces Dovetail.Joints.targets, Dovetail.Hosts.targets and Dovetail.ContextBuilder.targets
/// (the last of which only emits <c>DovetailContextBuilder.g.cs</c> when a <c>DovetailHost</c> item is
/// present - see Dovetail.ContextBuilder.targets) to all run against the same project, and the
/// generated code has to actually compile against real call sites instead of just being declared.
/// </summary>
public class DovetailJointHostContextBuilderTests
{
    [Test]
    public async Task HostApplication_WithDovetailJoint_GeneratesHostJointAndContextBuilderTogether()
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
                "worker/worker.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: "net10.0", outputType: "Exe")
                   .ItemInclude(
                        "PackageReference",
                        include: "Microsoft.Extensions.Logging",
                        metadata: new Dictionary<string, string?> { ["Version"] = "10.0.9" }
                    )
                   .ItemInclude(
                        "DovetailJoint",
                        include: "CustomLogging",
                        metadata: new Dictionary<string, string?>
                        {
                            ["ParameterType"] = "global::Microsoft.Extensions.Logging.ILoggingBuilder",
                            ["ParameterName"] = "builder",
                            ["Namespace"] = "Dovetail.Joints"
                        }
                    )
            )
           .AddFile(
                "worker/Program.cs",
                """
                using Microsoft.Extensions.Hosting;
                using Dovetail.Joints;
                using worker;

                var builder = Host.CreateApplicationBuilder(args);
                await builder.ConfigureDovetail(static contextBuilder => contextBuilder.ConfigureCustomLogging(static (context, logging) => { }));
                """
            );

        await project.VerifyProjects();
    }
}
