using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Utilities.ProjectCreation;
using NuGet.Packaging;
using TUnit.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// A single NuGet package produced by <c>dotnet pack</c>, read back via
/// <see cref="PackageArchiveReader"/> (a <see cref="PackageReaderBase"/>)
/// so tests can assert on what actually landed inside the .nupkg rather than trusting the MSBuild
/// evaluation snapshot alone.
/// </summary>
public sealed record PackageVerificationResult(string PackageId, string Version, IReadOnlyList<string> Files);

/// <summary>
/// Scaffolds a throwaway consumer project wired to the packed SDKs in the repo's
/// <c>artifacts/</c> directory. Evaluation-level assertions run in-proc via the MSBuild
/// runtime and are snapshotted with Verify; end-to-end behavior (restore/build/run/test/pack)
/// goes through <see cref="Dotnet"/>.
/// </summary>
public sealed class SdkTestProject : IDisposable
{
    private readonly List<ProjectCreator> _projects = [];
    private readonly VerifySettings _settings;
    private readonly PackageRepository _repository;

    public string NugetArtifactsDirectory { get; }
    public string Directory { get; }

    public SdkTestProject(string? nugetArtifactsDirectory = null)
    {
        NugetArtifactsDirectory = nugetArtifactsDirectory ?? Config.NugetArtifactsDirectory;

        var id = Guid.NewGuid().ToString("N");
        Directory = Path.Combine(Path.GetTempPath(), "sdk-tests", id);
        System.IO.Directory.CreateDirectory(Directory);

        _repository = PackageRepository.Create(Directory, feeds: [new("https://api.nuget.org/v3/index.json"), new(NugetArtifactsDirectory)]);
        foreach (var package in System.IO.Directory.EnumerateFiles(NugetArtifactsDirectory, "*.nupkg"))
        {
            _repository.Package(new(package), out _);
        }

        // PackageRepository.Package() above eagerly extracts each nupkg into this project's
        // isolated global-packages-folder by re-serializing its own copy of the nuspec - and that
        // re-serialization drops the <dependencies> group entirely (confirmed by diffing against
        // the source nupkg's nuspec). NuGet trusts an already-extracted package folder and won't
        // re-read the real one from the Local1 feed, so every Dovetail package with NuGet
        // dependencies (e.g. Dovetail.Hosting -> Microsoft.Extensions.Hosting) would silently lose
        // them for the rest of this test. Evict the pre-seeded copies so the real restore that
        // runs during VerifyProjects()/Dotnet is forced to extract fresh, correct ones.
        var globalPackagesFolder = Path.Combine(Directory, ".nuget", "packages");
        if (System.IO.Directory.Exists(globalPackagesFolder))
        {
            System.IO.Directory.Delete(globalPackagesFolder, recursive: true);
        }

        _settings = new VerifySettings();
        // MSBuild resolves the entry project's own full path via the process's current directory,
        // which macOS reports through the /var,/tmp -> /private/var,/private/tmp symlink. Verify's
        // built-in TempPath scrubber only matches the unresolved Path.GetTempPath() form, so every
        // other (import-chain-derived) path gets normalized to {TempPath} but this one doesn't.
        // Scrub both forms - and the id - in a single pass, since separate ScrubLinesWithReplace
        // registrations each run against the original line rather than chaining.
        var tempPath = Path.GetTempPath();
        _settings.ScrubLinesWithReplace(z => z.Replace("/private" + tempPath, "{TempPath}").Replace(id, "{id}"));
        // A project that never references Dovetail.Sdk has nothing pinning $(CustomAfterMicrosoftCommonTargets)
        // to a specific installed .NET SDK version - global.json's rollForward: latestMajor lets it float to
        // whatever's newest on the machine (e.g. a preview SDK), which would otherwise make the snapshot differ
        // across dev machines/CI runners with a different set of installed SDKs.
        _settings.ScrubLinesWithReplace(z => System.Text.RegularExpressions.Regex.Replace(z, @".*/sdk/\d+\.\d+\.\d+[^/]*/", "/sdk/{dotnet-sdk-version}/"));

        var currentGlobalJson = JsonDocument.Parse(File.ReadAllText(Path.Combine(Config.RootDirectory, "global.json")));
        var version = currentGlobalJson.RootElement.GetProperty("sdk").GetProperty("version").GetString();

        var globalJson = GlobalJsonCreator.Create(new DirectoryInfo(Directory));
        foreach (var sdkName in _repository.Packages.Where(z => z.Id.StartsWith("Dovetail.Sdk", StringComparison.OrdinalIgnoreCase)))
        {
            globalJson.MSBuildSdk(sdkName.Id, sdkName.Version);
            _settings.ScrubLinesWithReplace(z => z.Replace(sdkName.Version, "{sdk-version}"));
        }

        globalJson
            .TestRunner("Microsoft.Testing.Platform")
            .SdkVersion(version)
            .SdkRollForward(GlobalJsonSdkRollForward.LatestMajor)
            .Save();
    }

    /// <summary>
    /// Saves a project file to disk. When <paramref name="track"/> is <see langword="true"/>
    /// (the default) and the file is a .csproj, it's also registered for <see cref="VerifyProjects"/>
    /// / <see cref="VerifyProjectsAndPackages"/> to build independently. Pass <see langword="false"/>
    /// for a library that's meant to be built only transitively - via a <c>&lt;ProjectReference&gt;</c>
    /// from another tracked project - since building it standalone first would leave it "up to
    /// date" by the time the referencing project's own build runs, which skips regenerating its
    /// reference assembly (<c>obj/.../ref/{name}.dll</c>) and fails the referencing project's
    /// compile with CS0006.
    /// </summary>
    public SdkTestProject AddProject(string path, ProjectCreator project, bool track = true)
    {
        project.Save(Path.Combine(Directory, path));
        if (track && project.FullPath.EndsWith(".csproj"))
        {
            _projects.Add(project);
        }
        return this;
    }

    public SdkTestProject AddFile(string path, string content)
    {
        var fullPath = Path.Combine(Directory, path);
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, content);
        return this;
    }

    public async Task VerifyProjects()
    {
        var results = new List<ProjectEvaluationResult>();
        foreach (var project in _projects)
        {
            var relativePath = Path.GetDirectoryName(project.FullPath)!;
            results.Add(await BuildAndEvaluate(relativePath, "build"));
        }

        await Verify(results, settings: _settings);
    }

    /// <summary>
    /// Like <see cref="VerifyProjects"/>, but runs <c>dotnet pack</c> instead of <c>dotnet build</c>
    /// and additionally reads the resulting .nupkg back with <see cref="PackageArchiveReader"/> (a
    /// <see cref="PackageReaderBase"/>) to assert on the package identity and its
    /// actual file listing - the MSBuild evaluation snapshot alone can't tell you what a
    /// <c>Pack="true"</c>/<c>PackagePath</c> item actually produced inside the archive.
    /// </summary>
    public async Task VerifyProjectsAndPackages()
    {
        var results = new List<ProjectEvaluationResult>();
        var packages = new List<PackageVerificationResult>();
        foreach (var project in _projects)
        {
            var relativePath = Path.GetDirectoryName(project.FullPath)!;
            results.Add(await BuildAndEvaluate(relativePath, "pack"));

            var nupkg = System.IO.Directory.EnumerateFiles(relativePath, "*.nupkg", SearchOption.AllDirectories).SingleOrDefault() ?? throw new TUnitException($"dotnet pack did not produce a .nupkg for {relativePath}");
            packages.Add(ReadPackage(nupkg));
        }

        await Verify(new { Projects = results, Packages = packages }, settings: _settings);
    }

    /// <summary>
    /// Packs the project at <paramref name="relativePath"/> (e.g. <c>"lib1"</c>) and registers the
    /// resulting .nupkg with this scaffold's own local NuGet feed, so a project added afterward can
    /// consume it via a genuine <c>&lt;PackageReference&gt;</c> - exercising the "library shipped
    /// via a NuGet feed with its own configuration" path (NuGet's <c>contentFiles</c> restore
    /// convention) rather than the <c>&lt;ProjectReference&gt;</c> transitive-copy path covered by
    /// <see cref="BuildAndEvaluate"/>. The caller is responsible for reading the returned
    /// <see cref="PackageVerificationResult.Version"/> back into the consuming project's
    /// <c>PackageReference</c> item, since a plain <c>dotnet pack</c> with no explicit
    /// <c>&lt;Version&gt;</c> only assigns one once packing actually happens.
    /// </summary>
    public async Task<PackageVerificationResult> PackToLocalFeed(string relativePath)
    {
        var projectDirectory = Path.Combine(Directory, relativePath);
        await BuildAndEvaluate(projectDirectory, "pack");

        var nupkg = System.IO.Directory.EnumerateFiles(projectDirectory, "*.nupkg", SearchOption.AllDirectories).SingleOrDefault() ?? throw new TUnitException($"dotnet pack did not produce a .nupkg for {relativePath}");
        var result = ReadPackage(nupkg);

        // Deliberately NOT PackageRepository.Package() (used in the constructor for the repo's own
        // Dovetail.* packages): that call fabricates a synthetic placeholder package (a stub
        // assembly under a dummy "net452" target and none of the real content files) in the
        // isolated global-packages-folder rather than exposing the genuine .nupkg through a real
        // feed. Fine for packages this scaffold only restores by identity, but wrong here - without
        // a real feed, restore falls through to nuget.org and may silently resolve an unrelated,
        // same-named public package instead (confirmed happening for "lib1"/"lib2" test packages).
        // Register the pack output directory itself as an additional local feed so restore finds
        // and extracts the real .nupkg, contentFiles and all.
        AddLocalPackageSource(Path.GetDirectoryName(nupkg)!);

        return result;
    }

    /// <summary>
    /// Adds <paramref name="folder"/> as an additional <c>&lt;packageSources&gt;</c> entry in this
    /// scaffold's NuGet.Config. Any folder containing loose .nupkg files is a valid NuGet feed on
    /// its own - no index required.
    /// </summary>
    private void AddLocalPackageSource(string folder)
    {
        var nugetConfigPath = System.IO.Directory.EnumerateFiles(Directory, "NuGet.config", SearchOption.TopDirectoryOnly).Single();
        var document = XDocument.Load(nugetConfigPath);
        var packageSources = document.Root!.Element("packageSources")!;
        packageSources.Add(new XElement("add", new XAttribute("key", $"local-{packageSources.Elements("add").Count()}"), new XAttribute("value", folder)));
        document.Save(nugetConfigPath);
    }

    /// <summary>
    /// Reads a package's identity and file listing via <see cref="PackageArchiveReader"/>. The
    /// OPC bookkeeping entries (<c>_rels/</c>, <c>package/services/metadata/</c>,
    /// <c>[Content_Types].xml</c>) are excluded - they're an artifact of the .nupkg being a zip
    /// package, not something <c>Dovetail.PackConfiguration.targets</c> controls, so they're not
    /// meaningful test signal here.
    /// </summary>
    private static PackageVerificationResult ReadPackage(string nupkgPath)
    {
        using var reader = new PackageArchiveReader(nupkgPath);
        var identity = reader.GetIdentity();
        var files = reader.GetFiles()
           .Where(
                f => !f.StartsWith("_rels/", StringComparison.OrdinalIgnoreCase)
                    && !f.StartsWith("package/services/metadata/", StringComparison.OrdinalIgnoreCase)
                    && !f.Equals("[Content_Types].xml", StringComparison.OrdinalIgnoreCase)
            )
           .OrderBy(f => f, StringComparer.Ordinal)
           .ToArray();
        return new(identity.Id, identity.Version.ToNormalizedString(), files);
    }

    private async Task<ProjectEvaluationResult> BuildAndEvaluate(string relativePath, string command)
    {
        var psi = CreateHermeticStartInfo($"{command} -bl");
        psi.WorkingDirectory = relativePath;
        using var proc = Process.Start(psi)!;
        var stdout = await proc.StandardOutput.ReadToEndAsync();
        var stderr = await proc.StandardError.ReadToEndAsync();
        await proc.WaitForExitAsync();
        if (proc.ExitCode != 0)
        {
            throw new TUnitException($"dotnet {command} failed for {relativePath}:\n{stdout}\n{stderr}");
        }

        var binlog = Path.Combine(relativePath, "msbuild.binlog");
        var build = BinaryLog.ReadBuild(binlog);
        BuildAnalyzer.AnalyzeBuild(build);
        var projectEvaluation = build.FindChildrenRecursive<ProjectEvaluation>()[0];

        // The generated-source Compile items (DovetailJoint*.g.cs, DovetailHost.*.g.cs,
        // DovetailContextBuilder.g.cs, Dovetail.*Configuration.g.cs, ...) are added by
        // BeforeTargets="CoreCompile" targets at execution time, so they never show up in the
        // evaluation snapshot above - read them straight off disk instead.
        var objDirectory = Path.Combine(relativePath, "obj");
        var generatedFiles = System.IO.Directory.Exists(objDirectory)
            ? System.IO.Directory.EnumerateFiles(objDirectory, "*.g.cs", SearchOption.AllDirectories).Select(Path.GetFileName).ToArray()
            : [];

        // Dovetail.PackConfiguration.targets renames/copies appsettings.{ext} to
        // $(MSBuildProjectName).{ext} in bin/ via <None Update> metadata against an item the
        // .NET SDK's own default globs add (evaluated before this project's own body) - that
        // never shows up in the evaluation snapshot's Items either, so verify it landed by
        // reading the actual build output instead. When this project has a <ProjectReference>
        // to another Dovetail-configured project, MSBuild's own GetCopyToOutputDirectoryItems
        // walks that graph too, so a host application's own bin/ ends up with every referenced
        // library's renamed config file alongside its own.
        var binDirectory = Path.Combine(relativePath, "bin");
        var outputFiles = ListConfigurationFiles(binDirectory);

        return new(projectEvaluation, generatedFiles!, outputFiles);
    }

    /// <summary>
    /// Runs <c>dotnet publish</c> for the project at <paramref name="hostRelativePath"/> and
    /// verifies which configuration files (renamed by <c>Dovetail.PackConfiguration.targets</c>,
    /// including any transitively copied down from <c>ProjectReference</c>d libraries) ended up
    /// in its <c>publish/</c> output directory - publish assembles its own file set from
    /// <c>ResolvedFileToPublish</c> rather than reusing the build's bin/ contents directly, so
    /// this is not guaranteed by <see cref="VerifyProjects"/> passing.
    /// </summary>
    public async Task VerifyPublish(string hostRelativePath)
    {
        var hostDirectory = Path.Combine(Directory, hostRelativePath);
        var psi = CreateHermeticStartInfo("publish -bl");
        psi.WorkingDirectory = hostDirectory;
        using var proc = Process.Start(psi)!;
        var stdout = await proc.StandardOutput.ReadToEndAsync();
        var stderr = await proc.StandardError.ReadToEndAsync();
        await proc.WaitForExitAsync();
        if (proc.ExitCode != 0)
        {
            throw new TUnitException($"dotnet publish failed for {hostRelativePath}:\n{stdout}\n{stderr}");
        }

        var binDirectory = Path.Combine(hostDirectory, "bin");
        var publishDirectory = System.IO.Directory.Exists(binDirectory)
            ? System.IO.Directory.EnumerateDirectories(binDirectory, "publish", SearchOption.AllDirectories).SingleOrDefault()
            : null;
        var publishFiles = publishDirectory is null ? [] : ListConfigurationFiles(publishDirectory);

        await Verify(publishFiles, settings: _settings);
    }

    /// <summary>
    /// Lists the config-shaped filenames (json/yaml/yml/toml) under <paramref name="directory"/>,
    /// excluding the .NET SDK's own generated *.deps.json/*.runtimeconfig.json/
    /// *.staticwebassets.endpoints.json - those share the same extensions but aren't anything
    /// <c>Dovetail.PackConfiguration.targets</c> produced.
    /// </summary>
    private static IReadOnlyList<string> ListConfigurationFiles(string directory) =>
        System.IO.Directory.Exists(directory)
            ? System.IO.Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                 .Select(z => Path.GetFileName(z))
                 .Where(z => z.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                      || z.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase)
                      || z.EndsWith(".yml", StringComparison.OrdinalIgnoreCase)
                      || z.EndsWith(".toml", StringComparison.OrdinalIgnoreCase))
                 .Where(z => !z.EndsWith(".deps.json", StringComparison.OrdinalIgnoreCase)
                      && !z.EndsWith(".runtimeconfig.json", StringComparison.OrdinalIgnoreCase)
                      && !z.EndsWith(".staticwebassets.endpoints.json", StringComparison.OrdinalIgnoreCase))
                 .ToArray()
            : [];

    /// <summary>
    /// Like <see cref="CreateDotnetStartInfo"/>, but only strips CI-detection env vars so the
    /// scaffolded build's evaluated properties (e.g. <c>ContinuousIntegrationBuild</c>) don't
    /// differ between a local run and a CI runner. Deliberately leaves MSBuildSDKsPath/
    /// TESTINGPLATFORM_UI_LANGUAGE untouched: stripping those breaks apphost/SDK resolution for
    /// real (non-evaluation-only) builds on at least one dev machine.
    /// </summary>
    private ProcessStartInfo CreateHermeticStartInfo(string arguments)
    {
        var psi = new ProcessStartInfo("dotnet", arguments)
        {
            WorkingDirectory = Directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        psi.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        // Keep scaffolded builds hermetic: CI env vars would flip ContinuousIntegrationBuild
        // (warnings-as-errors, coverage) and make results differ between local and CI runs.
        psi.Environment.Remove("GITHUB_ACTIONS");
        psi.Environment.Remove("CI");
        psi.Environment.Remove("TF_BUILD");
        psi.Environment.Remove("GITLAB_CI");
        psi.Environment.Remove("APPVEYOR");
        psi.Environment.Remove("TEAMCITY_VERSION");
        // MSBuild.ProjectCreation/MSBuild.StructuredLogger.Utils load MSBuild in-proc for this
        // test *process*, which pins MSBuildSDKsPath/MSBUILD_EXE_PATH/MSBuildExtensionsPath to
        // that in-proc toolset for its own use - and Process.Start copies the parent environment
        // by default, so a spawned `dotnet build` inherits that pin too. When it disagrees with
        // the SDK global.json resolves for the scaffolded project (observed on this machine:
        // in-proc pin said 10.0.301, global.json rolled forward to an installed 11.0 preview),
        // ProjectReference builds intermittently fail with CS0006 (stale/half-built reference
        // assemblies) since two different toolset versions end up evaluating the project graph.
        // Stripping them lets the spawned process resolve its own SDK independently.
        psi.Environment.Remove("MSBuildSDKsPath");
        psi.Environment.Remove("MSBUILD_EXE_PATH");
        psi.Environment.Remove("MSBuildExtensionsPath");
        psi.Environment.Remove("MSBuildLoadMicrosoftTargetsReadOnly");
        return psi;
    }

    public void Dispose()
    {
        try
        {
            System.IO.Directory.Delete(Directory, recursive: true);
        }
        catch (IOException)
        {
            // Best effort cleanup; temp directory reaping will handle stragglers.
        }
    }
}
