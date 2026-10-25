using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers <c>Dovetail.ConfigurationSchema.EditorSettings.targets</c>: with <c>DovetailScaffoldEditorSettings=true</c>, building a
/// host maps its merged schema in <c>.vscode/settings.json</c>, touching nothing else in the file; without it nothing is written.
/// The editor itself is covered by the Dovetail.Build.Tasks tests.
/// </summary>
public class ConfigurationSchemaEditorSettingsTests
{
    private const string TargetFramework = "net10.0";
    private const string HostSchemaUrl = "\"./host/obj/appsettings.schema.json\"";

    private static SdkTestProject CreateHost(bool repository)
    {
        var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create("Directory.Build.props").Sdk("Dovetail.Sdk"))
           .AddProject("host/host.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: TargetFramework, outputType: "Exe"))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", ConfigurationSchemaTests.AppFragment);
        if (repository) project.AddFile(".git/HEAD", "ref: refs/heads/main\n");
        return project;
    }

    private static string RepositorySettings(SdkTestProject project) => Path.Combine(project.Directory, ".vscode", "settings.json");

    [Test]
    public async Task PropertyOff_WritesNothing()
    {
        using var project = CreateHost(repository: true);

        await project.BuildProject("host");

        File.Exists(Path.Combine(project.Directory, "host/obj/appsettings.schema.json")).ShouldBeTrue();
        Directory.Exists(Path.Combine(project.Directory, ".vscode")).ShouldBeFalse();
        Directory.Exists(Path.Combine(project.Directory, "host", ".vscode")).ShouldBeFalse();
    }

    [Test]
    public async Task PropertyOn_CreatesTheSettingsAtTheRepositoryRoot_AndLeavesAnUpToDateFileAlone()
    {
        using var project = CreateHost(repository: true);

        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

        var settings = RepositorySettings(project);
        var text = File.ReadAllText(settings);
        text.ShouldContain($"\"url\": {HostSchemaUrl}");
        text.ShouldContain($"\"yaml.schemas\": {{\n    {HostSchemaUrl}: [\"host/appsettings*.yaml\", \"host/appsettings*.yml\"]");
        text.ShouldContain("\"evenBetterToml.schema.associations\"");
        text.ShouldContain("\"/host/appsettings*.json\"");
        project.Warnings("host").ShouldNotContain(z => z.Contains("settings.json", StringComparison.Ordinal));
        Directory.Exists(Path.Combine(project.Directory, "host", ".vscode")).ShouldBeFalse();

        var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(settings, written);

        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

        File.GetLastWriteTimeUtc(settings).ShouldBe(written);
        File.ReadAllText(settings).ShouldBe(text);
    }

    [Test]
    public async Task PropertyOn_UpdatesAnExistingFile_KeepingEverythingElse()
    {
        using var project = CreateHost(repository: true);
        project.AddFile(".vscode/settings.json", "{\r\n\t// my editor\r\n\t\"editor.fontSize\": 14, /* big */\r\n}\r\n");

        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

        var text = File.ReadAllText(RepositorySettings(project));
        text.ShouldStartWith("{\r\n\t// my editor\r\n\t\"editor.fontSize\": 14, /* big */\r\n\t\"json.schemas\": [");
        text.ShouldContain($"\"url\": {HostSchemaUrl}");
        text.Replace("\r\n", "").ShouldNotContain("\n");
    }

    [Test]
    public async Task PropertyOn_WithoutARepository_UsesTheProjectDirectory_AndTheDirectoryPropertyOverrides()
    {
        using var project = CreateHost(repository: false);

        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true -p:DovetailEditorSettingsDirectory=..");

        File.ReadAllText(RepositorySettings(project)).ShouldContain($"\"url\": {HostSchemaUrl}");

        // No .git entry anywhere below the temp directory, so the project directory is the default.
        File.Delete(RepositorySettings(project));
        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

        var local = Path.Combine(project.Directory, "host", ".vscode", "settings.json");
        File.ReadAllText(local).ShouldContain("\"url\": \"./obj/appsettings.schema.json\"");
    }

    [Test]
    public async Task PropertyOn_ForALibrary_OrWithTheSchemaTurnedOff_WritesNothing()
    {
        using var project = CreateHost(repository: true);
        project
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: TargetFramework))
           .AddFile("lib/Fragments.cs", ConfigurationSchemaTests.GreetingsFragment);

        await project.BuildProject("lib", "-p:DovetailScaffoldEditorSettings=true");
        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true -p:DovetailConfigurationSchema=false");

        Directory.Exists(Path.Combine(project.Directory, ".vscode")).ShouldBeFalse();
    }

    [Test]
    public async Task PropertyOn_WithAFileItCantUpdate_WarnsAndLeavesItAlone()
    {
        using var project = CreateHost(repository: true);
        const string unparsable = "{ \"a\": 1 \"b\": 2 }";
        project.AddFile(".vscode/settings.json", unparsable);

        await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

        File.ReadAllText(RepositorySettings(project)).ShouldBe(unparsable);
        var warning = project.Warnings("host").Where(z => z.Contains("left", StringComparison.Ordinal) && z.Contains("settings.json", StringComparison.Ordinal)).ToArray().ShouldHaveSingleItem();
        warning.ShouldContain("\"json.schemas\": [");
        warning.ShouldContain(HostSchemaUrl);
    }

    [Test]
    public async Task PropertyOn_WithAReadOnlyFile_WarnsAndLeavesItAlone()
    {
        using var project = CreateHost(repository: true);
        project.AddFile(".vscode/settings.json", "{ \"a\": 1 }");
        var settings = RepositorySettings(project);
        File.SetAttributes(settings, FileAttributes.ReadOnly);
        try
        {
            await project.BuildProject("host", "-p:DovetailScaffoldEditorSettings=true");

            File.ReadAllText(settings).ShouldBe("{ \"a\": 1 }");
            project.Warnings("host").Where(z => z.Contains("read-only", StringComparison.Ordinal)).ToArray().ShouldHaveSingleItem();
        }
        finally
        {
            File.SetAttributes(settings, FileAttributes.Normal);
        }
    }
}
