using System.Text.Json;
using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The name <c>Joinery.Dovetail.PackConfiguration.targets</c> gives a project's configuration files: <c>appsettings.{ext}</c> is copied,
/// packed and embedded as <c>{Name}.{ext}</c> (an environment or <c>local</c> qualifier kept), and the generated configuration joint loads
/// <c>{Name}.{ext}</c>. The name is the project's assembly name: the <c>ApplicationName</c> the runtime conventions look for (the entry
/// assembly's name), unique among the assemblies that share an output directory, and, for a file-based app (<c>Build.cs</c>, evaluated as a
/// virtual <c>Build.cs.csproj</c>), its file name without the <c>.cs</c>. These evaluate the projects rather than build them; FileBasedAppTests
/// runs file-based apps end to end.
/// </summary>
[TargetFrameworks]
public class ConfigurationNameTests(string targetFramework)
{
    [Test]
    [Arguments("lib.csproj", null, null, "lib")]
    [Arguments("Contoso.Library.csproj", null, null, "Contoso.Library")]
    [Arguments("lib.csproj", "AssemblyName", "Contoso.Library", "Contoso.Library")]
    [Arguments("lib.csproj", "RootNamespace", "Contoso.Library", "lib")]
    public async Task Project_NamesItsConfigurationAfterItsAssembly(string projectFile, string? property, string? value, string expectedName)
    {
        var library = ProjectCreator
                     .Templates.SdkCsproj(targetFramework: targetFramework)
                     .Property("DovetailEnableYamlConfiguration", "true");
        if (property is not null)
        {
            library.Property(property, value);
        }

        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject($"lib/{projectFile}", library);
        AddAppSettings(project, "lib");

        await VerifyConfigurationName(project, "lib", expectedName);
    }

    [Test]
    [Arguments("Build.cs", null, true, "Build")]
    [Arguments("my.tool.cs", null, true, "my.tool")]
    [Arguments("Build.cs", "Contoso.Build", true, "Contoso.Build")]
    [Arguments("my.tool.cs", null, false, "my.tool")]
    public async Task FileBasedApp_NamesItsConfigurationAfterItsAssembly(string fileName, string? assemblyName, bool dovetailSdk, string expectedName)
    {
        var directives = dovetailSdk
            ? "#:sdk Joinery.Dovetail.Sdk"
            : $"""
               #:package Joinery.Dovetail.Hosting@{Config.DovetailPackageVersion("Joinery.Dovetail.Hosting")}
               #:package Joinery.Dovetail.Configuration.Yaml@{Config.DovetailPackageVersion("Joinery.Dovetail.Configuration.Yaml")}
               """;
        using var project = new SdkTestProject();
        project.AddFile(
            $"app/{fileName}",
            $"""
            {directives}
            #:property TargetFramework={targetFramework}
            #:property DovetailEnableYamlConfiguration=true
            {( assemblyName is null ? "" : $"#:property AssemblyName={assemblyName}" )}

            System.Console.WriteLine("hello");
            """
        );
        AddAppSettings(project, "app");

        await VerifyConfigurationName(project, $"app/{fileName}", expectedName);
    }

    private static void AddAppSettings(SdkTestProject project, string directory) =>
        project
           .AddFile($"{directory}/appsettings.yaml", "Greeting: hello")
           .AddFile($"{directory}/appsettings.Development.yaml", "Greeting: development")
           .AddFile($"{directory}/appsettings.local.yaml", "Greeting: local");

    /// <summary>
    /// Checks every place the name shows up: where each file is copied to and packed under, the name it is embedded under, and the files
    /// the generated joint loads, which have to be the copied ones.
    /// </summary>
    private static async Task VerifyConfigurationName(SdkTestProject project, string relativePath, string name)
    {
        var result = await project.Evaluate(
            relativePath,
            "-t:_DovetailGenerateYamlConfiguration -getProperty:MSBuildProjectDirectory -getProperty:IntermediateOutputPath -getItem:None -getItem:EmbeddedResource"
        );

        var none = Items(result, "None");
        none["appsettings.yaml"]["TargetPath"].ShouldBe($"{name}.yaml");
        none["appsettings.yaml"]["PackagePath"].ShouldBe($"contentFiles/any/any/configuration/{name}.yaml");
        none["appsettings.Development.yaml"]["TargetPath"].ShouldBe($"{name}.Development.yaml");
        none["appsettings.Development.yaml"]["PackagePath"].ShouldBe($"contentFiles/any/any/configuration/{name}.Development.yaml");
        none["appsettings.local.yaml"]["TargetPath"].ShouldBe($"{name}.local.yaml");
        none["appsettings.local.yaml"]["Pack"].ShouldBe("false");

        Items(result, "EmbeddedResource")
           .Values
           .Where(z => z.GetValueOrDefault("DovetailConfiguration") == "true")
           .Select(z => z["LogicalName"])
           .Order(StringComparer.Ordinal)
           .ShouldBe([$"{name}.Development.yaml", $"{name}.yaml"]);

        var properties = result.GetProperty("Properties");
        var joint = await File.ReadAllTextAsync(
            Path.Combine(
                properties.GetProperty("MSBuildProjectDirectory").GetString()!,
                properties.GetProperty("IntermediateOutputPath").GetString()!,
                "Dovetail.YamlConfiguration.g.cs"
            )
        );
        joint.ShouldContain($"""builder.AddYamlFile("{name}.yaml", optional: false""");
        joint.ShouldContain($$"""builder.AddYamlFile($"{{name}}.{environmentName}.yaml", optional: true""");
        joint.ShouldContain($"""builder.AddYamlFile("{name}.local.yaml", optional: true""");
        joint.ShouldContain($"""DovetailEmbeddedConfiguration.OpenResource(assembly, "{name}.yaml", "appsettings.yaml")""");
    }

    /// <summary>The <paramref name="itemType"/> items, by file name, with their metadata.</summary>
    private static Dictionary<string, Dictionary<string, string>> Items(JsonElement result, string itemType) =>
        result
           .GetProperty("Items")
           .GetProperty(itemType)
           .EnumerateArray()
           .Where(z => z.GetProperty("Filename").GetString()!.StartsWith("appsettings", StringComparison.Ordinal))
           .ToDictionary(
                z => z.GetProperty("Filename").GetString() + z.GetProperty("Extension").GetString(),
                z => z.EnumerateObject().ToDictionary(m => m.Name, m => m.Value.GetString() ?? "")
            );
}
