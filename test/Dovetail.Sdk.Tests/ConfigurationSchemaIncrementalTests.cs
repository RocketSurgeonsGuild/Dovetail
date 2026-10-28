using Microsoft.Build.Utilities.ProjectCreation;
using Task = System.Threading.Tasks.Task;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// The schema across builds: an up-to-date one isn't merged again (or touched), and the files are deleted once a project stops
/// writing them - when the schema is turned off, or a library no longer declares any section.
/// </summary>
public class ConfigurationSchemaIncrementalTests
{
    private const string TargetFramework = "net10.0";

    [Test]
    public async Task Host_SkipsAnUpToDateSchema_AndDeletesItWhenTurnedOff()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("host/host.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: TargetFramework, outputType: "Exe"))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", ConfigurationSchemaTests.AppFragment);
        var schema = Path.Combine(project.Directory, "host/obj/appsettings.schema.json");
        var output = Path.Combine(project.Directory, $"host/bin/Debug/{TargetFramework}/appsettings.schema.json");

        Merges(await project.BuildProject("host")).ShouldBeTrue();
        var written = File.GetLastWriteTimeUtc(schema);
        File.Exists(output).ShouldBeTrue();

        Merges(await project.BuildProject("host")).ShouldBeFalse();
        File.GetLastWriteTimeUtc(schema).ShouldBe(written);

        await project.BuildProject("host", "-p:DovetailConfigurationSchema=false");
        File.Exists(schema).ShouldBeFalse();
        File.Exists(output).ShouldBeFalse();

        Merges(await project.BuildProject("host")).ShouldBeTrue();
        File.Exists(schema).ShouldBeTrue();
        File.Exists(output).ShouldBeTrue();
    }

    [Test]
    public async Task Library_DeletesItsSchema_WhenItNoLongerDeclaresSections()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: TargetFramework))
           .AddFile("lib/Fragments.cs", ConfigurationSchemaTests.GreetingsFragment);
        var schema = Path.Combine(project.Directory, $"lib/obj/Debug/{TargetFramework}/ConfigurationSchema.json");

        await project.BuildProject("lib");
        File.Exists(schema).ShouldBeTrue();

        File.Delete(Path.Combine(project.Directory, "lib/Fragments.cs"));
        project.AddFile("lib/Class1.cs", "namespace Lib { public class Class1 { } }");
        await project.BuildProject("lib");
        File.Exists(schema).ShouldBeFalse();
    }

    private static bool Merges(Microsoft.Build.Logging.StructuredLogger.Build build) => build.FindChildrenRecursive<Microsoft.Build.Logging.StructuredLogger.Task>(z => z.Name == "MergeConfigurationSchema").Count > 0;
}
