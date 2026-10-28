using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Covers the implicit <c>using Joinery.Dovetail;</c> from <c>src/Joinery.Dovetail/buildTransitive/Joinery.Dovetail.targets</c>. NuGet imports a
/// package's buildTransitive/ assets instead of build/ when both exist, so the using has to come from there: for any project
/// that references Dovetail directly (including the reference Joinery.Dovetail.Sdk adds), but not for one that only gets Dovetail
/// transitively through another package.
/// </summary>
[TargetFrameworks]
public class ImplicitUsingTests(string targetFramework)
{
    [Test]
    public async Task SdkConsumer_GetsImplicitDovetailUsing()
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
                   .Property("ImplicitUsings", "enable")
            );

        ( await project.EvaluateItemIdentities("lib", "Using") ).ShouldContain("Joinery.Dovetail");
    }

    [Test]
    public async Task DirectPackageReference_GetsImplicitDovetailUsing()
    {
        using var project = new SdkTestProject();
        project.AddProject(
            "lib/lib.csproj",
            ProjectCreator
               .Templates.SdkCsproj(targetFramework: targetFramework)
               .Property("ImplicitUsings", "enable")
               .ItemInclude("PackageReference", include: "Joinery.Dovetail", metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Joinery.Dovetail") })
        );

        ( await project.EvaluateItemIdentities("lib", "Using") ).ShouldContain("Joinery.Dovetail");
    }

    [Test]
    public async Task TransitiveOnlyReference_DoesNotGetImplicitDovetailUsing()
    {
        using var project = new SdkTestProject();
        project.AddProject(
            "app/app.csproj",
            ProjectCreator
               .Templates.SdkCsproj(targetFramework: targetFramework)
               .Property("ImplicitUsings", "enable")
               .ItemInclude(
                    "PackageReference",
                    include: "Joinery.Dovetail.Hosting",
                    metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Joinery.Dovetail.Hosting") }
                )
        );

        ( await project.EvaluateItemIdentities("app", "Using") ).ShouldNotContain("Joinery.Dovetail");
    }
}
