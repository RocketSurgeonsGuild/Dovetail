using System.Text;

namespace Dovetail.Build.Tasks;

/// <summary>
///     What Dovetail maps for one host in VS Code's settings: the host's merged schema, and the configuration files it applies to
///     in each of the three formats. Every path is relative to the folder that holds <c>.vscode</c>, which is where VS Code
///     resolves relative schema paths from.
/// </summary>
internal sealed class EditorSettingsMappings(string schemaUrl, string jsonFileMatch, IReadOnlyList<string> yamlGlobs, string tomlPattern)
{
    /// <summary>
    ///     The schema's path, e.g. <c>./MyApp/obj/appsettings.schema.json</c>.
    /// </summary>
    public string SchemaUrl { get; } = schemaUrl;

    /// <summary>
    ///     The <c>json.schemas</c> <c>fileMatch</c> pattern, e.g. <c>/MyApp/appsettings*.json</c>.
    /// </summary>
    public string JsonFileMatch { get; } = jsonFileMatch;

    /// <summary>
    ///     The <c>yaml.schemas</c> globs for <c>appsettings*.yaml</c> and <c>appsettings*.yml</c>.
    /// </summary>
    public IReadOnlyList<string> YamlGlobs { get; } = yamlGlobs;

    /// <summary>
    ///     The <c>evenBetterToml.schema.associations</c> regular expression for <c>appsettings*.toml</c>.
    /// </summary>
    public string TomlPattern { get; } = tomlPattern;

    /// <summary>
    ///     The JSON key of the <c>json.schemas</c> setting.
    /// </summary>
    public const string JsonSchemasKey = "json.schemas";

    /// <summary>
    ///     The JSON key of the <c>yaml.schemas</c> setting.
    /// </summary>
    public const string YamlSchemasKey = "yaml.schemas";

    /// <summary>
    ///     The JSON key of the Even Better TOML associations setting.
    /// </summary>
    public const string TomlAssociationsKey = "evenBetterToml.schema.associations";

    /// <summary>
    ///     Works out the mappings of the host whose configuration files are in <paramref name="projectDirectory" /> and whose schema
    ///     is <paramref name="schemaFile" />, for a settings file under <paramref name="settingsDirectory" /> (the folder that holds
    ///     <c>.vscode</c>). Fails when either path is not below that folder, because VS Code could not then be given a path that
    ///     is relative to it.
    /// </summary>
    public static bool TryCreate(string settingsDirectory, string projectDirectory, string schemaFile, out EditorSettingsMappings? mappings, out string error)
    {
        mappings = null;
        var schema = RelativePath(settingsDirectory, schemaFile);
        var project = RelativePath(settingsDirectory, projectDirectory);
        if (schema is null || schema.StartsWith("..", StringComparison.Ordinal))
        {
            error = $"the schema file {schemaFile} isn't below {settingsDirectory}";
            return false;
        }

        if (project is null || project.StartsWith("..", StringComparison.Ordinal))
        {
            error = $"the project directory {projectDirectory} isn't below {settingsDirectory}";
            return false;
        }

        var prefix = project.Length == 0 ? "" : project + "/";
        mappings = new(
            "./" + schema,
            "/" + prefix + "appsettings*.json",
            [prefix + "appsettings*.yaml", prefix + "appsettings*.yml"],
            "^.+/" + EscapeRegex(prefix) + "appsettings.*\\.toml$"
        );
        error = "";
        return true;
    }

    /// <summary>
    ///     The text to paste into <c>.vscode/settings.json</c> for these mappings alone.
    /// </summary>
    public string ToSnippet() => VsCodeSettingsEditor.RenderDocument(this, "\n", "  ");

    /// <summary>
    ///     A path as it is compared: with forward slashes and without leading <c>./</c>.
    /// </summary>
    public static string NormalizePath(string path)
    {
        path = path.Trim().Replace('\\', '/');
        while (path.StartsWith("./", StringComparison.Ordinal)) path = path.Substring(2);

        return path;
    }

    /// <summary>
    ///     The forward-slash path from directory <paramref name="from" /> to <paramref name="to" />, empty when they are the same, or
    ///     null when they have no common root (different drives).
    /// </summary>
    internal static string? RelativePath(string from, string to)
    {
        var comparison = Path.DirectorySeparatorChar == '\\' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var fromParts = Split(Path.GetFullPath(from));
        var toParts = Split(Path.GetFullPath(to));
        var common = 0;
        while (common < fromParts.Length && common < toParts.Length && string.Equals(fromParts[common], toParts[common], comparison)) common++;

        // Nothing in common, not even the root: a different drive (or a UNC share).
        if (common == 0 && Path.DirectorySeparatorChar == '\\' && fromParts.Length > 0 && toParts.Length > 0) return null;

        return string.Join("/", Enumerable.Repeat("..", fromParts.Length - common).Concat(toParts.Skip(common)));

        static string[] Split(string path) => path.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries);
    }

    private static string EscapeRegex(string text)
    {
        var escaped = new StringBuilder();
        foreach (var c in text)
        {
            if (c is '\\' or '^' or '$' or '.' or '|' or '?' or '*' or '+' or '(' or ')' or '[' or ']' or '{' or '}') escaped.Append('\\');

            escaped.Append(c);
        }

        return escaped.ToString();
    }
}
