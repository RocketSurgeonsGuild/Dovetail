using System.Reflection;

namespace Dovetail;

/// <summary>
///     Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no
///     file system to read them from (mobile and other app-package hosts).
/// </summary>
/// <remarks>
///     A host turns this on by setting <see cref="AssemblyKey" /> on the context to the assembly that carries the application's
///     configuration. The configuration joints then read the embedded copies instead of files on disk.
/// </remarks>
[PublicAPI]
public static class DovetailEmbeddedConfiguration
{
    /// <summary>
    ///     The context key that holds the <see cref="Assembly" /> the application's configuration files are embedded in.
    /// </summary>
    public const string AssemblyKey = "EmbeddedConfigurationAssembly";

    extension(IDovetailContext context)
    {
        /// <summary>
        ///     The assembly the application's configuration files are embedded in, or <see langword="null" /> when configuration is read from disk.
        /// </summary>
        public Assembly? EmbeddedConfigurationAssembly => context.Get<Assembly>(AssemblyKey);
    }

    /// <summary>
    ///     Opens the first embedded resource that matches one of <paramref name="fileNames" />, in order. A resource matches when its name is
    ///     the file name itself (the Dovetail SDK's naming) or ends with <c>.{fileName}</c> (the default manifest naming of other tools).
    /// </summary>
    /// <param name="assembly">The assembly to search.</param>
    /// <param name="fileNames">The candidate file names, most preferred first.</param>
    /// <returns>The resource stream, or <see langword="null" /> if no resource matches.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="assembly" /> is <see langword="null" />.</exception>
    public static Stream? OpenResource(Assembly assembly, params ReadOnlySpan<string> fileNames)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        var resourceNames = assembly.GetManifestResourceNames();
        foreach (var fileName in fileNames)
        {
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.Equals(fileName, StringComparison.OrdinalIgnoreCase)
                 || ( resourceName.Length > fileName.Length
                     && resourceName[^( fileName.Length + 1 )] == '.'
                     && resourceName.EndsWith(fileName, StringComparison.OrdinalIgnoreCase) ))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets the conventional configuration file names, lowest precedence first: <c>appsettings</c>, the application name, each of those
    ///     qualified by the environment name, and each of those qualified by <c>local</c>.
    /// </summary>
    /// <param name="context">The context to read the <c>ApplicationName</c> and <c>EnvironmentName</c> from.</param>
    /// <param name="extensions">The file extensions to produce names for (without the leading dot), in order at each position.</param>
    /// <returns>The file names, lowest precedence first.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static IReadOnlyList<string> GetConventionalFileNames(IDovetailContext context, params ReadOnlySpan<string> extensions)
    {
        ArgumentNullException.ThrowIfNull(context);
        var applicationName = context.Get<string>("ApplicationName");
        var environmentName = context.Get<string>("EnvironmentName");

        var names = new List<string>();
        AddNames(names, "appsettings", applicationName, extensions);
        if (environmentName is { Length: > 0, })
        {
            AddNames(names, $"appsettings.{environmentName}", applicationName is { Length: > 0, } ? $"{applicationName}.{environmentName}" : null, extensions);
        }

        AddNames(names, "appsettings.local", applicationName is { Length: > 0, } ? $"{applicationName}.local" : null, extensions);
        return names;

        static void AddNames(List<string> names, string baseName, string? applicationBaseName, ReadOnlySpan<string> extensions)
        {
            foreach (var extension in extensions)
            {
                names.Add($"{baseName}.{extension}");
            }

            if (applicationBaseName is not { Length: > 0, }) return;
            foreach (var extension in extensions)
            {
                names.Add($"{applicationBaseName}.{extension}");
            }
        }
    }
}
