using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the actual .nupkg contents produced by <c>Dovetail.PackConfiguration.targets</c>'
/// rename/pack <c>&lt;None Update&gt;</c> items: whether the appsettings.{ext} file discovered on
/// disk is really present under <c>contentFiles/configuration/{ProjectName}.{ext}</c> inside the
/// package NuGet restores to consumers, not just in the MSBuild evaluation snapshot exercised by
/// <see cref="PackingTests"/>.
/// </summary>
public class PackageContentTests
{
    [Test]
    public async Task JsonConfigurationFile_IsPackagedUnderContentFilesConfiguration()
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
            )
           .AddFile("lib/appsettings.json", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task YamlConfigurationFile_IsPackagedUnderContentFilesConfiguration()
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
            )
           .AddFile("lib/appsettings.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task YmlConfigurationFile_IsPackagedUnderContentFilesConfiguration()
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
            )
           .AddFile("lib/appsettings.yml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task TomlConfigurationFile_IsPackagedUnderContentFilesConfiguration()
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
            )
           .AddFile("lib/appsettings.toml", "");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task LocalOverrideFile_IsExcludedFromPackageEvenThoughSiblingIsPacked()
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
            )
           .AddFile("lib/appsettings.yaml", "{}")
           .AddFile("lib/appsettings.local.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task ExplicitlyDisabled_ExcludesConfigurationFromPackageEvenWithAppSettingsFile()
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
                   .Property("DovetailPackConfiguration", "false")
            )
           .AddFile("lib/appsettings.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task DevelopmentEnvironmentQualifiedFile_IsPackagedWithQualifierPreserved()
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
            )
           .AddFile("lib/appsettings.yaml", "{}")
           .AddFile("lib/appsettings.Development.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task ProductionEnvironmentQualifiedFile_IsPackagedWithQualifierPreserved()
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
            )
           .AddFile("lib/appsettings.yaml", "{}")
           .AddFile("lib/appsettings.Production.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    /// <summary>
    /// <c>Dovetail.PackConfiguration.targets</c> is imported both by <c>Dovetail.Sdk</c> (via
    /// <c>Dovetail.HostDetection.targets</c>) and directly out of the plain <c>Dovetail</c> package's
    /// own <c>build/Dovetail.targets</c> (see the unconditional
    /// <c>&lt;Import Project="...Dovetail.PackConfiguration.targets" /&gt;</c> there) - so a library
    /// that only takes a <c>PackageReference</c> on <c>Dovetail</c>, without adopting
    /// <c>Dovetail.Sdk</c> at all, must still get its appsettings file renamed/packed correctly.
    /// </summary>
    [Test]
    public async Task PlainLibraryReferencingDovetailPackageDirectly_PacksConfigurationWithoutSdk()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: "net10.0")
                   .ItemInclude(
                        "PackageReference",
                        include: "Dovetail",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Dovetail") }
                    )
            )
           .AddFile("lib/appsettings.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task PlainLibraryReferencingDovetailPackageDirectly_ExcludesLocalOverrideFromPackage()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: "net10.0")
                   .ItemInclude(
                        "PackageReference",
                        include: "Dovetail",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Dovetail") }
                    )
            )
           .AddFile("lib/appsettings.yaml", "{}")
           .AddFile("lib/appsettings.local.yaml", "{}");

        await project.VerifyProjectsAndPackages();
    }
}
