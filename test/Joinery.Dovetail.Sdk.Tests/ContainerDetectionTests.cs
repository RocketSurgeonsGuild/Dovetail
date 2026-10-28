using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Covers the flag-gated DI container references in <c>src/Joinery.Dovetail.Sdk/Sdk/Joinery.Dovetail.HostDetection.targets</c>: once
/// <c>DovetailEnable{Container}</c> is set, referencing either the container itself or its
/// Microsoft.Extensions.DependencyInjection adapter package injects the matching Dovetail integration package.
/// </summary>
[TargetFrameworks]
public class ContainerDetectionTests(string targetFramework)
{
    [Test]
    [Arguments("DovetailEnableAutofac", "Autofac.Extensions.DependencyInjection", null)]
    [Arguments("DovetailEnableDryIoc", "DryIoc.Microsoft.DependencyInjection", null)]
    [Arguments("DovetailEnableStashbox", "Stashbox", "5.21.0")]
    [Arguments("DovetailEnableStashbox", "Stashbox.Extensions.DependencyInjection", null)]
    [Arguments("DovetailEnableStashbox", "Stashbox.Extensions.Hosting", "6.0.0")]
    public async Task ContainerReferenced_WhenEnabled_InjectsIntegrationPackage(string enableProperty, string containerPackageId, string? containerVersion)
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
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .Property(enableProperty, "true")
                   .ItemInclude(
                        "PackageReference",
                        include: containerPackageId,
                        metadata: new Dictionary<string, string?> { ["Version"] = containerVersion ?? Config.CentralPackageVersion(containerPackageId, targetFramework) }
                    )
            );

        await project.VerifyProjects();
    }

    [Test]
    public async Task ContainerReferenced_WhenNotEnabled_DoesNotInjectIntegrationPackage()
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
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .ItemInclude(
                        "PackageReference",
                        include: "Stashbox.Extensions.DependencyInjection",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.CentralPackageVersion("Stashbox.Extensions.DependencyInjection", targetFramework) }
                    )
            );

        await project.VerifyProjects();
    }
}
