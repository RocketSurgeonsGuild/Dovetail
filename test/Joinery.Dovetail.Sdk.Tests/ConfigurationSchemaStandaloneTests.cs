using System.IO.Compression;
using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// <c>Joinery.Dovetail.ConfigurationSchema*.targets</c> and the build tasks work on their own: a plain Microsoft.NET.Sdk project
/// that imports only those files, taken from the packed Joinery.Dovetail, without its other targets or the Dovetail SDK, writes and
/// copies the schema. Which projects are hosts and whether the host file is copied come from the hook properties
/// (<c>DovetailConfigurationSchemaHost</c>, <c>DovetailConfigurationSchemaCopyToOutput</c>) that Joinery.Dovetail.targets sets.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaStandaloneTests(string targetFramework)
{
    private const string SchemaDirectory = "schema";

    // The schema targets and the tasks they run, and nothing else from the package's build folder.
    private static void ExtractSchemaTargets(SdkTestProject project)
    {
        var version = Config.DovetailPackageVersion("Joinery.Dovetail");
        using var package = ZipFile.OpenRead(Path.Combine(Config.NugetArtifactsDirectory, $"Joinery.Dovetail.{version}.nupkg"));
        foreach (var entry in package.Entries)
        {
            if (!entry.FullName.StartsWith("build/Joinery.Dovetail.ConfigurationSchema", StringComparison.Ordinal)
             && !entry.FullName.StartsWith("build/tasks/", StringComparison.Ordinal))
            {
                continue;
            }

            var destination = Path.Combine(project.Directory, SchemaDirectory, entry.FullName["build/".Length..]);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            entry.ExtractToFile(destination);
        }
    }

    private SdkTestProject CreateProject(string outputType, params (string Name, string Value)[] properties)
    {
        var project = new SdkTestProject();
        ExtractSchemaTargets(project);

        var app = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: outputType);
        foreach (var (name, value) in properties)
        {
            app.Property(name, value);
        }

        app.Import(Path.Combine(project.Directory, SchemaDirectory, "Joinery.Dovetail.ConfigurationSchema.targets"));
        project
           .AddProject("app/app.csproj", app)
           .AddFile("app/Fragments.cs", ConfigurationSchemaTests.AppFragment);
        if (outputType == "Exe")
        {
            project.AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        }

        return project;
    }

    [Test]
    public async Task Application_WritesTheHostSchema_AndCopiesItToItsOutput()
    {
        using var project = CreateProject("Exe");

        await project.BuildProject("app");

        var schema = project.ReadFile("app/obj/appsettings.schema.json");
        await schema.Should().Contain("\"App\"");
        await project.ReadFile($"app/bin/Debug/{targetFramework}/appsettings.schema.json").Should().BeEqualTo(schema);
    }

    [Test]
    public async Task CopyToOutputFalse_WritesTheHostSchema_ButLeavesItOutOfTheOutput()
    {
        using var project = CreateProject("Exe", ("DovetailConfigurationSchemaCopyToOutput", "false"));

        await project.BuildProject("app");

        await project.ReadFile("app/obj/appsettings.schema.json").Should().Contain("\"App\"");
        await File.Exists(Path.Combine(project.Directory, $"app/bin/Debug/{targetFramework}/appsettings.schema.json")).Should().BeFalse();
    }

    [Test]
    public async Task HostTrue_MakesALibraryAHost()
    {
        using var project = CreateProject("Library", ("DovetailConfigurationSchemaHost", "true"));

        await project.BuildProject("app");

        await project.ReadFile("app/obj/appsettings.schema.json").Should().Contain("\"App\"");
        await File.Exists(Path.Combine(project.Directory, $"app/obj/Debug/{targetFramework}/ConfigurationSchema.json")).Should().BeFalse();
    }

    [Test]
    public async Task Library_WritesItsOwnFragments()
    {
        using var project = CreateProject("Library");

        await project.BuildProject("app");

        await project.ReadFile($"app/obj/Debug/{targetFramework}/ConfigurationSchema.json").Should().Contain("\"App\"");
        await File.Exists(Path.Combine(project.Directory, "app/obj/appsettings.schema.json")).Should().BeFalse();
    }
}
