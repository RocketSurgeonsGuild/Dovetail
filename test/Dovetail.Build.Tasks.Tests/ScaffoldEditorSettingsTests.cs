namespace Dovetail.Build.Tasks.Tests;

/// <summary>
///     The work around the editor (<see cref="ScaffoldEditorSettings" /> minus the MSBuild logging): where the settings file goes,
///     what is written when, and when it warns instead.
/// </summary>
public class ScaffoldEditorSettingsTests
{
    /// <summary>
    ///     A repository (a directory with a .git folder) holding a host project, in a fresh temporary directory.
    /// </summary>
    private sealed class Repository : IDisposable
    {
        public Repository(bool git = true)
        {
            Root = Directory.CreateTempSubdirectory("dovetail-scaffold-").FullName;
            if (git) Directory.CreateDirectory(Path.Combine(Root, ".git"));

            Project = Path.Combine(Root, "src", "Host");
            Directory.CreateDirectory(Project);
        }

        public string Root { get; }

        public string Project { get; }

        public string Settings => Path.Combine(Root, ".vscode", "settings.json");

        public EditorSettingsScaffoldOutcome Run(string settingsDirectory = "", string? project = null)
        {
            var projectDirectory = project ?? Project;
            return EditorSettingsScaffold.Run(projectDirectory, Path.Combine(projectDirectory, "obj", "appsettings.schema.json"), settingsDirectory);
        }

        public void Dispose()
        {
            foreach (var file in Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories)) File.SetAttributes(file, FileAttributes.Normal);

            Directory.Delete(Root, recursive: true);
        }
    }

    [Test]
    public void SettingsGoToTheNearestAncestorWithAGitEntry_AndAreCreatedWithTheirFolder()
    {
        using var repository = new Repository();

        var outcome = repository.Run();

        outcome.SettingsFile.ShouldBe(repository.Settings);
        outcome.Status.ShouldBe(EditorSettingsStatus.Updated);
        outcome.Warning.ShouldBeNull();
        File.ReadAllText(repository.Settings).ShouldContain("\"./src/Host/obj/appsettings.schema.json\"");
    }

    [Test]
    public void AGitFile_CountsAsAGitEntry()
    {
        using var repository = new Repository(git: false);
        File.WriteAllText(Path.Combine(repository.Root, ".git"), "gitdir: ../elsewhere");

        repository.Run().SettingsFile.ShouldBe(repository.Settings);
    }

    [Test]
    public void WithoutAGitEntry_TheProjectDirectoryIsUsed()
    {
        // A temporary directory's ancestors could hold a .git entry on a developer machine; the rule decides, so look for one first.
        using var repository = new Repository(git: false);
        if (EditorSettingsScaffold.FindRepositoryRoot(repository.Project) is { } ancestor) Skip.Test($"{ancestor} is a repository");

        var outcome = repository.Run();

        outcome.SettingsFile.ShouldBe(Path.Combine(repository.Project, ".vscode", "settings.json"));
        File.ReadAllText(outcome.SettingsFile).ShouldContain("\"./obj/appsettings.schema.json\"");
    }

    [Test]
    public void AnExplicitDirectory_WinsOverTheGitRoot()
    {
        using var repository = new Repository();
        var custom = Path.Combine(repository.Root, "src");

        var outcome = repository.Run(custom);

        outcome.SettingsFile.ShouldBe(Path.Combine(custom, ".vscode", "settings.json"));
        File.Exists(repository.Settings).ShouldBeFalse();
        File.ReadAllText(outcome.SettingsFile).ShouldContain("\"./Host/obj/appsettings.schema.json\"");
    }

    [Test]
    public void AnUpToDateFile_IsNotRewritten_AndKeepsItsTimestamp()
    {
        using var repository = new Repository();
        repository.Run().Status.ShouldBe(EditorSettingsStatus.Updated);
        var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(repository.Settings, written);
        var content = File.ReadAllBytes(repository.Settings);

        repository.Run().Status.ShouldBe(EditorSettingsStatus.Unchanged);

        File.GetLastWriteTimeUtc(repository.Settings).ShouldBe(written);
        File.ReadAllBytes(repository.Settings).ShouldBe(content);
    }

    [Test]
    public void ExistingSettings_GetOnlyTheMappingsAdded()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        File.WriteAllText(repository.Settings, "{\n  // keep\n  \"a\": 1\n}\n");

        var outcome = repository.Run();

        outcome.Status.ShouldBe(EditorSettingsStatus.Updated);
        outcome.Warning.ShouldBeNull();
        File.ReadAllText(repository.Settings).ShouldStartWith("{\n  // keep\n  \"a\": 1,\n  \"json.schemas\": [");
    }

    [Test]
    public void TwoHostsInOneRepository_EachGetTheirOwnEntries()
    {
        using var repository = new Repository();
        var other = Path.Combine(repository.Root, "src", "Other");
        Directory.CreateDirectory(other);

        repository.Run();
        repository.Run(project: other);
        repository.Run().Status.ShouldBe(EditorSettingsStatus.Unchanged);
        repository.Run(project: other).Status.ShouldBe(EditorSettingsStatus.Unchanged);

        var text = File.ReadAllText(repository.Settings);
        text.Split("\"./src/Host/obj/appsettings.schema.json\"").Length.ShouldBe(4);
        text.Split("\"./src/Other/obj/appsettings.schema.json\"").Length.ShouldBe(4);
    }

    [Test]
    public async Task HostsBuildingInParallel_DontLoseEachOthersEntries()
    {
        using var repository = new Repository();
        var projects = Enumerable.Range(0, 6).Select(z => Path.Combine(repository.Root, "src", $"Host{z}")).ToArray();
        foreach (var project in projects) Directory.CreateDirectory(project);

        await Task.WhenAll(projects.Select(project => Task.Run(() => repository.Run(project: project))));

        var text = File.ReadAllText(repository.Settings);
        foreach (var project in projects)
        {
            text.Split($"\"./src/{Path.GetFileName(project)}/obj/appsettings.schema.json\"").Length.ShouldBe(4);
        }
    }

    [Test]
    public void AFileThatCantBeUpdatedSafely_IsLeftAloneWithAWarningAndTheSnippet()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        const string broken = "{ \"a\": 1 \"b\": 2 }";
        File.WriteAllText(repository.Settings, broken);
        var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(repository.Settings, written);

        var outcome = repository.Run();

        outcome.Status.ShouldBe(EditorSettingsStatus.Unsafe);
        File.ReadAllText(repository.Settings).ShouldBe(broken);
        File.GetLastWriteTimeUtc(repository.Settings).ShouldBe(written);
        outcome.Warning.ShouldNotBeNull().ShouldContain(repository.Settings);
        outcome.Warning.ShouldContain("\"json.schemas\": [");
        outcome.Warning.ShouldContain("\"url\": \"./src/Host/obj/appsettings.schema.json\"");
        outcome.Warning.ShouldContain("\"evenBetterToml.schema.associations\"");
    }

    [Test]
    public void AReadOnlyFile_IsLeftAloneWithAWarning_UnlessItAlreadyHasTheMappings()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        File.WriteAllText(repository.Settings, "{ \"a\": 1 }");
        File.SetAttributes(repository.Settings, FileAttributes.ReadOnly);

        var outcome = repository.Run();

        outcome.Status.ShouldBe(EditorSettingsStatus.Unsafe);
        outcome.Warning.ShouldNotBeNull().ShouldContain("read-only");
        File.ReadAllText(repository.Settings).ShouldBe("{ \"a\": 1 }");

        File.SetAttributes(repository.Settings, FileAttributes.Normal);
        repository.Run().Status.ShouldBe(EditorSettingsStatus.Updated);
        File.SetAttributes(repository.Settings, FileAttributes.ReadOnly);

        var again = repository.Run();

        again.Status.ShouldBe(EditorSettingsStatus.Unchanged);
        again.Warning.ShouldBeNull();
    }

    [Test]
    public void AProjectOutsideTheSettingsDirectory_WarnsInsteadOfWritingAnUnusablePath()
    {
        using var repository = new Repository();
        var elsewhere = Directory.CreateTempSubdirectory("dovetail-scaffold-other-").FullName;
        try
        {
            var outcome = repository.Run(elsewhere);

            File.Exists(Path.Combine(elsewhere, ".vscode", "settings.json")).ShouldBeFalse();
            outcome.Warning.ShouldNotBeNull().ShouldContain("isn't below");
        }
        finally
        {
            Directory.Delete(elsewhere, recursive: true);
        }
    }
}
