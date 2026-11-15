using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace Joinery.Dovetail.Templates.Tasks;

/// <summary>
///     Writes one DovetailTemplate with its tokens replaced (Joinery.Dovetail.Templates.targets): every non-empty metadata value of
///     the item replaces <c>{Name}</c> and <c>__Name__</c>, in one pass, names ignoring case; <c>HostName</c> and <c>HostType</c>
///     only when <c>HostType</c> is set. A token no metadata fills is left as it is.
/// </summary>
/// <remarks>
///     A task because MSBuild has no way to list an item's custom metadata. The file is UTF-8 without a BOM with a trailing new line,
///     what <c>WriteLinesToFile</c> wrote before, and is left alone when its content hasn't changed, so incremental builds don't
///     recompile.
/// </remarks>
[PublicAPI]
public sealed class ExpandTemplate : Microsoft.Build.Utilities.Task
{
    private static readonly Regex Token = new(@"\{(?<name>[A-Za-z_]\w*)\}|__(?<name>[A-Za-z]\w*?)__", RegexOptions.CultureInvariant);

    /// <summary>
    ///     The DovetailEmittedTemplate item: its metadata are the token values.
    /// </summary>
    [Required]
    public ITaskItem Template { get; set; } = null!;

    /// <summary>
    ///     The template file to read.
    /// </summary>
    [Required]
    public string File { get; set; } = "";

    /// <summary>
    ///     The template's HostName; replaces its tokens only when <see cref="HostType" /> is set.
    /// </summary>
    public string HostName { get; set; } = "";

    /// <summary>
    ///     The template's HostType.
    /// </summary>
    public string HostType { get; set; } = "";

    /// <summary>
    ///     The absolute path of the file to write.
    /// </summary>
    [Required]
    public string OutputFile { get; set; } = "";

    /// <inheritdoc />
    public override bool Execute()
    {
        if (!Path.IsPathRooted(OutputFile))
        {
            Log.LogError("Dovetail: ExpandTemplate needs an absolute OutputFile, not \"{0}\".", OutputFile);
            return false;
        }

        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (DictionaryEntry entry in Template.CloneCustomMetadata())
        {
            if (entry.Value is string { Length: > 0 } value) values[(string)entry.Key] = value;
        }

        values.Remove("HostName");
        values.Remove("HostType");
        if (HostType.Length > 0)
        {
            values["HostType"] = HostType;
            if (HostName.Length > 0) values["HostName"] = HostName;
        }

        var content = Token.Replace(
                          System.IO.File.ReadAllText(File),
                          match => values.TryGetValue(match.Groups["name"].Value, out var value) ? value : match.Value
                      )
                    + Environment.NewLine;

        if (System.IO.File.Exists(OutputFile) && System.IO.File.ReadAllText(OutputFile) == content)
        {
            Log.LogMessage(MessageImportance.Low, "Dovetail: kept the unchanged {0}", OutputFile);
            return true;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(OutputFile)!);
        System.IO.File.WriteAllText(OutputFile, content, new UTF8Encoding(false));
        Log.LogMessage(MessageImportance.Low, "Dovetail: wrote {0} from {1}", OutputFile, File);
        return !Log.HasLoggedErrors;
    }
}
