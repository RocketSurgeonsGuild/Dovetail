using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers <c>Dovetail.ConfigurationSchema.targets</c>: libraries write their own configuration schema fragments to
/// <c>obj/.../ConfigurationSchema.json</c> and pack it at the package root, and hosts merge theirs and every reference's into
/// <c>obj/appsettings.schema.json</c>, copied to the output directory and published. The fragments are hand-written
/// <c>[assembly: AssemblyMetadata("Dovetail.ConfigurationSchema.v1:...", ...)]</c> attributes in the generator's shape, so these
/// don't depend on the generator.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaTests(string targetFramework)
{
    /// <summary>
    /// "Greetings", with an object type in the fragment's definitions.
    /// </summary>
    internal const string GreetingsFragment = """"
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","title":"Greetings","description":"Options of the greeter.","properties":{"Message":{"type":"string","description":"Text placed before the name."},"Endpoint":{"type":["object","null"],"allOf":[{"$ref":"#/definitions/Endpoint"}]}},"patternProperties":{"^(?:[Mm][Ee][Ss][Ss][Aa][Gg][Ee])$":{"type":"string","description":"Text placed before the name."},"^(?:[Ee][Nn][Dd][Pp][Oo][Ii][Nn][Tt])$":{"type":["object","null"],"allOf":[{"$ref":"#/definitions/Endpoint"}]}},"definitions":{"Endpoint":{"properties":{"Url":{"type":"string"}},"patternProperties":{"^(?:[Uu][Rr][Ll])$":{"type":"string"}}}}}""")]
        """";

    /// <summary>
    /// "greetings" again (another library binding the same section), and a nested "Shared:Http".
    /// </summary>
    internal const string SharedFragments = """"
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:greetings", """{"type":"object","title":"greetings","properties":{"Count":{"type":"integer"}},"patternProperties":{"^(?:[Cc][Oo][Uu][Nn][Tt])$":{"type":"integer"}}}""")]
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:Shared:Http", """{"type":"object","title":"Shared:Http","properties":{"Timeout":{"type":"integer"}},"patternProperties":{"^(?:[Tt][Ii][Mm][Ee][Oo][Uu][Tt])$":{"type":"integer"}}}""")]
        """";

    /// <summary>
    /// The application's own section.
    /// </summary>
    internal const string AppFragment = """"
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:App", """{"type":"object","title":"App","properties":{"Name":{"type":"string"}},"patternProperties":{"^(?:[Nn][Aa][Mm][Ee])$":{"type":"string"}}}""")]
        """";

    private const string HostSchema = "host/obj/appsettings.schema.json";

    [Test]
    public async Task Library_WritesItsSectionsToObj_AndPacksThemAtThePackageRoot()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib/Fragments.cs", GreetingsFragment + "\n" + SharedFragments);

        var objSchema = $"lib/obj/Release/{targetFramework}/ConfigurationSchema.json";
        await project.VerifyProjectsAndPackages(objSchema);

        project.ReadPackageFile("lib", "ConfigurationSchema.json").ShouldBe(project.ReadFile(objSchema));
    }

    /// <summary>
    /// An appsettings.schema.json next to the configuration (say, one copied there for editors) is not configuration: it isn't
    /// renamed, copied, packed or embedded like appsettings*.json.
    /// </summary>
    private SdkTestProject CreateLibraryWithASchemaFile()
    {
        var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib/appsettings.json", "{}")
           .AddFile("lib/appsettings.schema.json", "{}");
        return project;
    }

    [Test]
    public async Task ASchemaFileNextToTheConfiguration_IsNotPackedLikeIt()
    {
        using var project = CreateLibraryWithASchemaFile();

        await project.VerifyProjectsAndPackages();
    }

    [Test]
    public async Task ASchemaFileNextToTheConfiguration_IsNotEmbeddedLikeIt()
    {
        using var project = CreateLibraryWithASchemaFile();

        await project.VerifyEmbeddedConfiguration("lib");
    }

    private SdkTestProject CreateHostReferencingLibraries()
    {
        var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // track: false - built only through the host's ProjectReference (see HostConfigurationPropagationTests).
        var lib1 = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project.AddProject("lib1/lib1.csproj", lib1, track: false).AddFile("lib1/Fragments.cs", GreetingsFragment);
        var lib2 = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project.AddProject("lib2/lib2.csproj", lib2, track: false).AddFile("lib2/Fragments.cs", SharedFragments);

        var host = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe").ItemProjectReference(lib1).ItemProjectReference(lib2);
        project
           .AddProject("host/host.csproj", host)
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);
        return project;
    }

    [Test]
    public async Task HostReferencingLibraries_MergesTheirSections_AndCopiesTheSchemaToItsOutput()
    {
        using var project = CreateHostReferencingLibraries();

        await project.VerifyProjects(HostSchema);

        project.ReadFile($"host/bin/Debug/{targetFramework}/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    [Test]
    public async Task HostReferencingLibraries_PublishesTheSchema()
    {
        using var project = CreateHostReferencingLibraries();

        await project.VerifyPublish("host");

        project.ReadFile($"host/bin/Release/{targetFramework}/publish/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    private async Task<SdkTestProject> CreateHostReferencingPackages()
    {
        var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // Distinctive package IDs, so restore can't pick up an unrelated nuget.org package (see HostConfigurationPropagationTests).
        project
           .AddProject("lib1/DovetailSchemaTestLib1.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib1/Fragments.cs", GreetingsFragment);
        project
           .AddProject("lib2/DovetailSchemaTestLib2.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("lib2/Fragments.cs", SharedFragments);
        var lib1 = await project.PackToLocalFeed("lib1");
        var lib2 = await project.PackToLocalFeed("lib2");
        lib1.Files.ShouldContain("ConfigurationSchema.json");
        lib2.Files.ShouldContain("ConfigurationSchema.json");

        var host = ProjectCreator
                  .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                  .ItemInclude("PackageReference", include: lib1.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = lib1.Version })
                  .ItemInclude("PackageReference", include: lib2.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = lib2.Version });
        project
           .AddProject("host/host.csproj", host)
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);
        return project;
    }

    [Test]
    public async Task HostReferencingPackages_MergesTheirSections_AndCopiesTheSchemaToItsOutput()
    {
        using var project = await CreateHostReferencingPackages();

        await project.VerifyProjects(HostSchema);

        project.ReadFile($"host/bin/Debug/{targetFramework}/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    [Test]
    public async Task HostReferencingPackages_PublishesTheSchema()
    {
        using var project = await CreateHostReferencingPackages();

        await project.VerifyPublish("host");

        project.ReadFile($"host/bin/Release/{targetFramework}/publish/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    /// <summary>
    /// Test projects are hosts too, whatever their output type: they read configuration the way the application does.
    /// </summary>
    [Test]
    public async Task TestProject_MergesTheSectionsOfWhatItTests()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));
        var lib = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);
        project.AddProject("lib/lib.csproj", lib, track: false).AddFile("lib/Fragments.cs", GreetingsFragment);
        project.AddProject("tests/tests.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework).Property("IsTestProject", "true").ItemProjectReference(lib));

        await project.VerifyBuildWarnings("tests");

        project.ReadFile("tests/obj/appsettings.schema.json").ShouldContain("Unknown key in section \\\"Greetings\\\"");
        project.ReadFile($"tests/bin/Debug/{targetFramework}/appsettings.schema.json").ShouldBe(project.ReadFile("tests/obj/appsettings.schema.json"));
    }

    /// <summary>
    /// An application that only references Dovetail.Hosting (no Dovetail.Sdk, no direct Dovetail reference) gets Dovetail's
    /// build assets transitively, and with them the schema.
    /// </summary>
    private SdkTestProject CreateHostingOnlyHost()
    {
        var host = ProjectCreator
                  .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                  .ItemInclude(
                       "PackageReference",
                       include: "Joinery.Dovetail.Hosting",
                       metadata: new Dictionary<string, string?> { ["Version"] = Config.DovetailPackageVersion("Joinery.Dovetail.Hosting") }
                   );
        var project = new SdkTestProject();
        project
           .AddProject("host/host.csproj", host)
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);
        return project;
    }

    [Test]
    public async Task HostingOnlyHost_MergesItsSections_AndCopiesTheSchemaToItsOutput()
    {
        using var project = CreateHostingOnlyHost();

        await project.VerifyProjects(HostSchema);

        project.ReadFile($"host/bin/Debug/{targetFramework}/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }

    [Test]
    public async Task HostingOnlyHost_PublishesTheSchema()
    {
        using var project = CreateHostingOnlyHost();

        await project.VerifyPublish("host");

        project.ReadFile($"host/bin/Release/{targetFramework}/publish/appsettings.schema.json").ShouldBe(project.ReadFile(HostSchema));
    }
}
