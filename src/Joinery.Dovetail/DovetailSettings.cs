using Microsoft.Extensions.Configuration;

namespace Joinery.Dovetail;

/// <summary>
///     Settings that change how Dovetail sets up a host, read from the host's own configuration (environment variables, command line,
///     or anything added before Dovetail runs) rather than from Dovetail's configuration files.
/// </summary>
[PublicAPI]
public static class DovetailSettings
{
    /// <summary>
    ///     The configuration key naming the directory the configuration joints read their files from. Every host that reads configuration
    ///     files from disk honors it. Without it, the joints read from the directory of the entry point file of a file-based app
    ///     (<c>dotnet run Build.cs</c>), and otherwise from the host's own file provider, which the host roots at its content root. A
    ///     relative path is resolved against the current directory.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         As an environment variable it is <c>Dovetail__ConfigurationDirectory</c>; on the command line,
    ///         <c>--Dovetail:ConfigurationDirectory=path</c>.
    ///     </para>
    ///     <para>
    ///         Configuration embedded in the application (<see cref="DovetailEmbeddedConfiguration" />, as MAUI and Uno apps have) still
    ///         wins: the directory only applies where the joints read files from disk. A browser (WebAssembly) host has no file system
    ///         and ignores it.
    ///     </para>
    /// </remarks>
    public const string ConfigurationDirectory = "Dovetail:ConfigurationDirectory";

    private const string EntryPointFileDirectoryPath = nameof(EntryPointFileDirectoryPath);

    /// <summary>
    ///     The directory of the entry point file of a file-based app (<c>dotnet run Build.cs</c>), where its configuration files sit, or
    ///     <see langword="null" /> for any other app.
    /// </summary>
    /// <returns>The directory, or <see langword="null" />.</returns>
    /// <remarks>
    ///     The .NET SDK writes the directory into the runtimeconfig.json, so a published or copied file-based app still names the source
    ///     directory of the machine it was built on; where that doesn't exist, this is <see langword="null" />.
    /// </remarks>
    public static string? GetEntryPointFileDirectory() =>
        AppContext.GetData(EntryPointFileDirectoryPath) is string { Length: > 0, } entryPointFileDirectoryPath
     && Directory.Exists(entryPointFileDirectoryPath)
            ? entryPointFileDirectoryPath
            : null;

    /// <summary>
    ///     The full path of <see cref="ConfigurationDirectory" /> when <paramref name="configuration" /> sets it, or else the
    ///     <c>Dovetail__ConfigurationDirectory</c> environment variable does (for hosts such as MAUI whose configuration doesn't read
    ///     environment variables); otherwise <see langword="null" />.
    /// </summary>
    /// <param name="configuration">The host's configuration.</param>
    /// <returns>The full path of the directory, or <see langword="null" />.</returns>
    public static string? GetConfiguredDirectory(IConfiguration? configuration) =>
        ( configuration?[ConfigurationDirectory] is { Length: > 0, } configured ? configured
        : Environment.GetEnvironmentVariable(EnvironmentVariable) is { Length: > 0, } fromEnvironment ? fromEnvironment
        : null ) is { } directory
            ? Path.GetFullPath(directory)
            : null;

    private const string EnvironmentVariable = "Dovetail__ConfigurationDirectory";
}
