using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the transitive-<c>ProjectReference</c> copy path: two library projects each ship their
/// own appsettings.{ext} (renamed to <c>{ProjectName}.{ext}</c> by
/// <c>Dovetail.PackConfiguration.targets</c>), and a host application (<c>OutputType=Exe</c>)
/// references both. MSBuild's own <c>GetCopyToOutputDirectoryItems</c> machinery walks
/// <c>ProjectReference</c> edges and copies each referenced project's
/// <c>CopyToOutputDirectory</c> items down into the referencing project's own output, so both
/// libraries' renamed config files should land in the host's build output - and, separately, in
/// its publish output too.
/// </summary>
public class HostConfigurationPropagationTests
{
    private static SdkTestProject CreateHostWithTwoConfiguredLibraries()
    {
        var project = new SdkTestProject();
        project.AddProject(
            "Directory.Build.props",
            ProjectCreator
               .Create("Directory.Build.props")
               .Sdk("Dovetail.Sdk")
        );

        // track: false - these are meant to be built only transitively, through the host's own
        // <ProjectReference>. Building them standalone here first would leave lib1/lib2 "up to
        // date" by the time host's build runs, which skips regenerating their reference assemblies
        // (obj/.../ref/{name}.dll) and fails host's compile with CS0006.
        var lib1 = ProjectCreator.Templates.SdkCsproj(targetFramework: "net10.0");
        project.AddProject("lib1/lib1.csproj", lib1, track: false).AddFile("lib1/appsettings.yaml", "{}");

        var lib2 = ProjectCreator.Templates.SdkCsproj(targetFramework: "net10.0");
        project.AddProject("lib2/lib2.csproj", lib2, track: false).AddFile("lib2/appsettings.json", "{}");

        var host = ProjectCreator
           .Templates.SdkCsproj(targetFramework: "net10.0", outputType: "Exe")
           .ItemProjectReference(lib1)
           .ItemProjectReference(lib2);
        project.AddProject("host/host.csproj", host).AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        return project;
    }

    [Test]
    public async Task HostReferencingTwoConfiguredLibraries_CopiesBothConfigurationFilesToBuildOutput()
    {
        using var project = CreateHostWithTwoConfiguredLibraries();

        await project.VerifyProjects();
    }

    [Test]
    public async Task HostReferencingTwoConfiguredLibraries_CopiesBothConfigurationFilesToPublishOutput()
    {
        using var project = CreateHostWithTwoConfiguredLibraries();

        await project.VerifyPublish("host");
    }

    /// <summary>
    /// Same two-libraries-plus-host shape as <see cref="CreateHostWithTwoConfiguredLibraries"/>,
    /// but lib1/lib2 are packed first and consumed by the host via a genuine
    /// <c>&lt;PackageReference&gt;</c> restored from this scaffold's own local NuGet feed (see
    /// <see cref="SdkTestProject.PackToLocalFeed"/>), instead of a <c>&lt;ProjectReference&gt;</c>.
    /// This exercises NuGet's own <c>contentFiles</c> restore convention rather than MSBuild's
    /// <c>GetCopyToOutputDirectoryItems</c> project-graph walk - the two paths are driven by
    /// different machinery and aren't guaranteed to behave identically.
    /// </summary>
    private static async Task<SdkTestProject> CreateHostConsumingPackagedLibraries()
    {
        var project = new SdkTestProject();
        project.AddProject(
            "Directory.Build.props",
            ProjectCreator
               .Create("Directory.Build.props")
               .Sdk("Dovetail.Sdk")
        );

        // Package IDs deliberately distinctive (not "lib1"/"lib2"): NuGet restore consults every
        // registered source, including nuget.org, and a generic id can collide with an unrelated
        // real public package there - which silently wins if this scaffold's own local feed isn't
        // consulted first, masking the package this test actually built and meant to verify.
        project
           .AddProject("lib1/DovetailPkgTestLib1.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: "net10.0"))
           .AddFile("lib1/appsettings.yaml", "{}");

        project
           .AddProject("lib2/DovetailPkgTestLib2.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: "net10.0"))
           .AddFile("lib2/appsettings.json", "{}");

        var lib1Package = await project.PackToLocalFeed("lib1");
        var lib2Package = await project.PackToLocalFeed("lib2");

        var host = ProjectCreator
           .Templates.SdkCsproj(targetFramework: "net10.0", outputType: "Exe")
           .ItemInclude("PackageReference", include: lib1Package.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = lib1Package.Version })
           .ItemInclude("PackageReference", include: lib2Package.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = lib2Package.Version });
        project.AddProject("host/host.csproj", host).AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        return project;
    }

    [Test]
    public async Task HostReferencingTwoPackagedLibraries_CopiesBothConfigurationFilesToBuildOutput()
    {
        using var project = await CreateHostConsumingPackagedLibraries();

        await project.VerifyProjects();
    }

    [Test]
    public async Task HostReferencingTwoPackagedLibraries_CopiesBothConfigurationFilesToPublishOutput()
    {
        using var project = await CreateHostConsumingPackagedLibraries();

        await project.VerifyPublish("host");
    }
}
