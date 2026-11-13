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
