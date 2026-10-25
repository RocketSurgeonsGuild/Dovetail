using Microsoft.Build.Framework;

namespace Dovetail.Build.Tasks;

/// <summary>
///     Creates or updates Dovetail's own mappings for a host in a VS Code <c>.vscode/settings.json</c>: <c>json.schemas</c>,
///     <c>yaml.schemas</c> and <c>evenBetterToml.schema.associations</c>, each pointing at the host's merged
///     <c>appsettings.schema.json</c>. Everything else in the file is left as it is (see <see cref="VsCodeSettingsEditor" />).
/// </summary>
/// <remarks>
///     A file the task can't update safely (it doesn't parse, has an unexpected shape or is read-only) is not touched: the task
///     logs a warning with the mappings to paste instead. The file and its <c>.vscode</c> folder are created only when missing.
///     Nothing is written, and the file's timestamp is left alone, when the mappings are already there.
/// </remarks>
[PublicAPI]
public sealed class ScaffoldEditorSettings : Microsoft.Build.Utilities.Task
{
    /// <summary>
    ///     The host's project directory, where its <c>appsettings*</c> files are, as an absolute path.
    /// </summary>
    [Required]
    public string ProjectDirectory { get; set; } = "";

    /// <summary>
    ///     The host's merged schema file, as an absolute path.
    /// </summary>
    [Required]
    public string SchemaFile { get; set; } = "";

    /// <summary>
    ///     The folder that holds <c>.vscode</c>, as an absolute path. Defaults to the nearest ancestor of the project directory that
    ///     contains a <c>.git</c> entry, or the project directory itself when there is none.
    /// </summary>
    public string SettingsDirectory { get; set; } = "";

    /// <summary>
    ///     The settings file the task worked on.
    /// </summary>
    [Output]
    public string SettingsFile { get; set; } = "";

    /// <summary>
    ///     Whether the task created or changed the file.
    /// </summary>
    [Output]
    public bool Updated { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
        if (!Path.IsPathRooted(ProjectDirectory) || !Path.IsPathRooted(SchemaFile) || ( SettingsDirectory.Length > 0 && !Path.IsPathRooted(SettingsDirectory) ))
        {
            Log.LogError("Dovetail: ScaffoldEditorSettings needs absolute paths, not \"{0}\", \"{1}\" and \"{2}\".", ProjectDirectory, SchemaFile, SettingsDirectory);
            return false;
        }

        var outcome = EditorSettingsScaffold.Run(ProjectDirectory, SchemaFile, SettingsDirectory);
        SettingsFile = outcome.SettingsFile;
        switch (outcome.Status)
        {
            case EditorSettingsStatus.Unsafe:
                Log.LogWarning("{0}", outcome.Warning);
                break;
            case EditorSettingsStatus.Updated:
                Updated = true;
                Log.LogMessage(MessageImportance.Normal, "Dovetail: mapped {0} in {1}", outcome.Mappings!.SchemaUrl, outcome.SettingsFile);
                break;
            default:
                Log.LogMessage(MessageImportance.Low, "Dovetail: {0} already maps {1}", outcome.SettingsFile, outcome.Mappings!.SchemaUrl);
                break;
        }

        return !Log.HasLoggedErrors;
    }
}
