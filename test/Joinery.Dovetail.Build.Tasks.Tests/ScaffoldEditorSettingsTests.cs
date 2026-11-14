namespace Joinery.Dovetail.Build.Tasks.Tests;

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
    public async Task SettingsGoToTheNearestAncestorWithAGitEntry_AndAreCreatedWithTheirFolder()
    {
        using var repository = new Repository();

        var outcome = repository.Run();

        await outcome.SettingsFile.Should().BeEqualTo(repository.Settings);
        await outcome.Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        await outcome.Warning.Should().BeNull();
        await File.ReadAllText(repository.Settings).Should().Contain("\"./src/Host/obj/appsettings.schema.json\"");
    }

    [Test]
    public async Task AGitFile_CountsAsAGitEntry()
    {
        using var repository = new Repository(git: false);
        File.WriteAllText(Path.Combine(repository.Root, ".git"), "gitdir: ../elsewhere");

        await repository.Run().SettingsFile.Should().BeEqualTo(repository.Settings);
    }

    [Test]
    public async Task WithoutAGitEntry_TheProjectDirectoryIsUsed()
    {
        // A temporary directory's ancestors could hold a .git entry on a developer machine; the rule decides, so look for one first.
        using var repository = new Repository(git: false);
        if (EditorSettingsScaffold.FindRepositoryRoot(repository.Project) is { } ancestor) Skip.Test($"{ancestor} is a repository");

        var outcome = repository.Run();

        await outcome.SettingsFile.Should().BeEqualTo(Path.Combine(repository.Project, ".vscode", "settings.json"));
        await File.ReadAllText(outcome.SettingsFile).Should().Contain("\"./obj/appsettings.schema.json\"");
    }

    [Test]
    public async Task AnExplicitDirectory_WinsOverTheGitRoot()
    {
        using var repository = new Repository();
        var custom = Path.Combine(repository.Root, "src");

        var outcome = repository.Run(custom);

        await outcome.SettingsFile.Should().BeEqualTo(Path.Combine(custom, ".vscode", "settings.json"));
        await File.Exists(repository.Settings).Should().BeFalse();
        await File.ReadAllText(outcome.SettingsFile).Should().Contain("\"./Host/obj/appsettings.schema.json\"");
    }

    [Test]
    public async Task AnUpToDateFile_IsNotRewritten_AndKeepsItsTimestamp()
    {
        using var repository = new Repository();
        await repository.Run().Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(repository.Settings, written);
        var content = File.ReadAllBytes(repository.Settings);

        await repository.Run().Status.Should().BeEqualTo(EditorSettingsStatus.Unchanged);

        await File.GetLastWriteTimeUtc(repository.Settings).Should().BeEqualTo(written);
        await File.ReadAllBytes(repository.Settings).Should().BeEquivalentTo(content, CollectionOrdering.Matching);
    }

    [Test]
    public async Task ExistingSettings_GetOnlyTheMappingsAdded()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        File.WriteAllText(repository.Settings, "{\n  // keep\n  \"a\": 1\n}\n");

        var outcome = repository.Run();

        await outcome.Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        await outcome.Warning.Should().BeNull();
        await File.ReadAllText(repository.Settings).Should().StartWith("{\n  // keep\n  \"a\": 1,\n  \"json.schemas\": [");
    }

    [Test]
    public async Task TwoHostsInOneRepository_EachGetTheirOwnEntries()
    {
        using var repository = new Repository();
        var other = Path.Combine(repository.Root, "src", "Other");
        Directory.CreateDirectory(other);

        repository.Run();
        repository.Run(project: other);
        await repository.Run().Status.Should().BeEqualTo(EditorSettingsStatus.Unchanged);
        await repository.Run(project: other).Status.Should().BeEqualTo(EditorSettingsStatus.Unchanged);

        var text = File.ReadAllText(repository.Settings);
        await text.Split("\"./src/Host/obj/appsettings.schema.json\"").Length.Should().BeEqualTo(4);
        await text.Split("\"./src/Other/obj/appsettings.schema.json\"").Length.Should().BeEqualTo(4);
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
            await text.Split($"\"./src/{Path.GetFileName(project)}/obj/appsettings.schema.json\"").Length.Should().BeEqualTo(4);
        }
    }

    [Test]
    public async Task AFileThatCantBeUpdatedSafely_IsLeftAloneWithAWarningAndTheSnippet()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        const string broken = "{ \"a\": 1 \"b\": 2 }";
        File.WriteAllText(repository.Settings, broken);
        var written = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(repository.Settings, written);

        var outcome = repository.Run();

        await outcome.Status.Should().BeEqualTo(EditorSettingsStatus.Unsafe);
        await File.ReadAllText(repository.Settings).Should().BeEqualTo(broken);
        await File.GetLastWriteTimeUtc(repository.Settings).Should().BeEqualTo(written);
        await ( await outcome.Warning.Should().NotBeNull() ).Should().Contain(repository.Settings);
        await outcome.Warning.Should().Contain("\"json.schemas\": [");
        await outcome.Warning.Should().Contain("\"url\": \"./src/Host/obj/appsettings.schema.json\"");
        await outcome.Warning.Should().Contain("\"evenBetterToml.schema.associations\"");
    }

    [Test]
    public async Task AReadOnlyFile_IsLeftAloneWithAWarning_UnlessItAlreadyHasTheMappings()
    {
        using var repository = new Repository();
        Directory.CreateDirectory(Path.GetDirectoryName(repository.Settings)!);
        File.WriteAllText(repository.Settings, "{ \"a\": 1 }");
        File.SetAttributes(repository.Settings, FileAttributes.ReadOnly);

        var outcome = repository.Run();

        await outcome.Status.Should().BeEqualTo(EditorSettingsStatus.Unsafe);
        await ( await outcome.Warning.Should().NotBeNull() ).Should().Contain("read-only");
        await File.ReadAllText(repository.Settings).Should().BeEqualTo("{ \"a\": 1 }");

        File.SetAttributes(repository.Settings, FileAttributes.Normal);
        await repository.Run().Status.Should().BeEqualTo(EditorSettingsStatus.Updated);
        File.SetAttributes(repository.Settings, FileAttributes.ReadOnly);

        var again = repository.Run();

        await again.Status.Should().BeEqualTo(EditorSettingsStatus.Unchanged);
        await again.Warning.Should().BeNull();
    }

    [Test]
    public async Task AProjectOutsideTheSettingsDirectory_WarnsInsteadOfWritingAnUnusablePath()
    {
        using var repository = new Repository();
        var elsewhere = Directory.CreateTempSubdirectory("dovetail-scaffold-other-").FullName;
        try
        {
            var outcome = repository.Run(elsewhere);

            await File.Exists(Path.Combine(elsewhere, ".vscode", "settings.json")).Should().BeFalse();
            await ( await outcome.Warning.Should().NotBeNull() ).Should().Contain("isn't below");
        }
        finally
        {
            Directory.Delete(elsewhere, recursive: true);
        }
    }
}
