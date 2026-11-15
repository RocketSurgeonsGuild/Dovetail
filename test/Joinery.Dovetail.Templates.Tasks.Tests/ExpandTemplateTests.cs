using System.Collections;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Task = System.Threading.Tasks.Task;

namespace Joinery.Dovetail.Templates.Tasks.Tests;

/// <summary>
///     <see cref="ExpandTemplate" /> run the way Joinery.Dovetail.Templates.targets runs it: one DovetailEmittedTemplate item, its
///     File, HostName and HostType, and the file to write. TemplateExtensionPointTests in Joinery.Dovetail.Sdk.Tests covers the
///     targets around it in a real build.
/// </summary>
public sealed class ExpandTemplateTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("dovetail-template-").FullName;

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    [Test]
    public async Task EveryNonEmptyMetadataValue_ReplacesItsTokens_InBothForms_IgnoringCase()
    {
        var run = Run(
            """
            namespace __Namespace__;
            internal static class __clientname__ { }
            // {ClientName} from {OutputPrefix}, {Empty} {Unknown} __Unknown__ {} {1}
            """,
            new() { ["ClientName"] = "RocketsClient", ["Namespace"] = "MyApp.Rockets", ["OutputPrefix"] = "Chirograph", ["Empty"] = "" }
        );

        await run.Succeeded.Should().BeTrue();
        await run.Output.Should().BeEqualTo(
            $$"""
            namespace MyApp.Rockets;
            internal static class RocketsClient { }
            // RocketsClient from Chirograph, {Empty} {Unknown} __Unknown__ {} {1}{{Environment.NewLine}}
            """
        );
    }

    [Test]
    public async Task HostNameAndHostType_AreReplacedOnlyWhenHostTypeIsSet()
    {
        const string template = "{HostName} __HostType__";

        var withoutType = Run(template, new() { ["HostName"] = "Web" }, hostName: "Web", outputFile: Path.Combine(_directory, "without.g.cs"));
        var withType = Run(
            template,
            new() { ["HostName"] = "Web", ["HostType"] = "WebApplication" },
            hostName: "Web",
            hostType: "WebApplication",
            outputFile: Path.Combine(_directory, "with.g.cs")
        );

        await withoutType.Output.Should().BeEqualTo($"{{HostName}} __HostType__{Environment.NewLine}");
        await withType.Output.Should().BeEqualTo($"Web WebApplication{Environment.NewLine}");
    }

    [Test]
    public async Task TheFile_IsUtf8WithoutABom_KeepsBackslashes_AndItsFolderIsCreated()
    {
        var run = Run(@"var path = ""C:\\{Name}\\x"";", new() { ["Name"] = "Logs" }, outputFile: Path.Combine(_directory, "obj", "Debug", "out.g.cs"));

        await run.Succeeded.Should().BeTrue();
        await File.ReadAllBytes(run.OutputFile).Should().BeEquivalentTo(
            Encoding.UTF8.GetBytes(@"var path = ""C:\\Logs\\x"";" + Environment.NewLine),
            CollectionOrdering.Matching
        );
    }

    [Test]
    public async Task AnUnchangedFile_IsLeftAlone()
    {
        var first = Run("class {Name} { }", new() { ["Name"] = "A" });
        var written = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(first.OutputFile, written);

        Run("class {Name} { }", new() { ["Name"] = "A" });
        var unchanged = File.GetLastWriteTimeUtc(first.OutputFile);
        Run("class {Name} { }", new() { ["Name"] = "B" });

        await unchanged.Should().BeEqualTo(written);
        await File.ReadAllText(first.OutputFile).Should().BeEqualTo($"class B {{ }}{Environment.NewLine}");
    }

    [Test]
    public async Task ARelativeOutputFile_IsAnError()
    {
        var run = Run("class A { }", [], outputFile: "obj/out.g.cs");

        await run.Succeeded.Should().BeFalse();
        await run.Errors.Should().HaveSingleItem();
    }

    private sealed record Result(bool Succeeded, string OutputFile, IReadOnlyList<string> Errors)
    {
        public string Output => File.ReadAllText(OutputFile);
    }

    private Result Run(string template, Dictionary<string, string> metadata, string hostName = "", string hostType = "", string? outputFile = null)
    {
        var templateFile = Path.Combine(_directory, "Template.cs.template");
        File.WriteAllText(templateFile, template);
        metadata["File"] = templateFile;

        var engine = new BuildEngine();
        var task = new ExpandTemplate
        {
            BuildEngine = engine,
            Template = new TaskItem("Template", metadata),
            File = templateFile,
            HostName = hostName,
            HostType = hostType,
            OutputFile = outputFile ?? Path.Combine(_directory, "out.g.cs"),
        };

        return new(task.Execute(), task.OutputFile, engine.Errors);
    }

    private sealed class BuildEngine : IBuildEngine
    {
        public List<string> Errors { get; } = [];

        public bool ContinueOnError => false;

        public int LineNumberOfTaskNode => 0;

        public int ColumnNumberOfTaskNode => 0;

        public string ProjectFileOfTaskNode => "";

        public void LogErrorEvent(BuildErrorEventArgs e) => Errors.Add(e.Message ?? "");

        public void LogWarningEvent(BuildWarningEventArgs e) { }

        public void LogMessageEvent(BuildMessageEventArgs e) { }

        public void LogCustomEvent(CustomBuildEventArgs e) { }

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs) => false;
    }
}
