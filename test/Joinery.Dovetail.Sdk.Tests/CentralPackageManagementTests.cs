using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// A repository that manages its package versions centrally (<c>ManagePackageVersionsCentrally</c>) pins the Dovetail packages in
/// <c>Directory.Packages.props</c>, as LaunchPad does. NuGet rejects a <c>PackageVersion</c> for a package whose reference is implicitly defined
/// (NU1009) and a <c>Version</c> on any other <c>PackageReference</c> (NU1008), so the packages the SDK adds have to take the version the
/// repository pins instead of carrying the SDK's own.
/// </summary>
[TargetFrameworks]
public class CentralPackageManagementTests(string targetFramework)
{
    [Test]
    public async Task PackagePinnedByTheRepository_IsReferencedWithoutAVersion_AndRestores()
    {
        using var project = new SdkTestProject();
        var hostingVersion = Config.DovetailPackageVersion("Joinery.Dovetail.Hosting");
        project
           .AddFile(
                "Directory.Packages.props",
                $"""
                <Project>
                  <PropertyGroup>
                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                  </PropertyGroup>
                  <ItemGroup>
                    <PackageVersion Include="Joinery.Dovetail.Hosting" Version="{hostingVersion}" />
                  </ItemGroup>
                </Project>
                """
            )
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
            )
           .AddFile("worker/Program.cs", "System.Console.WriteLine(\"hello\");");

        // EvaluateItems restores first, which is where NU1009 is raised.
        var references = await project.EvaluateItems("worker", "PackageReference");

        var hosting = references.Single(z => z["Identity"] == "Joinery.Dovetail.Hosting");
        await hosting["Version"].Should().BeEmpty();
        await hosting.ContainsKey("IsImplicitlyDefined").Should().BeFalse();
    }

    [Test]
    public async Task PackageNotPinnedByTheRepository_KeepsTheVersionOfTheSdk_AndRestores()
    {
        using var project = new SdkTestProject();
        project
           .AddFile(
                "Directory.Packages.props",
                """
                <Project>
                  <PropertyGroup>
                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                  </PropertyGroup>
                </Project>
                """
            )
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
            )
           .AddFile("worker/Program.cs", "System.Console.WriteLine(\"hello\");");

        var references = await project.EvaluateItems("worker", "PackageReference");

        var hosting = references.Single(z => z["Identity"] == "Joinery.Dovetail.Hosting");
        await hosting["Version"].Should().BeEqualTo(Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"));
        await hosting["IsImplicitlyDefined"].Should().BeEqualTo("true");
    }
}
