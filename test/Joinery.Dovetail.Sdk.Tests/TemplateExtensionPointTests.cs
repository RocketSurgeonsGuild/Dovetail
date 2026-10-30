using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Any package can ship <c>*.cs.template</c> files and have Joinery.Dovetail write them into the projects that should get them, by
/// declaring <c>DovetailTemplate</c> items (<c>src/Joinery.Dovetail/build/Joinery.Dovetail.Templates.targets</c>), the way
/// Joinery.Dovetail.Hosting does for its helpers (<see cref="HostingHelperTests" /> covers what it emits). These pack a package that
/// ships templates, reference it from projects that declare the reference themselves and from one that only receives it
/// transitively, and read which templates landed in each project's <c>obj/</c>.
/// </summary>
[TargetFrameworks]
public class TemplateExtensionPointTests(string targetFramework)
{
    private const string ShipperId = "DovetailTemplateShipper";

    /// <summary>
    /// <c>DirectReferenceOnly</c> limits a template to projects that reference its package themselves. <c>transitive</c> references
    /// <c>middle</c>, which references the package: the package's assembly and build assets reach it (the template is even declared
    /// there), but it gets nothing, while both projects with a reference of their own get the template.
    /// </summary>
    [Test]
    public async Task DirectReferenceOnly_EmitsIntoDirectReferences_AndNothingIntoATransitiveOne()
    {
        using var project = await CreatePackagedScaffold(new Template("DirectOnly", """DirectReferenceOnly="true" """));

        await project.BuildProject("transitive");
        await project.BuildProject("direct");

        EmittedTemplates(project, "direct").ShouldBe(["DirectOnly"]);
        EmittedTemplates(project, "middle").ShouldBe(["DirectOnly"]);

        // transitive really did receive the package, so receiving nothing is the mode's doing and not a project that never saw it.
        File.Exists(Path.Combine(project.Directory, "transitive", "bin", "Debug", targetFramework, $"{ShipperId}.dll")).ShouldBeTrue();
        ( await project.EvaluateItemIdentities("transitive", "DovetailTemplate") ).ShouldContain("DirectOnly");
        EmittedTemplates(project, "transitive").ShouldBeEmpty();
    }

    /// <summary>
    /// What NuGet gives for free, and why the mode exists: a package's <c>build/</c> assets are only imported into projects that
    /// reference it directly, so a package that ships its templates from there never declares them in a transitive reference. The mode
    /// is for what that can't express: a package that ships from <c>buildTransitive/</c> (the Dovetail integration packages do, so
    /// projects that get them through another package still get their templates) and projects that consume it by
    /// <c>ProjectReference</c>, which import no build assets at all (see the test below).
    /// </summary>
    [Test]
    public async Task TemplatesShippedFromBuild_AreNotDeclaredInATransitiveReference_WithoutTheMode()
    {
        using var project = await CreatePackagedScaffold("build", new Template("Plain"));

        await project.BuildProject("transitive");
        await project.BuildProject("direct");

        EmittedTemplates(project, "direct").ShouldBe(["Plain"]);
        EmittedTemplates(project, "middle").ShouldBe(["Plain"]);
        ( await project.EvaluateItemIdentities("transitive", "DovetailTemplate") ).ShouldNotContain("Plain");
        EmittedTemplates(project, "transitive").ShouldBeEmpty();
    }

    /// <summary>
    /// The other gates, and the transitive reference that does receive a template that isn't <c>DirectReferenceOnly</c>: the package's
    /// assembly is among its references, which is all the default asks. <c>Hosts</c> asks for one of the hosts the Dovetail SDK
    /// detected, <c>References</c> for every assembly named.
    /// </summary>
    [Test]
    public async Task Gates_DecideWhichProjectsGetATemplate()
    {
        using var project = await CreatePackagedScaffold(
            new Template("Referenced"),
            new Template("RequiredAssemblyReferenced", """References="Joinery.Dovetail" """),
            new Template("RequiredAssemblyMissing", """References="Joinery.Dovetail;Not.Referenced.Anywhere" """),
            new Template("ForTheGenericHost", """Hosts="HostApplication" """),
            new Template("ForOtherHosts", """Hosts="WebApplication;WebAssembly" """)
        );

        await project.BuildProject("transitive");
        await project.BuildProject("direct");

        // Both are applications, so the SDK detects the generic host in each; the library has no host.
        string[] application = ["ForTheGenericHost", "Referenced", "RequiredAssemblyReferenced"];
        EmittedTemplates(project, "direct").ShouldBe(application);
        EmittedTemplates(project, "transitive").ShouldBe(application);
        EmittedTemplates(project, "middle").ShouldBe(["Referenced", "RequiredAssemblyReferenced"]);
    }

    /// <summary>
    /// A project reference is a direct reference to the project's package, which is how this repository (and LaunchPad's samples)
    /// consume what they ship: the templates are declared in a <c>Directory.Build.targets</c> instead of a package's, and the project
    /// is named after the package.
    /// </summary>
    [Test]
    public async Task DirectReferenceOnly_CountsProjectReferences_ButNotTheirTransitiveReferences()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddFile("Directory.Build.targets", $"<Project><ItemGroup>{Declare(new Template("InRepository", """DirectReferenceOnly="true" """))}</ItemGroup></Project>")
           .AddFile("InRepository.cs.template", TemplateSource("InRepository"));
        var shipper = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project
           .AddProject($"shipper/{ShipperId}.csproj", shipper, track: false)
           .AddFile("shipper/Shipper.cs", "namespace DovetailTemplateShipper; public static class Marker { }");
        var direct = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe").ItemProjectReference(shipper);
        project
           .AddProject("direct/direct.csproj", direct, track: false)
           .AddFile("direct/Program.cs", "System.Console.WriteLine(\"hello\");");
        project
           .AddProject("transitive/transitive.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe").ItemProjectReference(direct), track: false)
           .AddFile("transitive/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.BuildProject("transitive");

        EmittedTemplates(project, "direct").ShouldBe(["InRepository"]);
        EmittedTemplates(project, "transitive").ShouldBeEmpty();
        EmittedTemplates(project, "shipper").ShouldBeEmpty();
    }

    private sealed record Template(string Name, string Attributes = "");

    private static string Declare(Template template) =>
        $"""<DovetailTemplate Include="{template.Name}" File="$(MSBuildThisFileDirectory){template.Name}.cs.template" Package="{ShipperId}" {template.Attributes}/>""";

    private static string TemplateSource(string name) => $"namespace {ShipperId}.Emitted;{Environment.NewLine}{Environment.NewLine}internal static class {name} {{ }}{Environment.NewLine}";

    private Task<SdkTestProject> CreatePackagedScaffold(params Template[] templates) => CreatePackagedScaffold("buildTransitive", templates);

    /// <summary>
    /// A package that ships the templates from the <paramref name="assetFolder" /> of its build assets, and three projects: <c>direct</c>
    /// (an application) and <c>middle</c> (a library) reference it, and <c>transitive</c> (an application) only references
    /// <c>middle</c>. Every project applies the Dovetail SDK, so each runs the Dovetail generator whatever else reaches it.
    /// </summary>
    private async Task<SdkTestProject> CreatePackagedScaffold(string assetFolder, params Template[] templates)
    {
        var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                $"shipper/{ShipperId}.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .ItemInclude("None", "build/*.*", metadata: new Dictionary<string, string?> { ["Pack"] = "true", ["PackagePath"] = assetFolder }),
                track: false
            )
           .AddFile("shipper/Shipper.cs", "namespace DovetailTemplateShipper; public static class Marker { }")
           .AddFile($"shipper/build/{ShipperId}.targets", $"<Project><ItemGroup>{string.Join(Environment.NewLine, templates.Select(Declare))}</ItemGroup></Project>");
        foreach (var template in templates)
        {
            project.AddFile($"shipper/build/{template.Name}.cs.template", TemplateSource(template.Name));
        }

        var shipper = await project.PackToLocalFeed("shipper");
        shipper.Files.ShouldContain($"{assetFolder}/{ShipperId}.targets");

        var reference = new Dictionary<string, string?> { ["Version"] = shipper.Version };
        var middle = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework).ItemInclude("PackageReference", shipper.PackageId, metadata: reference);
        project
           .AddProject("middle/middle.csproj", middle, track: false)
           .AddFile("middle/Middle.cs", "namespace Middle; public static class Marker { }");
        project
           .AddProject(
                "direct/direct.csproj",
                ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe").ItemInclude("PackageReference", shipper.PackageId, metadata: reference),
                track: false
            )
           .AddFile("direct/Program.cs", "System.Console.WriteLine(\"hello\");");
        project
           .AddProject("transitive/transitive.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe").ItemProjectReference(middle), track: false)
           .AddFile("transitive/Program.cs", "System.Console.WriteLine(\"hello\");");
        return project;
    }

    private static IReadOnlyList<string> EmittedTemplates(SdkTestProject project, string relativePath) =>
        Directory
           .EnumerateFiles(Path.Combine(project.Directory, relativePath, "obj"), "DovetailTemplate.*.g.cs", SearchOption.AllDirectories)
           .Select(Path.GetFileName)
           .OfType<string>()
           .Select(z => z["DovetailTemplate.".Length..^".g.cs".Length])
           .Order(StringComparer.Ordinal)
           .ToArray();
}
