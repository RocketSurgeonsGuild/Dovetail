using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
///     Joinery.Dovetail.Configuration.Yaml ships its joints (<c>YamlJoint</c>, <c>YamlBrowserJoint</c> and <c>YamlEmbeddedJoint</c>)
///     as <c>DovetailTemplate</c> items (<c>src/Joinery.Dovetail.Configuration.Yaml/build/Joinery.Dovetail.Configuration.Yaml.targets</c>),
///     declared only in applications and test projects. A library's copy would be exported to every application that references it, and
///     each copy would load the application's YAML files again, so a library gets none and the application gets exactly one of each.
/// </summary>
[TargetFrameworks]
public class YamlJointTemplateTests(string targetFramework)
{
    private static readonly string[] YamlJoints = ["YamlBrowserJoint", "YamlEmbeddedJoint", "YamlJoint"];

    [Test]
    public async Task Library_ReferencingTheYamlPackage_GetsNoYamlJoint()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));
        project.AddProject("lib/lib.csproj", YamlLibrary());

        await project.BuildProject("lib");

        await YamlJointFiles(project, "lib").Should().BeEmpty();
    }

    /// <summary>
    ///     The application gets the joints from the library's reference to the package, and loads its <c>appsettings.yaml</c> once:
    ///     through its own <c>YamlJoint</c>, not again through a copy the library exported.
    /// </summary>
    [Test]
    public async Task Application_ReferencingALibraryWithTheYamlPackage_GetsOneOfEachJoint_AndLoadsItsYamlOnce()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));
        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests).
        var lib = YamlLibrary();
        project.AddProject("lib/lib.csproj", lib, track: false);

        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"))
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                 // The SDK's own per-project joint (Dovetail.YamlConfiguration.g.cs) also adds appsettings.yaml; leave it out so the count
                 // is the YAML package's joints alone.
                 .Property("DovetailEnableYamlConfiguration", "false")
                 .ItemProjectReference(lib);
        project
           .AddProject("app/app.csproj", app)
           .AddFile("app/appsettings.yaml", "Greeting: hello from yaml\n")
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using System.Linq;
                using Joinery.Dovetail;
                using Joinery.Dovetail.Configuration.Yaml;
                using Microsoft.Extensions.Configuration;
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.Hosting;

                var builder = Host.CreateApplicationBuilder(args);
                using var host = await builder.ConfigureDovetail();
                var configuration = host.Services.GetRequiredService<IConfiguration>();
                System.Console.WriteLine("greeting=" + configuration["Greeting"]);
                System.Console.WriteLine("appsettings.yaml providers=" + CountYaml((IConfigurationRoot)configuration));

                // Through the chained configurations too, wherever the joints added their sources.
                static int CountYaml(IConfigurationRoot root) =>
                    root.Providers.Sum(
                        provider => provider switch
                        {
                            YamlConfigurationProvider yaml when yaml.Source.Path == "appsettings.yaml" => 1,
                            Microsoft.Extensions.Configuration.ChainedConfigurationProvider { Configuration: IConfigurationRoot chained } => CountYaml(chained),
                            _ => 0,
                        }
                    );
                """
            );

        var lines = ( await project.Run("app") ).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        await YamlJointFiles(project, "app").Should().BeEquivalentTo(YamlJoints, CollectionOrdering.Matching);
        await YamlJointFiles(project, "lib").Should().BeEmpty();
        await lines.Should().Contain("greeting=hello from yaml");
        await lines.Single(line => line.StartsWith("appsettings.yaml providers=", StringComparison.Ordinal)).Should().BeEqualTo("appsettings.yaml providers=1");
    }

    /// <summary>
    ///     Test projects import joints like applications do, whatever their output type, so they get the joints too.
    /// </summary>
    [Test]
    public async Task TestProject_ReferencingALibraryWithTheYamlPackage_GetsOneOfEachJoint()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));
        var lib = YamlLibrary();
        project.AddProject("lib/lib.csproj", lib, track: false);
        project.AddProject(
            "tests/tests.csproj",
            ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework).Property("IsTestProject", "true").ItemProjectReference(lib)
        );

        await project.BuildProject("tests");

        await YamlJointFiles(project, "tests").Should().BeEquivalentTo(YamlJoints, CollectionOrdering.Matching);
        await YamlJointFiles(project, "lib").Should().BeEmpty();
    }

    private ProjectCreator YamlLibrary() =>
        ProjectCreator
           .Templates.SdkCsproj(targetFramework: targetFramework)
           .ItemPackageReference("Joinery.Dovetail.Configuration.Yaml", Config.DovetailPackageVersion("Joinery.Dovetail.Configuration.Yaml"));

    private static IReadOnlyList<string> YamlJointFiles(SdkTestProject project, string relativePath)
    {
        var obj = Path.Combine(project.Directory, relativePath, "obj");
        return Directory.Exists(obj)
            ? Directory
             .EnumerateFiles(obj, "DovetailConfigurationYaml.*.g.cs", SearchOption.AllDirectories)
             .Select(Path.GetFileName)
             .OfType<string>()
             .Select(z => z["DovetailConfigurationYaml.".Length..^".g.cs".Length])
             .Order(StringComparer.Ordinal)
             .ToArray()
            : [];
    }
}
