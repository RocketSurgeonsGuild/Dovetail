using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the embedding half of <c>src/Dovetail/build/Dovetail.PackConfiguration.targets</c>: every project's
/// <c>appsettings.{ext}</c> files are also embedded into its assembly under their renamed names (so hosts without a file
/// system can read them), <c>appsettings.local.{ext}</c> only with <c>DovetailEmbedConfiguration=true</c>, which also stops
/// the files being copied to the output directory.
/// </summary>
[TargetFrameworks]
public class EmbeddedConfigurationTests(string targetFramework)
{
    [Test]
    public async Task Library_EmbedsConfiguration_ExceptLocal_AndStillCopiesIt()
    {
        using var project = CreateLibrary(embedOnly: false);

        await project.VerifyEmbeddedConfiguration("lib");
    }

    [Test]
    public async Task EmbedConfigurationEnabled_EmbedsLocalToo_AndCopiesNothing()
    {
        using var project = CreateLibrary(embedOnly: true);

        await project.VerifyEmbeddedConfiguration("lib");
    }

    /// <summary>
    /// Uno.Sdk embeds <c>appsettings.json</c> itself, under the default manifest name and from targets imported after
    /// Dovetail's; Dovetail then leaves that file to it rather than embedding it twice.
    /// </summary>
    [Test]
    public async Task FileAlreadyEmbeddedByAnotherTool_IsNotEmbeddedTwice()
    {
        using var project = CreateLibrary(embedOnly: false, embedAppSettingsJson: true);

        await project.VerifyEmbeddedConfiguration("lib");
    }

    private SdkTestProject CreateLibrary(bool embedOnly, bool embedAppSettingsJson = false)
    {
        var library = ProjectCreator
                     .Templates.SdkCsproj(targetFramework: targetFramework)
                     .ItemInclude(
                          "PackageReference",
                          include: "Microsoft.Extensions.Configuration.Json",
                          metadata: new Dictionary<string, string?> { ["Version"] = "10.0.9" }
                      );
        if (embedOnly)
        {
            library.Property("DovetailEmbedConfiguration", "true");
        }

        if (embedAppSettingsJson)
        {
            library.ItemInclude("EmbeddedResource", "appsettings.json");
        }

        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create("Directory.Build.props")
                   .Sdk("Dovetail.Sdk")
            )
           .AddProject("lib/lib.csproj", library)
           .AddFile("lib/appsettings.json", "{}")
           .AddFile("lib/appsettings.Production.json", "{}")
           .AddFile("lib/appsettings.local.json", "{}");
        return project;
    }
}
