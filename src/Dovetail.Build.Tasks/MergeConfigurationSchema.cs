using Microsoft.Build.Framework;

namespace Dovetail.Build.Tasks;

/// <summary>
///     Merges the configuration schema fragments of an application (or test project) and every assembly it references into its
///     appsettings.schema.json, closing Dovetail's sections strictly and embedding SchemaStore's appsettings schema.
/// </summary>
/// <remarks>
///     Fragments are read from assembly metadata, so nothing is loaded. Unreadable assemblies, unknown fragment versions and
///     malformed fragments are skipped with a low-importance message rather than failing the build.
/// </remarks>
[PublicAPI]
public sealed class MergeConfigurationSchema : Microsoft.Build.Utilities.Task
{
    /// <summary>
    ///     The resolved references (<c>@(ReferencePath)</c>), as absolute paths.
    /// </summary>
    [Required]
    public ITaskItem[] References { get; set; } = [];

    /// <summary>
    ///     The project's own assembly (<c>@(IntermediateAssembly)</c>), as an absolute path.
    /// </summary>
    public ITaskItem[] ProjectAssemblies { get; set; } = [];

    /// <summary>
    ///     The absolute path of the schema to write.
    /// </summary>
    [Required]
    public string OutputFile { get; set; } = "";

    /// <summary>
    ///     Whether to close Dovetail's sections with case-insensitive <c>propertyNames</c> patterns. Defaults to <see langword="true" />.
    /// </summary>
    public bool Strict { get; set; } = true;

    /// <summary>
    ///     Whether to embed SchemaStore's appsettings schema as the base. Defaults to <see langword="true" />.
    /// </summary>
    public bool IncludeSchemaStore { get; set; } = true;

    /// <summary>
    ///     The number of fragments merged.
    /// </summary>
    [Output]
    public int FragmentCount { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
        if (!Path.IsPathRooted(OutputFile))
        {
            Log.LogError("Dovetail: MergeConfigurationSchema needs an absolute OutputFile, not \"{0}\".", OutputFile);
            return false;
        }

        var assemblies = References.Concat(ProjectAssemblies).Select(z => z.GetMetadata("FullPath")).Where(z => z.Length > 0).ToArray();
        var fragments = ConfigurationSchemaFragments.Read(assemblies, message => Log.LogMessage(MessageImportance.Low, message));
        FragmentCount = fragments.Count;

        var document = ConfigurationSchemaDocument.CreateApplicationSchema(fragments, Strict, IncludeSchemaStore);
        try
        {
            var written = ConfigurationSchemaDocument.Write(OutputFile, ConfigurationSchemaDocument.Serialize(document));
            Log.LogMessage(
                MessageImportance.Normal,
                "Dovetail: {0} {1} ({2} configuration schema fragments from {3} assemblies)",
                written ? "wrote" : "kept the unchanged",
                OutputFile,
                fragments.Count,
                assemblies.Length
            );
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Two builds of the same project (other target frameworks or runtime identifiers) may race for the file; it's only an
            // editor aid, so don't fail the build over it.
            Log.LogWarning("Dovetail: couldn't write the configuration schema {0}: {1}", OutputFile, exception.Message);
        }

        return !Log.HasLoggedErrors;
    }
}
