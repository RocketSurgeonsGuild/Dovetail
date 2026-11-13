using System.Text.Json;
using Microsoft.Build.Utilities.ProjectCreation;
using TUnit.Core.Exceptions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The name <c>Joinery.Dovetail.PackConfiguration.targets</c> gives a project's configuration files: <c>appsettings.{ext}</c> is copied,
/// packed and embedded as <c>{Name}.{ext}</c> (an environment or <c>local</c> qualifier kept), and the generated configuration joint loads
/// <c>{Name}.{ext}</c>. The name is the project's assembly name: the <c>ApplicationName</c> the runtime conventions look for (the entry
/// assembly's name), unique among the assemblies that share an output directory, and, for a file-based app (<c>Build.cs</c>, evaluated as a
/// virtual <c>Build.cs.csproj</c>), its file name without the <c>.cs</c>. A source file already named <c>{Name}.{ext}</c> (environment and
/// <c>local</c> variants included) is treated the same way and keeps its name. These evaluate the projects rather than build them, except
/// the collision checks; FileBasedAppTests runs file-based apps end to end.
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

    /// <summary>
    /// A source file already named after the assembly (<c>lib/lib.yaml</c>, or <c>Contoso.Library.yaml</c> in a <c>lib.csproj</c> whose
    /// assembly is <c>Contoso.Library</c>) is copied, packed and embedded under its own name, its environment and <c>local</c> variants too,
    /// and the generated joint loads it, exactly as it would a renamed <c>appsettings.yaml</c>.
    /// </summary>
    [Test]
    [Arguments(null, "lib")]
    [Arguments("Contoso.Library", "Contoso.Library")]
    public async Task Project_UsesAConfigurationFileNamedAfterItsAssembly(string? assemblyName, string name)
    {
        var library = ProjectCreator
                     .Templates.SdkCsproj(targetFramework: targetFramework)
                     .Property("DovetailEnableYamlConfiguration", "true");
        if (assemblyName is not null)
        {
            library.Property("AssemblyName", assemblyName);
        }

        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", library);
        AddAppSettings(project, "lib", name);

        await VerifyConfigurationName(project, "lib", name, name);
    }

    /// <summary>
    /// A file-based app's configuration file named after its assembly (<c>Build.yaml</c> next to <c>Build.cs</c>), with or without the
    /// Dovetail SDK.
    /// </summary>
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task FileBasedApp_UsesAConfigurationFileNamedAfterItsAssembly(bool dovetailSdk)
    {
        var directives = dovetailSdk
            ? "#:sdk Joinery.Dovetail.Sdk"
            : $"""
               #:package Joinery.Dovetail.Hosting@{Config.DovetailPackageVersion("Joinery.Dovetail.Hosting")}
               #:package Joinery.Dovetail.Configuration.Yaml@{Config.DovetailPackageVersion("Joinery.Dovetail.Configuration.Yaml")}
               """;
        using var project = new SdkTestProject();
        project.AddFile(
            "app/Build.cs",
            $"""
            {directives}
            #:property TargetFramework={targetFramework}
            #:property DovetailEnableYamlConfiguration=true

            System.Console.WriteLine("hello");
            """
        );
        AddAppSettings(project, "app", "Build");

        await VerifyConfigurationName(project, "app/Build.cs", "Build", "Build");
    }

    /// <summary>
    /// The name is the assembly's, so a file named after the project file of a project whose assembly is named otherwise (<c>lib.yaml</c>
    /// in a <c>lib.csproj</c> building <c>Contoso.Library</c>) is not configuration: it would be copied as <c>lib.yaml</c>, which neither
    /// the generated joint nor the runtime conventions load. It is left alone, like any other file.
    /// </summary>
    [Test]
    public async Task Project_IgnoresAFileNamedAfterTheProjectFileButNotTheAssembly()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .Property("DovetailEnableYamlConfiguration", "true")
                   .Property("AssemblyName", "Contoso.Library")
            );
        AddAppSettings(project, "lib", "lib");

        var result = await project.Evaluate(
            "lib",
            "-t:_DovetailGenerateYamlConfiguration -getProperty:MSBuildProjectDirectory -getProperty:IntermediateOutputPath -getItem:None -getItem:EmbeddedResource"
        );

        var none = Items(result, "None", "lib");
        none.Keys.Order(StringComparer.Ordinal).ShouldBe(["lib.Development.yaml", "lib.local.yaml", "lib.yaml"]);
        none.Values.ShouldAllBe(z => !z.ContainsKey("TargetPath") || z["TargetPath"] == "");
        none.Values.ShouldAllBe(z => !z.ContainsKey("PackagePath") || z["PackagePath"] == "");
        Items(result, "EmbeddedResource", "lib").ShouldBeEmpty();
        var properties = result.GetProperty("Properties");
        File.Exists(
                Path.Combine(
                    properties.GetProperty("MSBuildProjectDirectory").GetString()!,
                    properties.GetProperty("IntermediateOutputPath").GetString()!,
                    "Dovetail.YamlConfiguration.g.cs"
                )
            )
           .ShouldBeFalse();
    }

    /// <summary>
    /// <c>appsettings{suffix}.{ext}</c> and <c>{Name}{suffix}.{ext}</c> would be copied, packed and embedded under the same name, one
    /// silently replacing the other, so the build fails with <c>DT0019</c> naming both.
    /// </summary>
    [Test]
    [Arguments("lib.yaml", "lib.yaml", "appsettings.yaml")]
    [Arguments("lib.Development.yaml", "lib.Development.yaml", "appsettings.Development.yaml")]
    public async Task BothSourceNamesForTheSameFile_FailTheBuild(string file, string collidingFile, string collidingAppSettings)
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib/appsettings.yaml", "Greeting: appsettings")
           .AddFile("lib/appsettings.Development.yaml", "Greeting: appsettings")
           .AddFile("lib/lib.yaml", "Greeting: lib")
           .AddFile($"lib/{file}", "Greeting: lib");

        var exception = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("lib"));

        exception.Message.ShouldContain(
            $"error DT0019: Dovetail: '{collidingFile}' and '{collidingAppSettings}' are both copied, packed and embedded as '{collidingFile}'."
        );
    }

    /// <summary>
    /// Files that don't land on the same path coexist: a different suffix (<c>lib.Development.yaml</c> is not included without
    /// <c>lib.yaml</c>, and wouldn't collide with <c>appsettings.yaml</c> anyway), or a different extension (<c>.yaml</c> and <c>.yml</c>).
    /// </summary>
    [Test]
    [Arguments("lib.Development.yaml")]
    [Arguments("lib.yml")]
    public async Task SourceNamesForDifferentFiles_Build(string file)
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib/appsettings.yaml", "Greeting: appsettings")
           .AddFile($"lib/{file}", "Greeting: lib");

        var build = await project.BuildProject("lib");

        build.Succeeded.ShouldBeTrue();
    }

    private static void AddAppSettings(SdkTestProject project, string directory, string source = "appsettings") =>
        project
           .AddFile($"{directory}/{source}.yaml", "Greeting: hello")
           .AddFile($"{directory}/{source}.Development.yaml", "Greeting: development")
           .AddFile($"{directory}/{source}.local.yaml", "Greeting: local");

    /// <summary>
    /// Checks every place the name shows up: where each file is copied to and packed under, the name it is embedded under, and the files
    /// the generated joint loads, which have to be the copied ones.
    /// </summary>
    private static async Task VerifyConfigurationName(SdkTestProject project, string relativePath, string name, string source = "appsettings")
    {
        var result = await project.Evaluate(
            relativePath,
            "-t:_DovetailGenerateYamlConfiguration -getProperty:MSBuildProjectDirectory -getProperty:IntermediateOutputPath -getItem:None -getItem:EmbeddedResource"
        );

        var none = Items(result, "None", source);
        none.Keys.Order(StringComparer.Ordinal).ShouldBe([$"{source}.Development.yaml", $"{source}.local.yaml", $"{source}.yaml"]);
        none[$"{source}.yaml"]["TargetPath"].ShouldBe($"{name}.yaml");
        none[$"{source}.yaml"]["PackagePath"].ShouldBe($"contentFiles/any/any/configuration/{name}.yaml");
        none[$"{source}.Development.yaml"]["TargetPath"].ShouldBe($"{name}.Development.yaml");
        none[$"{source}.Development.yaml"]["PackagePath"].ShouldBe($"contentFiles/any/any/configuration/{name}.Development.yaml");
        none[$"{source}.local.yaml"]["TargetPath"].ShouldBe($"{name}.local.yaml");
        none[$"{source}.local.yaml"]["Pack"].ShouldBe("false");

        Items(result, "EmbeddedResource", source)
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
        joint.ShouldContain($"""builder.AddYamlFile(FileName(files, "{name}.yaml", "appsettings.yaml"), optional: false""");
        joint.ShouldContain($$"""builder.AddYamlFile(FileName(files, $"{{name}}.{environmentName}.yaml", $"appsettings.{environmentName}.yaml"), optional: true""");
        joint.ShouldContain($"""builder.AddYamlFile(FileName(files, "{name}.local.yaml", "appsettings.local.yaml"), optional: true""");
        joint.ShouldContain($"""DovetailEmbeddedConfiguration.OpenResource(assembly, "{name}.yaml", "appsettings.yaml")""");
    }

    /// <summary>The <paramref name="itemType"/> items whose file name starts with <paramref name="source"/>, by file name, with their metadata.</summary>
    private static Dictionary<string, Dictionary<string, string>> Items(JsonElement result, string itemType, string source) =>
        result
           .GetProperty("Items")
           .TryGetProperty(itemType, out var items)
            ? items
               .EnumerateArray()
               .Where(z => z.GetProperty("Filename").GetString()!.StartsWith(source, StringComparison.Ordinal))
               .ToDictionary(
                    z => z.GetProperty("Filename").GetString() + z.GetProperty("Extension").GetString(),
                    z => z.EnumerateObject().ToDictionary(m => m.Name, m => m.Value.GetString() ?? "")
                )
            : [];
}
