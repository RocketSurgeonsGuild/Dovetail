using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// <c>[assembly: DovetailConfiguration&lt;T&gt;("Section")]</c> describes a type from a package with that package's XML docs. The
/// compiler loads no docs for references and analyzers can't read files, so <c>Dovetail.targets</c> passes the doc file next to
/// each package reference to the generator as an <c>AdditionalFiles</c> item. This checks that wiring in a real build.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaFragmentTests(string targetFramework)
{
    [Test]
    public async Task PackageType_IsDescribedWithItsXmlDocs()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Dovetail.Sdk")
            )
           .AddProject(
                "app/app.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .Property("EmitCompilerGeneratedFiles", "true")
            )
           .AddFile(
                "app/Program.cs",
                """
                [assembly: Dovetail.Attributes.DovetailConfiguration<Microsoft.Extensions.Hosting.HostOptions>("Host")]

                System.Console.WriteLine("hello");
                """
            );

        await project.Run("app");

        var fragment = File.ReadAllText(
            Directory.EnumerateFiles(Path.Combine(project.Directory, "app", "obj"), "Dovetail_ConfigurationSchema.g.cs", SearchOption.AllDirectories).Single()
        );
        fragment.ShouldContain("Dovetail.ConfigurationSchema.v1:Host");
        // HostOptions.ShutdownTimeout's <summary>, from Microsoft.Extensions.Hosting.xml in the package.
        fragment.ShouldContain("Gets or sets the default timeout for IHost.StopAsync.");
    }
}
