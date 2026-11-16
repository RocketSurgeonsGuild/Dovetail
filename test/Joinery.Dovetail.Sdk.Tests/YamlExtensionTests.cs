using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// YAML has two extensions and the generated configuration joint takes either: a library's base file can be <c>.yaml</c> or <c>.yml</c>
/// (only one is required), and its environment and local layers are read in either extension, whatever the base file's is.
/// </summary>
[TargetFrameworks]
public class YamlExtensionTests(string targetFramework)
{
    private const string Program = """
        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.Hosting;
        using host;

        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { Args = args, EnvironmentName = "Development" });
        using var app = await builder.ConfigureDovetail();
        System.Console.WriteLine("Base=" + builder.Configuration["Lib:Base"]);
        System.Console.WriteLine("Greeting=" + builder.Configuration["Lib:Greeting"]);
        """;

    [Test]
    [Arguments("yml", "yaml")]
    [Arguments("yaml", "yml")]
    [Arguments("yml", "yml")]
    public async Task Library_WithEitherBaseExtension_LoadsItsEnvironmentLayerInEitherExtension(string baseExtension, string environmentExtension)
    {
        using var project = new SdkTestProject();
        project.AddProject(
            "Directory.Build.props",
            ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk").Property("DovetailEnableConfiguration", "true")
        );

        // track: false - built only through the host's ProjectReference (see HostConfigurationPropagationTests).
        var lib = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project
           .AddProject("lib/lib.csproj", lib, track: false)
           .AddFile($"lib/appsettings.{baseExtension}", "Lib:\n  Base: from-base\n  Greeting: base")
           .AddFile($"lib/appsettings.Development.{environmentExtension}", "Lib:\n  Greeting: development");

        var host = ProjectCreator
                  .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                  .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                  .ItemProjectReference(lib);
        project.AddProject("host/host.csproj", host).AddFile("host/Program.cs", Program);

        var output = await project.PublishAndRun("host");

        await output.Should().Contain("Base=from-base");
        await output.Should().Contain("Greeting=development");
    }
}
