namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     Collects the configuration schema fragments of a set of assemblies.
/// </summary>
internal static class ConfigurationSchemaFragments
{
    /// <summary>
    ///     Reads every assembly in <paramref name="assemblyPaths" /> (each once, in order), reporting through
    ///     <paramref name="log" /> what was found and, without failing, what was skipped: unreadable files, unknown versions and
    ///     malformed fragments.
    /// </summary>
    public static IReadOnlyList<ConfigurationSchemaFragment> Read(IEnumerable<string> assemblyPaths, Action<string> log)
    {
        var fragments = new List<ConfigurationSchemaFragment>();
        foreach (var path in assemblyPaths.Distinct(StringComparer.Ordinal))
        {
            IReadOnlyList<KeyValuePair<string, string?>> entries;
            try
            {
                entries = AssemblyMetadataReader.Read(path, ConfigurationSchemaFragment.KeyPrefix);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or BadImageFormatException)
            {
                log($"Dovetail: skipped {path} when reading configuration schema fragments: {exception.Message}");
                continue;
            }

            var source = Path.GetFileNameWithoutExtension(path);
            foreach (var entry in entries)
            {
                if (ConfigurationSchemaFragment.TryParse(entry.Key, entry.Value, source, out var problem) is { } fragment)
                {
                    log($"Dovetail: configuration schema fragment \"{string.Join(":", fragment.Path)}\" from {source}");
                    fragments.Add(fragment);
                }
                else
                {
                    log($"Dovetail: skipped configuration schema fragment \"{entry.Key}\" in {source}: {problem}");
                }
            }
        }

        return fragments;
    }
}
