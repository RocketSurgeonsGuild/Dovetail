using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// An Aspire AppHost is <c>Microsoft.NET.Sdk</c> with <c>Aspire.Hosting.AppHost</c> since Aspire 17. The SDK gives a project that
/// references Aspire <c>Joinery.Dovetail.Aspire</c> (the <c>DistributedApplicationBuilder</c> joint), and an AppHost the
/// <c>DistributedApplication</c> host in place of the generic host: <c>ConfigureDovetail</c> on
/// <c>DistributedApplication.CreateBuilder(args)</c>.
/// </summary>
[TargetFrameworks]
public class AspireDetectionTests(string targetFramework)
{
    /// <summary>
    /// The AppHost's own joint adds a resource: it is in the model of the application <c>ConfigureDovetail</c> builds. Nothing is run,
    /// so the test needs neither the Aspire CLI's orchestrator nor a container runtime.
    /// </summary>
    private const string AppHostProgram = """
        using apphost;
        using Aspire.Hosting;
        using Aspire.Hosting.ApplicationModel;
        using Joinery.Dovetail;
        using Joinery.Dovetail.Attributes;
        using Joinery.Dovetail.Joints;
        using Microsoft.Extensions.DependencyInjection;

        await using var app = await DistributedApplication.CreateBuilder(args).ConfigureDovetail();
        var model = app.Services.GetRequiredService<DistributedApplicationModel>();
        System.Console.WriteLine("Resources=" + string.Join(",", System.Linq.Enumerable.Select(model.Resources, z => z.Name)));

        [DovetailExport]
        sealed class CacheJoint : IDistributedApplicationBuilderJoint
        {
            public void Register(IDovetailContext context, IDistributedApplicationBuilder builder) => builder.AddContainer("cache", "redis");
        }
        """;

    [Test]
    public async Task AppHost_DetectsDistributedApplicationHostAndInjectsAspire()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "apphost/apphost.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .ItemPackageReference("Aspire.Hosting.AppHost", Config.CentralPackageVersion("Aspire.Hosting.AppHost", targetFramework))
            )
           .AddFile("apphost/Program.cs", AppHostProgram);

        await project.VerifyProjects();
    }

    [Test]
    public async Task AppHost_ConfigureDovetail_AppliesTheJointsThatAddResources()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "apphost/apphost.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   // dotnet run on an AppHost hands over to `aspire run`, which starts the orchestrator: run the program itself.
                   .Property("ASPIRE_SUPPRESS_CLI_RUN_HOOK", "true")
                   .ItemPackageReference("Aspire.Hosting.AppHost", Config.CentralPackageVersion("Aspire.Hosting.AppHost", targetFramework))
            )
           .AddFile("apphost/Program.cs", AppHostProgram);

        var output = await project.Run("apphost");

        await output.Should().Contain("Resources=cache");
    }

    /// <summary>
    /// A library that references Aspire.Hosting gets the joint, so it can export resources to an AppHost, but no host: it has no entry
    /// point.
    /// </summary>
    [Test]
    public async Task Library_ReferencingAspireHosting_InjectsAspireWithoutAHost()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .ItemPackageReference("Aspire.Hosting", Config.CentralPackageVersion("Aspire.Hosting", targetFramework))
            )
           .AddFile(
                "lib/CacheJoint.cs",
                """
                using Aspire.Hosting;
                using Joinery.Dovetail;
                using Joinery.Dovetail.Attributes;
                using Joinery.Dovetail.Joints;

                namespace lib;

                [DovetailExport]
                public sealed class CacheJoint : IDistributedApplicationBuilderJoint
                {
                    public void Register(IDovetailContext context, IDistributedApplicationBuilder builder) => builder.AddContainer("cache", "redis");
                }
                """
            );

        await project.VerifyProjects();
    }

    /// <summary><c>DovetailEnableAspire=false</c> leaves Joinery.Dovetail.Aspire out, and the AppHost is a generic host again.</summary>
    [Test]
    public async Task DovetailEnableAspireFalse_LeavesAspireOut()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "apphost/apphost.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .Property("DovetailEnableAspire", "false")
                   .ItemPackageReference("Aspire.Hosting.AppHost", Config.CentralPackageVersion("Aspire.Hosting.AppHost", targetFramework))
            )
           .AddFile("apphost/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.VerifyProjects();
    }
}
