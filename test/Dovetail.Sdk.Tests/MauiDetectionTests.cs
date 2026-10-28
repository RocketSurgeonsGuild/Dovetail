using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the MAUI branch of <c>src/Dovetail.Sdk/Sdk/Dovetail.HostDetection.targets</c>. A MAUI app (<c>UseMaui</c> with
/// <c>OutputType=Exe</c>) gets the MAUI host - not the generic host its <c>Exe</c> output type would otherwise pick - with the
/// blocking entry points, embed-only configuration and a build-time environment name; a MAUI class library gets no host.
/// These projects need the MAUI workload to build, so they're only evaluated.
/// </summary>
[TargetFrameworks]
public class MauiDetectionTests(string targetFramework)
{
    private static readonly string[] Properties = ["_DovetailApplicationDetected", "DovetailEmbedConfiguration", "ImportDovetailAssembly", "ExportDovetailAssembly"];
    private static readonly string[] Items = ["DovetailHost", "PackageReference", "AssemblyMetadata"];

    // With the MAUI workload installed, the MAUI SDK adds its own implicit package references (at the workload's version), so
    // keep only Dovetail's.
    private static bool DovetailPackagesOnly(string itemType, string identity) =>
        itemType != "PackageReference" || identity.StartsWith("Joinery.Dovetail", StringComparison.Ordinal);

    [Test]
    public async Task MauiApp_DetectsMauiHostAndInjectsHostingMaui()
    {
        using var project = CreateProject(outputType: "Exe");

        await project.VerifyEvaluation("app", Properties, Items, DovetailPackagesOnly);
    }

    [Test]
    public async Task MauiApp_DovetailEnvironmentName_OverridesTheBuildTimeEnvironment()
    {
        using var project = CreateProject(outputType: "Exe", environmentName: "Staging");

        await project.VerifyEvaluation("app", Properties, ["AssemblyMetadata"], DovetailPackagesOnly);
    }

    [Test]
    public async Task MauiLibrary_NoHostDetected()
    {
        using var project = CreateProject(outputType: null);

        await project.VerifyEvaluation("app", Properties, Items, DovetailPackagesOnly);
    }

    private SdkTestProject CreateProject(string? outputType, string? environmentName = null)
    {
        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: outputType)
                 .Property("UseMaui", "true");
        if (environmentName is not null)
        {
            app.Property("DovetailEnvironmentName", environmentName);
        }

        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("app/app.csproj", app);
        return project;
    }
}
