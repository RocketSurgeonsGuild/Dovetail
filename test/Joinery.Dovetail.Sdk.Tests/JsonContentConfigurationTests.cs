using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The Web and Worker SDKs glob <c>*.json</c> as <c>Content</c> rather than <c>None</c>. <c>Joinery.Dovetail.PackConfiguration.targets</c>
/// must rename those items too: the generated JSON configuration joint loads <c>{ProjectName}.json</c> (not optional), so an
/// app whose <c>appsettings.json</c> kept its original name failed at startup with a FileNotFoundException.
/// </summary>
[TargetFrameworks]
public class JsonContentConfigurationTests(string targetFramework)
{
    private SdkTestProject CreateApplicationWithJsonAppSettings(string sdk)
    {
        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "app/app.csproj",
                ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, sdk: sdk)
            )
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("app/appsettings.json", "{}")
           .AddFile("app/appsettings.Development.json", "{}");
        return project;
    }

    [Test]
    [Arguments("Microsoft.NET.Sdk.Web")]
    [Arguments("Microsoft.NET.Sdk.Worker")]
    public async Task JsonAppSettings_AreCopiedUnderProjectName(string sdk)
    {
        using var project = CreateApplicationWithJsonAppSettings(sdk);

        await project.VerifyProjects();
    }

    [Test]
    [Arguments("Microsoft.NET.Sdk.Web")]
    [Arguments("Microsoft.NET.Sdk.Worker")]
    public async Task JsonAppSettings_ArePublishedUnderProjectName(string sdk)
    {
        using var project = CreateApplicationWithJsonAppSettings(sdk);

        await project.VerifyPublish("app");
    }

    /// <summary>
    /// A file-based web app (<c>#:sdk Microsoft.NET.Sdk.Web</c>) gets its JSON files as <c>Content</c> too, while the packages include its
    /// other configuration files as <c>None</c> (it has no default <c>None</c> items): each JSON file stays a single item, renamed after the
    /// assembly (<c>Build</c>, not <c>Build.cs</c>), which is the file the generated joint loads.
    /// </summary>
    [Test]
    public async Task FileBasedWebApp_JsonAppSettings_AreCopiedUnderItsAssemblyName()
    {
        using var project = new SdkTestProject();
        project
           .AddFile(
                "app/Build.cs",
                $"""
                #:sdk Microsoft.NET.Sdk.Web
                #:package Joinery.Dovetail@{Config.DovetailPackageVersion("Joinery.Dovetail")}
                #:property TargetFramework={targetFramework}

                System.Console.WriteLine("hello");
                """
            )
           .AddFile("app/appsettings.json", "{}")
           .AddFile("app/appsettings.Development.json", "{}");

        var result = await project.Evaluate(
            "app/Build.cs",
            "-t:_DovetailGenerateJsonConfiguration -getProperty:MSBuildProjectDirectory -getProperty:IntermediateOutputPath -getItem:None -getItem:Content"
        );

        AppSettings(result, "None").ShouldBeEmpty();
        AppSettings(result, "Content").ShouldBe(
            [
                ("appsettings.Development.json", "Build.Development.json", "PreserveNewest"),
                ("appsettings.json", "Build.json", "PreserveNewest"),
            ]
        );
        var properties = result.GetProperty("Properties");
        var joint = await File.ReadAllTextAsync(
            Path.Combine(
                properties.GetProperty("MSBuildProjectDirectory").GetString()!,
                properties.GetProperty("IntermediateOutputPath").GetString()!,
                "Dovetail.JsonConfiguration.g.cs"
            )
        );
        joint.ShouldContain("""builder.AddJsonFile(FileName(files, "Build.json", "appsettings.json"), optional: false""");
        joint.ShouldContain("""builder.AddJsonFile(FileName(files, $"Build.{environmentName}.json", $"appsettings.{environmentName}.json"), optional: true""");
    }

    /// <summary>The <c>appsettings*</c> items of <paramref name="itemType"/>: file name, target path and copy setting.</summary>
    private static (string File, string? TargetPath, string? CopyToOutputDirectory)[] AppSettings(System.Text.Json.JsonElement result, string itemType) =>
        result.GetProperty("Items").TryGetProperty(itemType, out var items)
            ? [.. items
                 .EnumerateArray()
                 .Select(
                      z => (
                          File: z.GetProperty("Filename").GetString() + z.GetProperty("Extension").GetString(),
                          TargetPath: z.TryGetProperty("TargetPath", out var targetPath) ? targetPath.GetString() : null,
                          CopyToOutputDirectory: z.TryGetProperty("CopyToOutputDirectory", out var copy) ? copy.GetString() : null
                      )
                  )
                 .Where(z => z.File.StartsWith("appsettings", StringComparison.Ordinal))
                 .OrderBy(z => z.File, StringComparer.Ordinal)]
            : [];
}
