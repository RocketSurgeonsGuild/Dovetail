using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.Tests;

/// <summary>
///     A file-based app (<c>dotnet run Build.cs</c>) keeps its configuration files next to its entry point file, which the .NET SDK names in
///     the <c>EntryPointFileDirectoryPath</c> AppContext data; its output directory is somewhere else entirely. The configuration joints read
///     their files from there.
/// </summary>
[NotInParallel("EntryPointFileDirectoryPath")]
public class FileBasedAppConfigurationTests
{
    [Test]
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheDirectoryOfTheEntryPointFile()
    {
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: entry-point\n");
            AppContext.SetData("EntryPointFileDirectoryPath", directory.FullName);

            using var host = await Host.CreateApplicationBuilder().ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].ShouldBe("entry-point");
        }
        finally
        {
            AppContext.SetData("EntryPointFileDirectoryPath", null);
            directory.Delete(true);
        }
    }

    /// <summary>
    ///     <see cref="DovetailSettings.ConfigurationDirectory" /> wins over the directory of the entry point file.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheConfigurationDirectorySetting()
    {
        var entryPointDirectory = Directory.CreateTempSubdirectory();
        var configurationDirectory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(entryPointDirectory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: entry-point\n");
            await File.WriteAllTextAsync(Path.Combine(configurationDirectory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: setting\n");
            AppContext.SetData("EntryPointFileDirectoryPath", entryPointDirectory.FullName);
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { [DovetailSettings.ConfigurationDirectory] = configurationDirectory.FullName, }
            );

            using var host = await builder.ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].ShouldBe("setting");
        }
        finally
        {
            AppContext.SetData("EntryPointFileDirectoryPath", null);
            configurationDirectory.Delete(true);
            entryPointDirectory.Delete(true);
        }
    }

    /// <summary>
    ///     <see cref="DovetailSettings.ConfigurationDirectory" /> applies to any app, not only a file-based one.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheConfigurationDirectorySetting_OutsideAFileBasedApp()
    {
        var configurationDirectory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(configurationDirectory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: setting\n");
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(
                new Dictionary<string, string?> { [DovetailSettings.ConfigurationDirectory] = configurationDirectory.FullName, }
            );

            using var host = await builder.ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].ShouldBe("setting");
        }
        finally
        {
            configurationDirectory.Delete(true);
        }
    }

    /// <summary>
    ///     Outside a file-based app the joints read from the host's own file provider, which the host roots at its content root.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_ReadsTheFilesOfTheConfigurationJoints_FromTheContentRoot_OutsideAFileBasedApp()
    {
        var contentRoot = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(contentRoot.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: content-root\n");

            using var host = await Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { ContentRootPath = contentRoot.FullName, }).ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].ShouldBe("content-root");
        }
        finally
        {
            contentRoot.Delete(true);
        }
    }

    /// <summary>
    ///     A base path set on the host's configuration wins over the directory of the entry point file.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_KeepsTheBasePathOfTheHostConfiguration_InAFileBasedApp()
    {
        var entryPointDirectory = Directory.CreateTempSubdirectory();
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(entryPointDirectory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: entry-point\n");
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "appsettings.yaml"), "Dovetail:\n  FromFile: base-path\n");
            AppContext.SetData("EntryPointFileDirectoryPath", entryPointDirectory.FullName);
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.SetBasePath(directory.FullName);

            using var host = await builder.ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>()["Dovetail:FromFile"].ShouldBe("base-path");
        }
        finally
        {
            AppContext.SetData("EntryPointFileDirectoryPath", null);
            directory.Delete(true);
            entryPointDirectory.Delete(true);
        }
    }

    /// <summary>
    ///     The SDK writes the directory into the runtimeconfig.json, so a published or copied file-based app still names the source directory
    ///     of the machine it was built on. Where that doesn't exist, the joints read the output directory, as they would anywhere else.
    /// </summary>
    [Test]
    public async Task ConfigureDovetail_Starts_WhenTheDirectoryOfTheEntryPointFileDoesNotExist()
    {
        AppContext.SetData("EntryPointFileDirectoryPath", Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        try
        {
            using var host = await Host.CreateApplicationBuilder().ConfigureDovetail();

            host.Services.GetRequiredService<IConfiguration>().ShouldNotBeNull();
        }
        finally
        {
            AppContext.SetData("EntryPointFileDirectoryPath", null);
        }
    }
}
