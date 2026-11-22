using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// An application's configuration joints read its files where the host does (its content root, which <c>dotnet run</c> leaves at the
/// current directory), but the build copies a library's files, renamed after the library, to the application's output directory only.
/// So a library's joint that doesn't find its base file where the host looks reads it from the output directory, and only the
/// application's own joint looks for <c>appsettings.*</c>, which in the application's project directory are the application's files.
/// These run an application that references a configured library, as a project or as a package, with <c>dotnet run</c> from a few
/// directories.
/// </summary>
[TargetFrameworks]
public class RunWorkingDirectoryTests(string targetFramework)
{
    /// <summary>
    /// The library ships <c>appsettings.yaml</c> and <c>appsettings.Development.yaml</c>, copied to the application's output as
    /// <c>{Library}.yaml</c> and <c>{Library}.Development.yaml</c>: through the <c>ProjectReference</c>, or, when
    /// <paramref name="packaged"/>, packed and restored from the package's <c>contentFiles</c>.
    /// </summary>
    private static async Task<SdkTestProject> CreateApplicationWithAConfiguredLibrary(string targetFramework, bool packaged)
    {
        var project = new SdkTestProject();
        project.AddProject(
            "Directory.Build.props",
            ProjectCreator
               .Create()
               .Sdk("Joinery.Dovetail.Sdk")
               .Property("DovetailEnableConfiguration", "true")
        );

        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests). A distinctive name,
        // as a package id that can't collide with a public package on nuget.org.
        var lib = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project
           .AddProject("lib/DovetailRunTestLibrary.csproj", lib, track: false)
           .AddFile("lib/appsettings.yaml", "Library: from lib\nShared: from lib")
           .AddFile("lib/appsettings.Development.yaml", "LibraryEnvironment: from lib.Development");

        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework));
        if (packaged)
        {
            var package = await project.PackToLocalFeed("lib");
            app.ItemInclude("PackageReference", include: package.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = package.Version });
        }
        else
        {
            app.ItemProjectReference(lib);
        }

        project
           .AddProject("app/app.csproj", app)
           .AddFile("app/appsettings.yaml", "Application: from app\nShared: from app")
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using Microsoft.Extensions.Configuration;
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.Hosting;

                var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, EnvironmentName = "Development" });
                using var host = await builder.ConfigureDovetail();
                var configuration = host.Services.GetRequiredService<IConfiguration>();
                foreach (var key in new[] { "Library", "LibraryEnvironment", "Application", "Shared" })
                {
                    System.Console.WriteLine(key + "=" + configuration[key]);
                }
                """
            );
        return project;
    }

    /// <summary>
    /// From another directory (<c>dotnet run --project app</c>), the content root holds neither project's files: both are read from the
    /// output directory, where the build copied them as <c>app.yaml</c> and <c>DovetailRunTestLibrary.yaml</c>.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunFromAnotherDirectory_ReadsTheFilesTheBuildCopied(bool packaged)
    {
        using var project = await CreateApplicationWithAConfiguredLibrary(targetFramework, packaged);

        var output = await project.Run("app", workingDirectory: ".");

        await output.Should().Contain("Library=from lib");
        await output.Should().Contain("LibraryEnvironment=from lib.Development");
        await output.Should().Contain("Application=from app");
        await output.Should().Contain("Shared=from app");
    }

    /// <summary>
    /// From the application's own directory, the application reads its <c>appsettings.yaml</c> there, and the library, whose file isn't
    /// there, reads its own from the output directory rather than the application's <c>appsettings.yaml</c>.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunFromTheProjectDirectory_ReadsTheApplicationsSourceFile_AndTheLibrarysCopiedFile(bool packaged)
    {
        using var project = await CreateApplicationWithAConfiguredLibrary(targetFramework, packaged);

        var output = await project.Run("app");

        await output.Should().Contain("Library=from lib");
        await output.Should().Contain("LibraryEnvironment=from lib.Development");
        await output.Should().Contain("Application=from app");
        await output.Should().Contain("Shared=from app");
    }

    /// <summary>
    /// <c>Dovetail:ConfigurationDirectory</c> still wins for the files it holds; a library whose files it doesn't hold reads them from the
    /// output directory.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ConfigurationDirectory_WinsForTheFilesItHolds(bool packaged)
    {
        using var project = await CreateApplicationWithAConfiguredLibrary(targetFramework, packaged);
        project.AddFile("config/app.yaml", "Application: from config\nShared: from config");

        var output = await project.Run("app", "--Dovetail:ConfigurationDirectory=config", workingDirectory: ".");

        await output.Should().Contain("Library=from lib");
        await output.Should().Contain("Application=from config");
        await output.Should().Contain("Shared=from config");
    }
}
