using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the managed-configuration opt-in in <c>src/Dovetail.Sdk/Sdk/Dovetail.HostDetection.targets</c> and
/// <c>Dovetail.Configuration.targets</c>: configuration stays off by default (see <see cref="HostTypeTests"/>), and is turned
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
