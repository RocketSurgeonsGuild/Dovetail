using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Covers the managed-configuration opt-in in <c>src/Joinery.Dovetail.Sdk/Sdk/Joinery.Dovetail.HostDetection.targets</c> and
/// <c>Joinery.Dovetail.Configuration.targets</c>: configuration stays off by default (see <see cref="HostTypeTests"/>), and is turned
/// on by either <c>DovetailEnableConfiguration=true</c> or an explicit format flag, which injects that format's provider.
/// </summary>
[TargetFrameworks]
public class ConfigurationOptInTests(string targetFramework)
{
    private static readonly string[] Properties = ["DovetailEnableConfiguration"];
    private static readonly string[] Items = ["PackageReference"];

    private static bool ConfigurationProvidersOnly(string itemType, string identity) =>
        identity.StartsWith("Joinery.Dovetail.Configuration.", StringComparison.Ordinal);

    [Test]
    [Arguments("DovetailEnableConfiguration")]
    [Arguments("DovetailEnableYamlConfiguration")]
    [Arguments("DovetailEnableJsonConfiguration")]
    [Arguments("DovetailEnableTomlConfiguration")]
    public async Task OptInProperty_EnablesConfigurationAndInjectsProvider(string optInProperty)
    {
        using var project = CreateLibrary(new() { [optInProperty] = "true" });

        await project.VerifyEvaluation("lib", Properties, Items, ConfigurationProvidersOnly);
    }

    [Test]
    public async Task ExplicitlyDisabled_WinsOverFormatFlag()
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "false", ["DovetailEnableYamlConfiguration"] = "true" });

        await project.VerifyEvaluation("lib", Properties, Items, ConfigurationProvidersOnly);
    }

    [Test]
    public async Task NoGitIgnore_WarnsAboutLocalOverrides()
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "true" });

        await project.VerifyBuildWarnings("lib");
    }

    /// <summary>
    /// Git applies every .gitignore up to the repository root, so a root entry has to silence the warning even when the
    /// project directory has its own .gitignore without one.
    /// </summary>
    [Test]
    public async Task RootGitIgnoreEntry_SilencesWarningDespiteProjectGitIgnore()
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "true" });
        project
           .AddFile(".gitignore", "appsettings.local.*\n")
           .AddFile("lib/.gitignore", "bin/\nobj/\n");

        await project.VerifyBuildWarnings("lib");
    }

    /// <summary>
    /// A local override named after the assembly (<c>lib.local.yaml</c>) isn't ignored by <c>appsettings.local.*</c>, so it is warned
    /// about on its own, by name.
    /// </summary>
    [Test]
    public async Task NamedLocalFile_NotIgnored_WarnsAboutIt()
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "true" });
        project
           .AddFile(".gitignore", "appsettings.local.*\n")
           .AddFile("lib/lib.yaml", "Greeting: lib")
           .AddFile("lib/lib.local.yaml", "Greeting: local");

        await project.VerifyBuildWarnings("lib");
    }

    /// <summary>
    /// An entry for the named file, or a <c>*.local.*</c> wildcard (which covers <c>appsettings.local.*</c> as well), silences both
    /// warnings.
    /// </summary>
    [Test]
    [Arguments("appsettings.local.*\nlib.local.*\n")]
    [Arguments("*.local.*\n")]
    public async Task NamedLocalFile_Ignored_DoesNotWarn(string gitIgnore)
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "true" });
        project
           .AddFile(".gitignore", gitIgnore)
           .AddFile("lib/lib.yaml", "Greeting: lib")
           .AddFile("lib/lib.local.yaml", "Greeting: local");

        await project.VerifyBuildWarnings("lib");
    }

    /// <summary>
    /// With only <c>appsettings.local.yaml</c>, an <c>appsettings.local.*</c> entry is still all it takes: nothing is said about a
    /// <c>lib.local.*</c> file that doesn't exist.
    /// </summary>
    [Test]
    public async Task OnlyAppSettingsLocalFile_IgnoredByItsEntry_DoesNotWarn()
    {
        using var project = CreateLibrary(new() { ["DovetailEnableConfiguration"] = "true" });
        project
           .AddFile(".gitignore", "appsettings.local.*\n")
           .AddFile("lib/appsettings.yaml", "Greeting: appsettings")
           .AddFile("lib/appsettings.local.yaml", "Greeting: local");

        await project.VerifyBuildWarnings("lib");
    }

    private SdkTestProject CreateLibrary(Dictionary<string, string> properties)
    {
        var library = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        foreach (var (name, value) in properties)
        {
            library.Property(name, value);
        }

        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("lib/lib.csproj", library);
        return project;
    }
}
