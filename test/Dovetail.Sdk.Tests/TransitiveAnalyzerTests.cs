using Microsoft.Build.Utilities.ProjectCreation;
using NuGet.Packaging;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Dovetail's integration packages let the analyzer flow through their dependency on Dovetail, so a project that only
/// references (say) Dovetail.Hosting - no Dovetail.Sdk, no direct Dovetail reference - still runs the generator. What it
/// emits then follows the same rules as everywhere else: applications get an Imports class, libraries don't, and a
/// project pinned below C# 12 gets a warning instead of code it can't compile. EmitCompilerGeneratedFiles makes the
/// generator's output visible in the GeneratedFiles snapshot.
/// </summary>
[TargetFrameworks]
public class TransitiveAnalyzerTests(string targetFramework)
{
    private SdkTestProject CreateHostingOnlyProject(string? outputType, string? langVersion = null)
    {
        var project = ProjectCreator
                     .Templates.SdkCsproj(targetFramework: targetFramework, outputType: outputType)
                     .Property("EmitCompilerGeneratedFiles", "true")
                     .ItemInclude(
                          "PackageReference",
                          include: "Dovetail.Hosting",
                          metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Dovetail.Hosting") }
                      );
        if (langVersion is not null)
        {
            project.Property("LangVersion", langVersion);
        }

        var scaffold = new SdkTestProject();
        scaffold.AddProject("app/app.csproj", project);
        if (outputType is "Exe")
        {
            scaffold.AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        }

        return scaffold;
    }

    [Test]
    public async Task HostingOnlyApplication_GetsGeneratedImports()
    {
        using var project = CreateHostingOnlyProject("Exe");

        await project.VerifyProjects();
    }

    [Test]
    public async Task HostingOnlyLibrary_GetsNoGeneratedImports()
    {
        using var project = CreateHostingOnlyProject(outputType: null);

        await project.VerifyProjects();
    }

    [Test]
    public async Task HostingOnlyProject_BelowCSharp12_WarnsInsteadOfGenerating()
    {
        using var project = CreateHostingOnlyProject("Exe", langVersion: "11");

        await project.VerifyBuildWarnings("app");
    }

    /// <summary>
    /// The packed dependency on Dovetail used to carry NuGet's default exclude="Build,Analyzers", which kept the generator
    /// out of every transitive consumer. Only build assets stay private now.
    /// </summary>
    [Test]
    public void PackedDependenciesOnDovetail_DoNotExcludeAnalyzers()
    {
        var excludes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var nupkg in Directory.EnumerateFiles(Config.NugetArtifactsDirectory, "Dovetail.*.nupkg"))
        {
            using var reader = new PackageArchiveReader(nupkg);
            var packageId = reader.GetIdentity().Id;
            foreach (var group in reader.NuspecReader.GetDependencyGroups())
            {
                foreach (var dependency in group.Packages.Where(z => z.Id == "Dovetail"))
                {
                    excludes[$"{packageId} ({group.TargetFramework.GetShortFolderName()})"] = string.Join(",", dependency.Exclude);
                }
            }
        }

        excludes.ShouldNotBeEmpty();
        excludes.Where(z => z.Value.Contains("Analyzers", StringComparison.OrdinalIgnoreCase)).ShouldBeEmpty();
        excludes.Values.ShouldAllBe(z => z == "Build");
    }
}
