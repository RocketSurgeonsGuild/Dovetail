using Microsoft.Build.Framework;

namespace Dovetail.Build.Tasks;

/// <summary>
///     Writes a library's own configuration schema fragments as its <c>ConfigurationSchema.json</c>, permissive and rooted at the
///     configuration root (the shape Aspire's packages ship at their package root). Deletes the file when the library declares no
///     sections, so a stale one is never packed.
/// </summary>
/// <remarks>
///     Unreadable assemblies, unknown fragment versions and malformed fragments are skipped with a low-importance message rather
///     than failing the build.
/// </remarks>
[PublicAPI]
public sealed class WriteConfigurationSchemaSegment : Microsoft.Build.Utilities.Task
{
    /// <summary>
    ///     The project's own assembly (<c>@(IntermediateAssembly)</c>), as an absolute path.
    /// </summary>
    [Required]
    public ITaskItem[] Assemblies { get; set; } = [];

    /// <summary>
    ///     The absolute path of the file to write.
    /// </summary>
    [Required]
    public string OutputFile { get; set; } = "";

    /// <summary>
    ///     The number of fragments written.
    /// </summary>
    [Output]
    public int FragmentCount { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
        if (!Path.IsPathRooted(OutputFile))
        {
            Log.LogError("Dovetail: WriteConfigurationSchemaSegment needs an absolute OutputFile, not \"{0}\".", OutputFile);
            return false;
        }

        var assemblies = Assemblies.Select(z => z.GetMetadata("FullPath")).Where(z => z.Length > 0).ToArray();
        var fragments = ConfigurationSchemaFragments.Read(assemblies, message => Log.LogMessage(MessageImportance.Low, message));
        FragmentCount = fragments.Count;

        try
        {
            if (ConfigurationSchemaDocument.CreateLibrarySchema(fragments) is { } document)
            {
                var written = ConfigurationSchemaDocument.Write(OutputFile, ConfigurationSchemaDocument.Serialize(document));
                Log.LogMessage(MessageImportance.Normal, "Dovetail: {0} {1} ({2} configuration schema fragments)", written ? "wrote" : "kept the unchanged", OutputFile, fragments.Count);
            }
            else if (File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
                Log.LogMessage(MessageImportance.Normal, "Dovetail: deleted {0}: the project declares no configuration sections any more", OutputFile);
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Log.LogWarning("Dovetail: couldn't update the configuration schema {0}: {1}", OutputFile, exception.Message);
        }

        return !Log.HasLoggedErrors;
    }
}
