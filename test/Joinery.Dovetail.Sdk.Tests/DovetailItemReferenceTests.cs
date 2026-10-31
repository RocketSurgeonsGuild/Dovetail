using Microsoft.Build.Utilities.ProjectCreation;
using TUnit.Core.Exceptions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Builds the <c>DovetailJoint</c>, <c>DovetailHost</c> and <c>DovetailTemplate</c> declarations that the reference pages under
/// <c>docs/src/content/docs/reference/</c> describe, for the metadata no project in <c>src/</c> or <c>sample/</c> sets (the pages
/// take every other example straight from those projects). Each application calls the generated API, so the build proves the
/// signatures the pages state, and the defaults are read back from the generated code or from the evaluated items. The tests at
/// the end are the pages' "common mistakes": each one is a declaration that doesn't compile, with the error it fails with.
/// </summary>
[TargetFrameworks]
public class DovetailItemReferenceTests(string targetFramework)
{
    /// <summary>
    /// The defaults in the <c>DovetailJoint</c> table. The prepare target is the only place they are applied, so the items are read after
    /// it ran (<c>-t:_DovetailPrepareJointItems</c>): a metadata value that was never set stays empty.
    /// </summary>
    [Test]
    public async Task DovetailJoint_Defaults_AreTheOnesTheReferenceStates()
    {
        using var project = CreateProject(
            J("Bare"),
            J("Typed", "ParameterType", "global::System.IDisposable"),
            J("Generic", "GenericParameter", "T"),
            J("Explicit", "Namespace", "Custom.Joints", "IsAsync", "false", "ParameterType", "global::System.IDisposable", "ParameterName", "thing", "GenericParameter", "T", "GenericVariance", "in")
        );

        var items = ( await project.EvaluateItems("worker", "DovetailJoint", "-t:_DovetailPrepareJointItems") ).ToDictionary(z => z["Identity"]);

        Metadata(items["Bare"], "Namespace").ShouldBe("worker.Joints");
        Metadata(items["Bare"], "IsAsync").ShouldBe("true");
        Metadata(items["Bare"], "ParameterName").ShouldBe("", "ParameterName only defaults when there is a ParameterType");
        Metadata(items["Bare"], "GenericVariance").ShouldBe("", "GenericVariance only defaults when there is a GenericParameter");
        Metadata(items["Typed"], "ParameterName").ShouldBe("parameter");
        Metadata(items["Generic"], "GenericVariance").ShouldBe("in");
        Metadata(items["Explicit"], "Namespace").ShouldBe("Custom.Joints");
        Metadata(items["Explicit"], "IsAsync").ShouldBe("false");
        Metadata(items["Explicit"], "ParameterName").ShouldBe("thing");
    }

    /// <summary>
    /// One joint per row of the <c>DovetailJoint</c> table that no real project exercises. <c>Probe</c> calls every generated type and method
    /// with the parameter lists the page states; if the targets generated anything else this project would not compile.
    /// </summary>
    [Test]
    public async Task DovetailJoint_Metadata_ShapesTheGeneratedJoint()
    {
        using var project = CreateProject(
            // Nothing but the name: the context is the only parameter, and the namespace is $(RootNamespace).Joints.
            J("Minimal"),
            J("Wired", "ProvideConfiguration", "true", "ProvideServices", "true"),
            J(
                "Builder",
                "Namespace",
                "Custom.Joints",
                "ProvideServices",
                "true",
                "ParameterType",
                "global::Microsoft.Extensions.Logging.ILoggingBuilder",
                "ParameterName",
                "builder",
                "BuilderMethodName",
                "ConfigureTheBuilder"
            ),
            J("Generic", "GenericParameter", "TBuilder", "GenericConstraint", "global::Microsoft.Extensions.Hosting.IHostApplicationBuilder", "ParameterType", "TBuilder", "ParameterName", "builder"),
            J("SyncOnly", "IsAsync", "false"),
            J("Chained", "ReturnFirstArgument", "true"),
            // Written as given: this one is not prefixed with global:: in the generated code, and still compiles.
            J("Unprefixed", "ParameterType", "Microsoft.Extensions.Logging.ILoggingBuilder", "ParameterName", "builder")
        );
        project.AddFile(
            "worker/Probe.cs",
            """
            using System.Threading;
            using System.Threading.Tasks;
            using Custom.Joints;
            using Joinery.Dovetail;
            using Microsoft.Extensions.Configuration;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.Hosting;
            using Microsoft.Extensions.Logging;
            using worker.Joints;

            namespace worker;

            internal static class Probe
            {
                public static async Task Minimal(IDovetailContext context)
                {
                    await context.ApplyMinimal();
                    IMinimalJoint joint = null!;
                    IMinimalAsyncJoint asyncJoint = null!;
                    MinimalJointDelegate @delegate = static (IDovetailContext c) => { };
                    MinimalAsyncJointDelegate asyncDelegate = static (IDovetailContext c, CancellationToken ct) => ValueTask.CompletedTask;
                    DovetailContextBuilder.Create().ConfigureMinimal(@delegate).ConfigureMinimal(asyncDelegate);
                }

                // The context, then IConfiguration, then IServiceCollection; the subject of ApplyWired is the service collection.
                public static async Task Wired(IDovetailContext context, IServiceCollection services)
                {
                    await services.ApplyWired(context);
                    WiredJointDelegate @delegate = static (IDovetailContext c, IConfiguration configuration, IServiceCollection s) => { };
                    DovetailContextBuilder.Create().ConfigureWired(@delegate);
                }

                // The subject is the custom parameter; the context and the services follow it. The builder method has the name asked for.
                public static async Task Builder(IDovetailContext context, ILoggingBuilder logging, IServiceCollection services)
                {
                    await logging.ApplyBuilder(context, services);
                    BuilderJointDelegate @delegate = static (IDovetailContext c, IServiceCollection s, ILoggingBuilder builder) => { };
                    DovetailContextBuilder.Create().ConfigureTheBuilder(@delegate);
                }

                // The type parameter is contravariant (in) by default, and the constraint is the one asked for.
                public static async Task Generic(IDovetailContext context, HostApplicationBuilder builder)
                {
                    await builder.ApplyGeneric(context);
                    IGenericJoint<IHostApplicationBuilder> joint = null!;
                    IGenericJoint<HostApplicationBuilder> narrower = joint;
                    DovetailContextBuilder.Create().ConfigureGeneric<HostApplicationBuilder>(static (IDovetailContext c, HostApplicationBuilder b) => { });
                }

                // ApplySyncOnly returns void, and there is no ISyncOnlyAsyncJoint.
                public static void SyncOnly(IDovetailContext context)
                {
                    context.ApplySyncOnly();
                    ISyncOnlyJoint joint = null!;
                    DovetailContextBuilder.Create().ConfigureSyncOnly(static (IDovetailContext c) => { });
                }

                public static async Task<IDovetailContext> Chained(IDovetailContext context) => await context.ApplyChained();

                public static async Task Unprefixed(IDovetailContext context, ILoggingBuilder logging) => await logging.ApplyUnprefixed(context);
            }
            """
        );

        await project.BuildProject("worker");

        var objDirectory = $"worker/obj/Debug/{targetFramework}";
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailJoint.SyncOnly.g.cs")).ShouldBeTrue();
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailJointAsync.SyncOnly.g.cs")).ShouldBeFalse("IsAsync=false leaves out the async file");
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailJointAsync.Minimal.g.cs")).ShouldBeTrue();

        var unprefixed = project.ReadFile($"{objDirectory}/DovetailJoint.Unprefixed.g.cs");
        unprefixed.ShouldContain("(global::Joinery.Dovetail.IDovetailContext context, Microsoft.Extensions.Logging.ILoggingBuilder builder);");
        unprefixed.ShouldNotContain("global::Microsoft.Extensions.Logging.ILoggingBuilder");

        project.ReadFile($"{objDirectory}/DovetailJoint.Minimal.g.cs").ShouldContain("namespace worker.Joints;");
        project.ReadFile($"{objDirectory}/DovetailJoint.Builder.g.cs").ShouldContain("namespace Custom.Joints;");
    }

    /// <summary>
    /// The three hosts differ in what the <c>DovetailHost</c> table calls defaults: <c>Mine</c> sets nothing but the two required types
    /// (so its namespace is $(RootNamespace), its helper is <c>DovetailMineHelpers</c> and it builds with the default expression),
    /// <c>Custom</c> splices more arguments into the call, and <c>NoBuild</c> leaves the expression out. Each helper is the project's own.
    /// </summary>
    [Test]
    public async Task DovetailHost_Metadata_ShapesTheGeneratedEntryPoints()
    {
        using var project = CreateProject(
            H("Mine"),
            H("Custom", "Namespace", "Hosts.Custom", "HelperName", "global::worker.CustomHelper", "BuildExpression", ", static b => b.Build(), \"extra\""),
            H("NoBuild", "Namespace", "Hosts.NoBuild", "HelperName", "global::worker.NoBuildHelper", "HasBuildExpression", "false", "BuildExpression", ", static b => b.Build()")
        );
        project.AddFile(
            "worker/Helpers.cs",
            """
            using System.Threading;
            using System.Threading.Tasks;
            using Joinery.Dovetail;
            using Microsoft.Extensions.Hosting;

            namespace worker;

            internal static class DovetailMineHelpers
            {
                public static ValueTask<IHost> Configure(HostApplicationBuilder builder, System.Func<HostApplicationBuilder, IHost> build, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
                    => ValueTask.FromResult(build(builder));
            }

            internal static class CustomHelper
            {
                public static ValueTask<IHost> Configure(HostApplicationBuilder builder, System.Func<HostApplicationBuilder, IHost> build, string extra, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
                    => ValueTask.FromResult(build(builder));
            }

            internal static class NoBuildHelper
            {
                public static ValueTask<IHost> Configure(HostApplicationBuilder builder, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
                    => ValueTask.FromResult(builder.Build());
            }

            internal static class Probe
            {
                // The three hosts share a builder type, so each has its own namespace (or is called through its class): see "common mistakes".
                public static async Task Run(string[] args)
                {
                    var mine = await GeneratedDovetailMineExtensions.ConfigureDovetail(Host.CreateApplicationBuilder(args));
                    var custom = await Hosts.Custom.GeneratedDovetailCustomExtensions.ConfigureDovetail(Host.CreateApplicationBuilder(args), static contextBuilder => { });
                    var noBuild = await Hosts.NoBuild.GeneratedDovetailNoBuildExtensions.ConfigureDovetail(Host.CreateApplicationBuilder(args));
                }
            }
            """
        );

        await project.BuildProject("worker");

        var objDirectory = $"worker/obj/Debug/{targetFramework}";
        var mine = project.ReadFile($"{objDirectory}/DovetailHost.Mine.g.cs");
        mine.ShouldContain("namespace worker;");
        mine.ShouldContain("return await DovetailMineHelpers.Configure(builder, static b => b.Build(), contextBuilder, cancellationToken);");
        mine.ShouldContain("internal static partial class GeneratedDovetailMineExtensions");
        project.ReadFile($"{objDirectory}/DovetailHost.Custom.g.cs").ShouldContain("namespace Hosts.Custom;");
        project.ReadFile($"{objDirectory}/DovetailHost.Custom.g.cs").ShouldContain("return await global::worker.CustomHelper.Configure(builder, static b => b.Build(), \"extra\", contextBuilder, cancellationToken);");
        project.ReadFile($"{objDirectory}/DovetailHost.NoBuild.g.cs").ShouldContain("return await global::worker.NoBuildHelper.Configure(builder, contextBuilder, cancellationToken);");

        // One DovetailContextBuilder.Create(), however many hosts there are.
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailContextBuilder.g.cs")).ShouldBeTrue();
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailHost.Mine.Sync.g.cs")).ShouldBeFalse("SyncEntryPoint is not set");
        File.Exists(Path.Combine(project.Directory, objDirectory, "DovetailHost.Mine.Navigate.g.cs")).ShouldBeFalse("NavigateEntryPoint is not set");
    }

    /// <summary>
    /// <c>ParameterType</c> is written into the generated code as given, so a name without <c>global::</c> is looked up from the generated
    /// namespace outwards, and the first namespace on the way with a member of that name wins. <c>Custom.Microsoft</c> captures the
    /// <c>Microsoft</c> of a joint in <c>Custom.Joints</c>; the joint that says <c>global::</c> is not affected.
    /// </summary>
    [Test]
    public async Task DovetailJoint_UnprefixedParameterType_IsCapturedByANamespaceOfTheSameName()
    {
        using var project = CreateProject(
            J("Captured", "Namespace", "Custom.Joints", "ParameterType", "Microsoft.Extensions.Logging.ILoggingBuilder", "ParameterName", "builder"),
            J("Safe", "Namespace", "Custom.Joints", "ParameterType", "global::Microsoft.Extensions.Logging.ILoggingBuilder", "ParameterName", "builder")
        );
        project.AddFile("worker/Capture.cs", "namespace Custom.Microsoft { internal sealed class Capture { } }");

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS0234");
        failure.Message.ShouldContain("DovetailJoint.Captured.g.cs");
        failure.Message.ShouldNotContain("DovetailJoint.Safe.g.cs");
    }

    /// <summary>
    /// <c>GenericVariance</c> is the modifier on the type parameter of the generated interface and delegates, whose only use is as an input of
    /// <c>Register</c>: only <c>in</c> is valid there.
    /// </summary>
    [Test]
    public async Task DovetailJoint_CovariantGenericParameter_DoesNotCompile()
    {
        using var project = CreateProject(J("Generic", "GenericParameter", "T", "GenericVariance", "out", "ParameterType", "T", "ParameterName", "value"));

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS1961");
    }

    /// <summary>
    /// <c>ReturnFirstArgument</c> makes <c>Apply{Joint}</c> return what the executor returns, the <c>IDovetailContext</c>, as the
    /// <c>ValueTask</c>'s result, which only matches a joint whose first argument is the context itself.
    /// </summary>
    [Test]
    public async Task DovetailJoint_ReturnFirstArgumentOnAnythingButTheContext_DoesNotCompile()
    {
        using var project = CreateProject(
            J("WithParameter", "ReturnFirstArgument", "true", "ParameterType", "global::Microsoft.Extensions.Logging.ILoggingBuilder", "ParameterName", "builder"),
            J("WithServices", "ReturnFirstArgument", "true", "ProvideServices", "true")
        );

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS0029");
        failure.Message.ShouldContain("ValueTask<Microsoft.Extensions.Logging.ILoggingBuilder>");
        failure.Message.ShouldContain("ValueTask<Microsoft.Extensions.DependencyInjection.IServiceCollection>");
    }

    /// <summary>
    /// A host's generated code calls <c>Dovetail{Name}Helpers.Configure</c> unless <c>HelperName</c> names another class, so a host that
    /// is named for nothing the Hosting package emits and sets no <c>HelperName</c> refers to a class that doesn't exist.
    /// </summary>
    [Test]
    public async Task DovetailHost_WithoutAHelperOfThatName_DoesNotCompile()
    {
        using var project = CreateProject(H("Orphan"));

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS0103");
        failure.Message.ShouldContain("DovetailOrphanHelpers");
    }

    /// <summary>
    /// <c>HostBuilderType</c> and <c>HostType</c> have no default: the template writes them into every signature as they are, so a host
    /// without them generates a file that isn't valid C#.
    /// </summary>
    [Test]
    public async Task DovetailHost_WithoutHostBuilderTypeAndHostType_DoesNotCompile()
    {
        using var project = CreateProject(("DovetailHost", "Mine", Pairs(["HelperName", "global::Joinery.Dovetail.DovetailHostApplicationHelpers"])));

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS1001");
        failure.Message.ShouldContain("DovetailHost.Mine.g.cs");
    }

    /// <summary>
    /// Two hosts that extend the same builder type, in one namespace, both define <c>ConfigureDovetail(builder)</c>: the call is ambiguous. The SDK
    /// already declares <c>HostApplication</c> in an application, so a second host on <c>HostApplicationBuilder</c> needs a namespace of its own.
    /// </summary>
    [Test]
    public async Task DovetailHost_SecondHostOnTheSameBuilderTypeAndNamespace_MakesTheEntryPointAmbiguous()
    {
        using var project = CreateProject(H("Second", "HelperName", "global::Joinery.Dovetail.DovetailHostApplicationHelpers"));
        project.AddFile(
            "worker/Probe.cs",
            """
            using Microsoft.Extensions.Hosting;

            namespace worker;

            internal static class Probe
            {
                public static async System.Threading.Tasks.Task Run(string[] args) => await Host.CreateApplicationBuilder(args).ConfigureDovetail();
            }
            """
        );

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain("CS0121");
    }

    /// <summary>
    /// The rows of the <c>DovetailTemplate</c> table that gate or shape a template without a package of their own (the package and
    /// direct-reference gates are <see cref="TemplateExtensionPointTests" />' business). The project is a generic-host application that
    /// references Microsoft.Extensions.Hosting, so it declares the <c>HostApplication</c> host and has that assembly among its references.
    /// </summary>
    [Test]
    public async Task DovetailTemplate_Metadata_DecidesWhichTemplatesAreWrittenAndHowTheyAreNamed()
    {
        string[] names = ["Plain", "Prefixed", "Referenced", "MissingReference", "ForThisHost", "ForOtherHosts", "PerHost", "NameOnly"];
        using var project = CreateProject(
            T("Plain"),
            T("Prefixed", "OutputPrefix", "Contoso"),
            T("Referenced", "References", "Microsoft.Extensions.Hosting"),
            T("MissingReference", "References", "Microsoft.Extensions.Hosting;Not.Referenced.Anywhere"),
            T("ForThisHost", "Hosts", "Maui;HostApplication"),
            T("ForOtherHosts", "Hosts", "Maui;Uno"),
            T("PerHost", "HostName", "Mine", "HostType", "global::System.Object"),
            T("NameOnly", "HostName", "Mine")
        );
        foreach (var name in names)
        {
            // The placeholders are in a comment, so the file compiles whether or not they are replaced.
            project.AddFile($"worker/{name}.cs.template", $"namespace worker.Emitted;{Environment.NewLine}internal static class {name} {{ }}{Environment.NewLine}// {{HostName}} {{HostType}}{Environment.NewLine}");
        }

        await project.BuildProject("worker");

        var objDirectory = Path.Combine(project.Directory, "worker", "obj", "Debug", targetFramework);
        Directory
           .EnumerateFiles(objDirectory, "*.g.cs")
           .Select(Path.GetFileName)
           .Where(z => z!.StartsWith("DovetailTemplate.", StringComparison.Ordinal) || z.StartsWith("Contoso.", StringComparison.Ordinal))
           .Order(StringComparer.Ordinal)
           .ShouldBe(
                [
                    "Contoso.Prefixed.g.cs",
                    "DovetailTemplate.ForThisHost.g.cs",
                    "DovetailTemplate.NameOnly.g.cs",
                    "DovetailTemplate.PerHost.g.cs",
                    "DovetailTemplate.Plain.g.cs",
                    "DovetailTemplate.Referenced.g.cs",
                ]
            );
        File.ReadAllText(Path.Combine(objDirectory, "DovetailTemplate.PerHost.g.cs")).ShouldContain("// Mine global::System.Object");
        File.ReadAllText(Path.Combine(objDirectory, "DovetailTemplate.NameOnly.g.cs")).ShouldContain("// {HostName} {HostType}");
    }

    /// <summary>
    /// The three ways to get a <c>DovetailTemplate</c> wrong that fail the build instead of being ignored.
    /// </summary>
    [Test]
    [Arguments("NoFile", "has no File.")]
    [Arguments("DirectReferenceOnlyWithoutPackage", "is DirectReferenceOnly, so it needs the Package it is direct to.")]
    [Arguments("FileThatDoesNotExist", "names a file that doesn't exist")]
    public async Task DovetailTemplate_Mistakes_FailTheBuild(string mistake, string message)
    {
        using var project = CreateProject(
            ("DovetailTemplate", mistake, mistake switch
            {
                "NoFile" => Pairs([]),
                "DirectReferenceOnlyWithoutPackage" => Pairs(["File", $"$(MSBuildThisFileDirectory){mistake}.cs.template", "DirectReferenceOnly", "true"]),
                _ => Pairs(["File", $"$(MSBuildThisFileDirectory){mistake}.cs.template"]),
            })
        );

        var failure = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("worker"));

        failure.Message.ShouldContain($"the template '{mistake}' {message}");
    }

    private SdkTestProject CreateProject(params (string Type, string Name, Dictionary<string, string?> Metadata)[] items)
    {
        var project = new SdkTestProject();
        var worker = ProjectCreator
           .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
           // The generic host's own package, which a HostApplicationBuilder application references (see HostingHelperTests).
           .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
           .ItemPackageReference("Microsoft.Extensions.Logging", Config.CentralPackageVersion("Microsoft.Extensions.Logging", targetFramework));
        foreach (var (type, name, metadata) in items)
        {
            worker = worker.ItemInclude(type, name, metadata: metadata);
        }

        return project
              .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
              .AddProject("worker/worker.csproj", worker)
              .AddFile("worker/Program.cs", "System.Console.WriteLine(\"hello\");");
    }

    /// <summary>A <c>DovetailTemplate</c> item whose template is <c>{name}.cs.template</c> next to the project, with more metadata given as name, value pairs.</summary>
    private static (string Type, string Name, Dictionary<string, string?> Metadata) T(string name, params string[] metadata)
    {
        var pairs = Pairs(metadata);
        pairs["File"] = $"$(MSBuildThisFileDirectory){name}.cs.template";
        return ("DovetailTemplate", name, pairs);
    }

    /// <summary>A <c>DovetailJoint</c> item, with metadata given as name, value pairs.</summary>
    private static (string Type, string Name, Dictionary<string, string?> Metadata) J(string name, params string[] metadata) => ("DovetailJoint", name, Pairs(metadata));

    /// <summary>
    /// A <c>DovetailHost</c> item on <c>HostApplicationBuilder</c>, with metadata given as name, value pairs. The helper of a host named
    /// <c>Mine</c> is the project's own <c>DovetailMineHelpers</c>, so the builder and host types are the generic host's.
    /// </summary>
    private static (string Type, string Name, Dictionary<string, string?> Metadata) H(string name, params string[] metadata)
    {
        var pairs = Pairs(metadata);
        pairs["HostBuilderType"] = "global::Microsoft.Extensions.Hosting.HostApplicationBuilder";
        pairs["HostType"] = "global::Microsoft.Extensions.Hosting.IHost";
        return ("DovetailHost", name, pairs);
    }

    private static Dictionary<string, string?> Pairs(string[] pairs)
    {
        var metadata = new Dictionary<string, string?>();
        for (var i = 0; i < pairs.Length; i += 2)
        {
            metadata[pairs[i]] = pairs[i + 1];
        }

        return metadata;
    }

    private static string Metadata(IReadOnlyDictionary<string, string> item, string name) => item.GetValueOrDefault(name, "");
}
