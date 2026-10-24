using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// The Web and Worker SDKs glob <c>*.json</c> as <c>Content</c> rather than <c>None</c>. <c>Dovetail.PackConfiguration.targets</c>
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
                   .Create("Directory.Build.props")
                   .Sdk("Dovetail.Sdk")
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
}
