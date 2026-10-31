using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// A joint is only as good as its <c>Register</c> signature, and the docs show a lot of joints. This builds the real thing: an application
/// that references the packed Dovetail packages, with every C# code block of the docs site that implements a joint interface as a source
/// file of its own, so a signature that is not the one the packages generate fails to compile (CS0535, the class does not implement the
/// interface member). The compiler reports the page and the line of the docs, not of a generated file.
/// <see cref="DocumentationExcerptTests" /> accounts for the rest of the <c>Register</c> declarations on the site.
/// </summary>
[TargetFrameworks]
public class DocumentationExampleTests(string targetFramework)
{
    /// <summary>
    /// What a fragment may leave out. A block without <c>using</c> directives of its own is a fragment, so it gets these; a block with
    /// directives is a complete file and gets nothing, and has to compile on its own.
    /// </summary>
    private const string FragmentUsings =
        "using System.Threading; using System.Threading.Tasks; using Joinery.Dovetail; using Joinery.Dovetail.Attributes; using Joinery.Dovetail.Joints; "
      + "using Microsoft.Extensions.Configuration; using Microsoft.Extensions.DependencyInjection; using Microsoft.Extensions.Hosting; "
      + "using Microsoft.Extensions.Logging; using Specular;";

    /// <summary>
    /// Types the examples use and the docs do not define: <c>IInjectData</c> and <c>PrimitiveOptions</c> are what the dependency injection
    /// examples inject, <c>IPlugin</c> is what the Specular example scans for and <c>ICache</c> is what the host lifecycle example warms up.
    /// </summary>
    private const string ExampleTypes = """
        using System.Threading;
        using System.Threading.Tasks;

        public interface IInjectData;

        public class PrimitiveOptions;

        public interface IPlugin;

        public interface ICache
        {
            Task WarmUpAsync(CancellationToken cancellationToken);
        }
        """;

    /// <summary>
    /// The packages the examples are written against, as a reader of the docs would have them: the SDK adds Dovetail and the hosting
    /// package, and the examples that use the Serilog, OpenTelemetry and Specular joints bring those.
    /// </summary>
    private static readonly string[] DovetailPackages = ["Joinery.Dovetail.Serilog", "Joinery.Dovetail.OpenTelemetry"];

    [Test]
    public async Task EveryJointOnTheDocsSite_CompilesAgainstTheInterfacesThePackagesGenerate()
    {
        var examples = DocumentationCodeBlock.All().Where(z => z.ImplementsAJoint && z.ExcerptOf is null).ToArray();
        examples.ShouldNotBeEmpty();
        // The page the examples were wrong on: if the extraction stops finding its joints, the test would pass without compiling them.
        examples.Select(z => z.Path).ShouldContain("docs/src/content/docs/concepts/defining-joints.md");

        using var project = CreateProject();
        project.AddFile("worker/ExampleTypes.cs", ExampleTypes);
        for (var i = 0; i < examples.Length; i++)
        {
            var example = examples[i];
            // #line makes the compiler (and so the failed build) name the page and the line of the docs.
            project.AddFile(
                $"worker/Examples/{i:D2}.cs",
                $"{( example.HasUsings ? "" : FragmentUsings )}\n#line {example.Line} \"{example.Path}\"\n{example.Code}\n#line default\n"
            );
        }

        await project.BuildProject("worker");
    }

    private SdkTestProject CreateProject()
    {
        var worker = ProjectCreator
                    .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                    // The examples implement the OpenTelemetry joints and never start a host, so leave out the check that wants OpenTelemetry.Extensions.Hosting.
                    .Property("DovetailEnableOpenTelemetry", "false")
                    // The examples use nullable annotations, as the code of the repository does.
                    .Property("Nullable", "enable")
                    .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                    .ItemPackageReference("Microsoft.Extensions.Logging", Config.CentralPackageVersion("Microsoft.Extensions.Logging", targetFramework))
                    .ItemPackageReference("Specular", Config.CentralPackageVersion("Specular", targetFramework));
        foreach (var package in DovetailPackages)
        {
            worker.ItemPackageReference(package, Config.DovetailPackageVersion(package));
        }

        return new SdkTestProject()
              .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
              .AddProject("worker/worker.csproj", worker)
              .AddFile("worker/Program.cs", "System.Console.WriteLine(\"hello\");");
    }
}
