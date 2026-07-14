using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers <c>DovetailPackConfiguration</c> (see <c>Dovetail.HostDetection.targets</c>): whether
/// <c>Dovetail.PackConfiguration.targets</c> - the appsettings rename/pack/copy + codegen surface
/// exercised by <see cref="ConfigurationTests"/> - is imported at all for a given project.
/// </summary>
public class PackingTests
{
    [Test]
    public async Task PlainLibrary_DefaultsDovetailPackConfigurationTrue()
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

        await project.VerifyProjects();
    }

    [Test]
    public async Task ExplicitlyDisabled_SkipsRenameAndCodegenEvenWithAppSettingsFile()
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
                   .ItemInclude(
                        "PackageReference",
                        include: "Dovetail.Configuration.Yaml",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Dovetail.Configuration.Yaml") }
                    )
            )
           .AddFile("lib/appsettings.yaml", "{}");

        await project.VerifyProjects();
    }

    [Test]
    public async Task WebApplication_ExplicitlyEnabled_StillAppliesRenameAndCodegen()
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
                "web/web.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: "net10.0", sdk: "Microsoft.NET.Sdk.Web")
                   .Property("DovetailPackConfiguration", "true")
                   .ItemInclude(
                        "PackageReference",
                        include: "Dovetail.Configuration.Yaml",
                        metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Dovetail.Configuration.Yaml") }
                    )
            )
           .AddFile("web/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("web/appsettings.yaml", "{}");

        await project.VerifyProjects();
    }
}
