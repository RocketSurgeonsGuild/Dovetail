using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers <c>Dovetail.ConfigurationSchema.VisualStudio.targets</c>: the <c>JsonSchemaSegment</c> items Visual Studio's JSON
/// editor reads. A host gets one for its merged <c>obj/appsettings.schema.json</c>; every project gets one for each referenced
/// package that ships a root <c>ConfigurationSchema.json</c>, unless the package (or the project) declares it already. The
/// items are added by a target, so each test builds just far enough to run it. What Visual Studio does with them can't be
/// tested here.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaVisualStudioTests(string targetFramework)
{
    private const string ResolveReferences = "-t:ResolveAssemblyReferences";

    /// <summary>
    /// What a Visual Studio design-time build runs to get the references, without compiling (its project system's
    /// CollectResolvedCompilationReferencesDesignTime, which only exists inside Visual Studio, builds on this target).
    /// </summary>
    private const string DesignTimeReferences = "-t:ResolveAssemblyReferencesDesignTime -p:DesignTimeBuild=true -p:BuildingInsideVisualStudio=true";

    private const string FilePathPattern = @"appsettings\..*json";

    private static SdkTestProject CreateProject()
    {
        var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create("Directory.Build.props").Sdk("Dovetail.Sdk"));
        return project;
    }

    private static ProjectCreator HostProject(string targetFramework, string outputType = "Exe") =>
        ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: outputType);

    private static ProjectCreator Library(string targetFramework) => ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework);

    private string HostSchemaPath(SdkTestProject project, string relativePath) =>
        Path.GetFullPath(Path.Combine(project.Directory, relativePath, "obj", "appsettings.schema.json"));

    /// <summary>
    /// A package's root <c>ConfigurationSchema.json</c> in whichever folder NuGet extracted it to; the folder is named by the
    /// lower-cased ID and the version.
    /// </summary>
    private static bool IsPackageSchema(string identity, PackageVerificationResult package) =>
        identity.EndsWith(
            string.Join(Path.DirectorySeparatorChar, package.PackageId.ToLowerInvariant(), package.Version.ToLowerInvariant(), "ConfigurationSchema.json"),
            StringComparison.OrdinalIgnoreCase
        );

    private static string[] Identities(IReadOnlyList<IReadOnlyDictionary<string, string>> segments) =>
        segments.Select(z => Path.GetFullPath(z["Identity"])).OrderBy(z => z, StringComparer.Ordinal).ToArray();

    private static void ShouldAllMatchTheFilePattern(IReadOnlyList<IReadOnlyDictionary<string, string>> segments)
    {
        foreach (var segment in segments)
        {
            segment["FilePathPattern"].ShouldBe(FilePathPattern);
        }
    }

    private async Task<(SdkTestProject Project, PackageVerificationResult[] Packages)> CreateProjectWithPackedLibraries(params string[] packageIds)
    {
        var project = CreateProject();
        var packages = new List<PackageVerificationResult>();
        foreach (var packageId in packageIds)
        {
            // Distinctive package IDs, so restore can't pick up an unrelated nuget.org package (see ConfigurationSchemaTests).
            var library = Library(targetFramework);
            if (packageId.EndsWith("Own", StringComparison.Ordinal))
            {
                // Declares its own segment from buildTransitive, the way Aspire's client packages do.
                library.ItemInclude(
                    "None",
                    "Own.targets",
                    metadata: new Dictionary<string, string?>
                    {
                        ["Pack"] = "true", ["PackagePath"] = $"buildTransitive/{targetFramework}/{packageId}.targets", ["Visible"] = "false",
                    }
                );
                project.AddFile(
                    $"{packageId}/Own.targets",
                    """
                    <Project>
                      <ItemGroup>
                        <JsonSchemaSegment Include="$(MSBuildThisFileDirectory)..\..\ConfigurationSchema.json" FilePathPattern="appsettings\..*json" />
                      </ItemGroup>
                    </Project>
                    """
                );
            }

            project
               .AddProject($"{packageId}/{packageId}.csproj", library)
               .AddFile($"{packageId}/Fragments.cs", ConfigurationSchemaTests.GreetingsFragment);
            var package = await project.PackToLocalFeed(packageId);
            package.Files.ShouldContain("ConfigurationSchema.json");
            packages.Add(package);
        }

        return (project, packages.ToArray());
    }

    private static ProjectCreator ReferencePackages(ProjectCreator project, params PackageVerificationResult[] packages)
    {
        foreach (var package in packages)
        {
            project.ItemInclude("PackageReference", include: package.PackageId, metadata: new Dictionary<string, string?> { ["Version"] = package.Version });
        }

        return project;
    }

    [Test]
    public async Task Host_AddsASegmentForItsMergedSchema_AndNothingForProjectReferences()
    {
        using var project = CreateProject();
        var lib = Library(targetFramework);
        project.AddProject("lib/lib.csproj", lib, track: false).AddFile("lib/Fragments.cs", ConfigurationSchemaTests.GreetingsFragment);
        project
           .AddProject("host/host.csproj", HostProject(targetFramework).ItemProjectReference(lib))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", ResolveReferences);

        Identities(segments).ShouldBe([HostSchemaPath(project, "host")]);
        ShouldAllMatchTheFilePattern(segments);
    }

    /// <summary>
    /// The items are there in the build a Visual Studio session runs when it loads the project.
    /// </summary>
    [Test]
    public async Task Host_AddsItsSegmentInDesignTimeBuilds()
    {
        using var project = CreateProject();
        project.AddProject("host/host.csproj", HostProject(targetFramework)).AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", DesignTimeReferences);

        Identities(segments).ShouldBe([HostSchemaPath(project, "host")]);
    }

    [Test]
    public async Task TestProject_AddsASegmentForItsMergedSchema()
    {
        using var project = CreateProject();
        project.AddProject("tests/tests.csproj", Library(targetFramework).Property("IsTestProject", "true"));

        var segments = await project.EvaluateItems("tests", "JsonSchemaSegment", ResolveReferences);

        Identities(segments).ShouldBe([HostSchemaPath(project, "tests")]);
    }

    [Test]
    public async Task Host_AddsASegmentForEachReferencedPackageWithASchema()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsSegmentsLib1", "DovetailVsSegmentsLib2");
        using var _ = project;
        project
           .AddProject("host/host.csproj", ReferencePackages(HostProject(targetFramework), packages))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", ResolveReferences);

        var identities = Identities(segments);
        identities.ShouldContain(HostSchemaPath(project, "host"));
        foreach (var package in packages)
        {
            identities.Count(z => IsPackageSchema(z, package)).ShouldBe(1);
        }

        identities.ShouldBe(identities.Distinct().ToArray());
        ShouldAllMatchTheFilePattern(segments);
        File.Exists(identities.Single(z => IsPackageSchema(z, packages[0]))).ShouldBeTrue();
    }

    [Test]
    public async Task Host_AddsPackageSegmentsInDesignTimeBuilds()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsDesignTimeLib");
        using var _ = project;
        project
           .AddProject("host/host.csproj", ReferencePackages(HostProject(targetFramework), packages))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", DesignTimeReferences);

        var identities = Identities(segments);
        identities.ShouldContain(HostSchemaPath(project, "host"));
        identities.Count(z => IsPackageSchema(z, packages[0])).ShouldBe(1);
    }

    /// <summary>
    /// A library gets the segments of the packages it references, but has no merged file of its own.
    /// </summary>
    [Test]
    public async Task Library_AddsPackageSegments_ButNoHostSegment()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsLibraryConsumedLib");
        using var _ = project;
        project.AddProject("consumer/consumer.csproj", ReferencePackages(Library(targetFramework), packages));

        var segments = await project.EvaluateItems("consumer", "JsonSchemaSegment", ResolveReferences);

        var identities = Identities(segments);
        identities.Count(z => IsPackageSchema(z, packages[0])).ShouldBe(1);
        identities.ShouldNotContain(HostSchemaPath(project, "consumer"));
    }

    /// <summary>
    /// A package that declares the segment itself, as Aspire's client packages do from buildTransitive, ends up once.
    /// </summary>
    [Test]
    public async Task PackageDeclaringItsOwnSegment_IsNotAddedAgain()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsDeclaredLibOwn", "DovetailVsDiscoveredLib");
        using var _ = project;
        project
           .AddProject("host/host.csproj", ReferencePackages(HostProject(targetFramework), packages))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        // Evaluation alone sees what the package declares...
        var declared = Identities(await project.EvaluateItems("host", "JsonSchemaSegment"));
        declared.Count(z => IsPackageSchema(z, packages[0])).ShouldBe(1);
        declared.Count(z => IsPackageSchema(z, packages[1])).ShouldBe(0);

        // ...and the build adds only what is missing.
        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", ResolveReferences);
        var identities = Identities(segments);
        identities.Count(z => IsPackageSchema(z, packages[0])).ShouldBe(1);
        identities.Count(z => IsPackageSchema(z, packages[1])).ShouldBe(1);
        identities.Length.ShouldBe(3);
    }

    /// <summary>
    /// A host that declares the segment for its own merged file keeps just that one.
    /// </summary>
    [Test]
    public async Task HostDeclaringItsOwnSegment_IsNotAddedAgain()
    {
        using var project = CreateProject();
        var host = HostProject(targetFramework)
           .ItemInclude(
                "JsonSchemaSegment",
                @"$(MSBuildProjectDirectory)\obj\appsettings.schema.json",
                metadata: new Dictionary<string, string?> { ["FilePathPattern"] = FilePathPattern }
            );
        project.AddProject("host/host.csproj", host).AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", ResolveReferences);

        Identities(segments).ShouldBe([HostSchemaPath(project, "host")]);
    }

    [Test]
    public async Task VisualStudioOptOut_AddsNoSegments_ButPackageSegmentsDeclaredByPackagesRemain()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsOptOutLibOwn", "DovetailVsOptOutDiscoveredLib");
        using var _ = project;
        project
           .AddProject("host/host.csproj", ReferencePackages(HostProject(targetFramework), packages))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", $"{ResolveReferences} -p:DovetailConfigurationSchemaVisualStudio=false");

        var identities = Identities(segments);
        identities.Length.ShouldBe(1);
        IsPackageSchema(identities[0], packages[0]).ShouldBeTrue();
    }

    [Test]
    public async Task SchemaOptOut_AddsNoSegments_ButPackageSegmentsDeclaredByPackagesRemain()
    {
        var (project, packages) = await CreateProjectWithPackedLibraries("DovetailVsSchemaOffLibOwn", "DovetailVsSchemaOffDiscoveredLib");
        using var _ = project;
        project
           .AddProject("host/host.csproj", ReferencePackages(HostProject(targetFramework), packages))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");");

        var segments = await project.EvaluateItems("host", "JsonSchemaSegment", $"{ResolveReferences} -p:DovetailConfigurationSchema=false");

        var identities = Identities(segments);
        identities.Length.ShouldBe(1);
        IsPackageSchema(identities[0], packages[0]).ShouldBeTrue();
    }
}
