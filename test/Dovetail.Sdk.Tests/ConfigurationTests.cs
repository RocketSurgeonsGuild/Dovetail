using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers <c>src/Dovetail.Sdk/Sdk/Dovetail.PackConfiguration.targets</c>: for each supported
/// <c>appsettings.{ext}</c> format, an existing file is renamed/packed/copied to
/// <c>$(MSBuildProjectName).{ext}</c> and - when the matching configuration provider assembly is
/// also referenced - a generated <c>IConfigurationJoint</c> source is emitted. Both effects are
/// driven purely by file presence + provider-reference detection, independent of the
/// (default-off) <c>DovetailEnable*Configuration</c>/<c>Dovetail.Configuration.targets</c> opt-in path.
/// </summary>
public class ConfigurationTests
{
    [Test]
    [Arguments("json", "Microsoft.Extensions.Configuration.Json", "10.0.9", "Dovetail.JsonConfiguration.g.cs")]
    [Arguments("yaml", "Dovetail.Configuration.Yaml", null, "Dovetail.YamlConfiguration.g.cs")]
    [Arguments("yml", "Dovetail.Configuration.Yaml", null, "Dovetail.YmlConfiguration.g.cs")]
    [Arguments("toml", "Dovetail.Configuration.Toml", null, "Dovetail.TomlConfiguration.g.cs")]
    public async Task AppSettingsFile_WithProviderReferenced_EmitsConfigurationJoint(
        string extension,
        string providerPackageId,
        string? providerVersion,
        string generatedFileName
    )
    {
        var version = providerVersion ?? Config.DovetailPackageVersion(providerPackageId);

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
                   .ItemInclude(
                        "PackageReference",
                        include: providerPackageId,
                        metadata: new Dictionary<string, string?> { ["Version"] = version }
                    )
            )
           .AddFile($"lib/appsettings.{extension}", "{}");

        await project.VerifyProjects();
    }

    /// <summary>
    /// Two independently-detected <c>appsettings.{ext}</c> providers in the same project don't
    /// interfere with each other: each still gets renamed/copied and gets its own generated
    /// <c>IConfigurationJoint</c>, side by side. Split into one method per pair (rather than
    /// [Arguments] rows) because Verify derives each snapshot's file name from the full parameter
    /// list, and doubling the parameters for a provider pair overflows the filesystem's file name
    /// length limit.
    /// </summary>
    [Test]
    public Task TwoAppSettingsFiles_JsonAndYaml_EmitBothConfigurationJoints() =>
        VerifyTwoProviders("json", "Microsoft.Extensions.Configuration.Json", "10.0.9", "yaml", "Dovetail.Configuration.Yaml", null);

    [Test]
    public Task TwoAppSettingsFiles_JsonAndToml_EmitBothConfigurationJoints() =>
        VerifyTwoProviders("json", "Microsoft.Extensions.Configuration.Json", "10.0.9", "toml", "Dovetail.Configuration.Toml", null);

    [Test]
    public Task TwoAppSettingsFiles_YamlAndToml_EmitBothConfigurationJoints() =>
        VerifyTwoProviders("yaml", "Dovetail.Configuration.Yaml", null, "toml", "Dovetail.Configuration.Toml", null);

    private static async Task VerifyTwoProviders(
        string extensionA,
        string providerPackageIdA,
        string? providerVersionA,
        string extensionB,
        string providerPackageIdB,
        string? providerVersionB
    )
    {
        var versionA = providerVersionA ?? Config.DovetailPackageVersion(providerPackageIdA);
        var versionB = providerVersionB ?? Config.DovetailPackageVersion(providerPackageIdB);

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
                   .ItemInclude(
                        "PackageReference",
                        include: providerPackageIdA,
                        metadata: new Dictionary<string, string?> { ["Version"] = versionA }
                    )
                   .ItemInclude(
                        "PackageReference",
                        include: providerPackageIdB,
                        metadata: new Dictionary<string, string?> { ["Version"] = versionB }
                    )
            )
           .AddFile($"lib/appsettings.{extensionA}", "{}")
           .AddFile($"lib/appsettings.{extensionB}", "{}");

        await project.VerifyProjects();
    }

    [Test]
    [Arguments("json")]
    [Arguments("yaml")]
    [Arguments("yml")]
    [Arguments("toml")]
    public async Task AppSettingsFile_WithoutProviderReferenced_NoConfigurationJointEmitted(string extension)
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
           .AddFile($"lib/appsettings.{extension}", "{}");

        await project.VerifyProjects();
    }
}
