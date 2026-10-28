namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     What <see cref="EditorSettingsScaffold.Run" /> did. <see cref="Warning" /> is set when the file was left alone.
/// </summary>
internal readonly struct EditorSettingsScaffoldOutcome(string settingsFile, EditorSettingsMappings? mappings, EditorSettingsStatus status, string? warning)
{
    public string SettingsFile { get; } = settingsFile;

    public EditorSettingsMappings? Mappings { get; } = mappings;

    public EditorSettingsStatus Status { get; } = status;

    public string? Warning { get; } = warning;
}

/// <summary>
///     Finds the settings file for a host and brings Dovetail's mappings in it up to date (the work behind
///     <see cref="ScaffoldEditorSettings" />, apart from the MSBuild logging).
/// </summary>
internal static class EditorSettingsScaffold
{
    private const int MaxLockAttempts = 50;
    private const int LockRetryMilliseconds = 100;

    /// <summary>
    ///     Creates or updates the mappings of the host in <paramref name="projectDirectory" /> whose schema is
    ///     <paramref name="schemaFile" />. <paramref name="settingsDirectory" /> (the folder that holds <c>.vscode</c>) defaults to
    ///     the nearest ancestor of the project directory with a <c>.git</c> entry, or the project directory.
    /// </summary>
    public static EditorSettingsScaffoldOutcome Run(string projectDirectory, string schemaFile, string settingsDirectory)
    {
        projectDirectory = Path.GetFullPath(projectDirectory);
        settingsDirectory = settingsDirectory.Length > 0 ? Path.GetFullPath(settingsDirectory) : FindRepositoryRoot(projectDirectory) ?? projectDirectory;
        var file = Path.Combine(settingsDirectory, ".vscode", "settings.json");

        if (!EditorSettingsMappings.TryCreate(settingsDirectory, projectDirectory, Path.GetFullPath(schemaFile), out var mappings, out var error))
        {
            return new(
                file,
                null,
                EditorSettingsStatus.Unsafe,
                $"Dovetail: can't map this project's configuration schema in {file}: {error}. Set DovetailEditorSettingsDirectory to a folder that contains the project, or DovetailScaffoldEditorSettings to false."
            );
        }

        var (status, reason) = Update(file, mappings!);
        return new(
            file,
            mappings,
            status,
            status == EditorSettingsStatus.Unsafe
                ? $"Dovetail: left {file} as it is ({reason}). Add these entries to it to get completion and validation for this project's configuration, or set DovetailScaffoldEditorSettings to false to stop this warning:\n{mappings!.ToSnippet().TrimEnd()}"
                : null
        );
    }

    /// <summary>
    ///     The nearest ancestor of <paramref name="directory" /> (itself included) that holds a <c>.git</c> folder or file, if any.
    /// </summary>
    public static string? FindRepositoryRoot(string directory)
    {
        for (var current = new DirectoryInfo(directory); current is not null; current = current.Parent)
        {
            var git = Path.Combine(current.FullName, ".git");
            if (Directory.Exists(git) || File.Exists(git)) return current.FullName;
        }

        return null;
    }

    /// <summary>
    ///     Reads, edits and writes the file while holding it, so hosts that build in parallel and share one settings file never
    ///     overwrite each other's entries.
    /// </summary>
    private static (EditorSettingsStatus Status, string Reason) Update(string file, EditorSettingsMappings mappings)
    {
        try
        {
            if (File.Exists(file) && ( File.GetAttributes(file) & FileAttributes.ReadOnly ) != 0)
            {
                return ReadOnly(file, mappings, "it is read-only");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(file)!);
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    using var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    var original = new byte[stream.Length];
                    var read = 0;
                    while (read < original.Length)
                    {
                        var count = stream.Read(original, read, original.Length - read);
                        if (count == 0) break;

                        read += count;
                    }

                    var result = VsCodeSettingsEditor.Apply(original, mappings);
                    if (result.Status == EditorSettingsStatus.Updated)
                    {
                        stream.Position = 0;
                        stream.Write(result.Content, 0, result.Content.Length);
                        stream.SetLength(result.Content.Length);
                    }

                    return (result.Status, result.Reason);
                }
                catch (IOException) when (attempt < MaxLockAttempts && File.Exists(file))
                {
                    // Another host's build holds the file for a moment.
                    Thread.Sleep(LockRetryMilliseconds);
                }
            }
        }
        catch (UnauthorizedAccessException exception)
        {
            return ReadOnly(file, mappings, exception.Message);
        }
        catch (IOException exception)
        {
            return (EditorSettingsStatus.Unsafe, exception.Message);
        }
    }

    /// <summary>
    ///     A file that can't be written is fine when it already holds the mappings.
    /// </summary>
    private static (EditorSettingsStatus Status, string Reason) ReadOnly(string file, EditorSettingsMappings mappings, string why)
    {
        try
        {
            var result = VsCodeSettingsEditor.Apply(File.ReadAllBytes(file), mappings);
            return result.Status == EditorSettingsStatus.Unchanged ? (EditorSettingsStatus.Unchanged, "") : (EditorSettingsStatus.Unsafe, result.Status == EditorSettingsStatus.Unsafe ? result.Reason : why);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            return (EditorSettingsStatus.Unsafe, why);
        }
    }
}
