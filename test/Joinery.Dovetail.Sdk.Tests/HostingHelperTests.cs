using Microsoft.Build.Utilities.ProjectCreation;
using TUnit.Core.Exceptions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Joinery.Dovetail.Hosting emits the helpers the generated <c>ConfigureDovetail</c> entry points call from templates
/// (<c>src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets</c>), each only when the assemblies it compiles against
/// are among the project's references, so the package itself depends on no host framework, the generic host's
/// Microsoft.Extensions.Hosting included. These build an application that references the package and one host framework, which also
/// proves the emitted helpers compile there, and read which helpers landed in <c>obj/</c>. The <c>IDovetailContext</c> environment
/// extensions are emitted the same way, but by Joinery.Dovetail itself
/// (<c>src/Joinery.Dovetail/build/Joinery.Dovetail.Environment.targets</c>), so they land in a differently-prefixed generated file;
/// <see cref="BuildAndReadHelpers" /> merges both.
/// </summary>
[TargetFrameworks]
public class HostingHelperTests(string targetFramework)
{
    private const string GenericHost = "DovetailHostApplicationHelpers";

    /// <summary>
    ///     <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c>, for the hosts that run
    ///     <c>IHostedLifecycleService</c>: the generic host and ASP.NET Core, not .NET MAUI or Blazor WebAssembly.
    /// </summary>
    private const string HostLifecycle = "DovetailHostLifecycleHelpers";

    /// <summary>
    ///     <c>context.Environment</c>, which comes with any project referencing Microsoft.Extensions.Hosting.Abstractions (the
    ///     assembly <c>IHostEnvironment</c> lives in) - in practice the generic host, .NET MAUI and Uno Platform, whose helpers
    ///     populate it.
    /// </summary>
    private const string HostingContext = "DovetailHostingContextExtensions";

    [Test]
    public async Task GenericHost_EmitsTheGenericHostHelper_AndContextEnvironment()
    {
        using var project = CreateApplication(app => WithGenericHost(app));

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, HostLifecycle, HostingContext]);
    }

    /// <summary>
    /// The package depends on Microsoft.Extensions.Hosting.Abstractions only, so the generic-host helper (and the SDK's HostApplication
    /// entry points, which call it) only appear once the application references Microsoft.Extensions.Hosting itself.
    /// </summary>
    [Test]
    public async Task WithoutTheGenericHost_EmitsNoHelpers_AndNoEntryPoints()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("app/app.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe"))
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");

        ( await BuildAndReadHelpers(project) ).ShouldBe(WithSharedFrameworkHostingContext());
        GeneratedFiles(project, "DovetailHost.*.g.cs").ShouldBeEmpty();
    }

    [Test]
    public async Task AspNetCore_AlsoEmitsTheSchemaEndpointExtensions()
    {
        using var project = CreateApplication(app => app.ItemInclude("FrameworkReference", "Microsoft.AspNetCore.App"));

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailConfigurationSchemaEndpointExtensions", GenericHost, HostLifecycle, HostingContext]);
    }

    [Test]
    public async Task Maui_EmitsTheMauiHelpers()
    {
        using var project = CreateApplication(app => app.ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework)));

        ( await BuildAndReadHelpers(project) ).ShouldBe([HostingContext, "DovetailMauiHelpers"]);
    }

    [Test]
    public async Task WebAssembly_EmitsTheWebAssemblyHelpers()
    {
        using var project = CreateApplication(app => WebAssembly(app));

        ( await BuildAndReadHelpers(project) ).ShouldBe(WithSharedFrameworkHostingContext("DovetailWebAssemblyContextExtensions", "DovetailWebAssemblyHelpers"));
    }

    /// <summary>
    /// The <c>context.WebAssemblyHostEnvironment</c> extension property is a C# 14 extension member, so a project pinned below C# 14
    /// doesn't get it rather than failing to compile it.
    /// </summary>
    [Test]
    public async Task WebAssembly_BelowCSharp14_LeavesOutTheContextExtensions()
    {
        using var project = CreateApplication(app => WebAssembly(app).Property("LangVersion", "13"));

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailWebAssemblyHelpers"]);
    }

    /// <summary>
    ///     <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c> are only emitted where the host runs
    ///     <c>IHostedLifecycleService</c>: the generic host and ASP.NET Core, and Uno Platform, whose host is a generic host. .NET MAUI and
    ///     Blazor WebAssembly don't run it, so they get nothing, even with Microsoft.Extensions.Hosting among their references (the
    ///     <c>WithGenericHost</c> cases) - methods that compile but never run their handlers would be worse than none. The snapshot
    ///     lists what each host gets, less <c>context.Environment</c>, which depends on the target framework (both share one snapshot).
    /// </summary>
    [Test]
    [Arguments("GenericHost")]
    [Arguments("AspNetCore")]
    [Arguments("Uno")]
    [Arguments("Maui")]
    [Arguments("MauiWithGenericHost")]
    [Arguments("WebAssembly")]
    [Arguments("WebAssemblyWithGenericHost")]
    public async Task HostLifecycle_IsEmittedOnlyForTheHostsThatRunIt(string host)
    {
        using var project = CreateApplication(
            app =>
            {
                switch (host)
                {
                    case "GenericHost":
                        WithGenericHost(app);
                        break;
                    case "AspNetCore":
                        app.ItemInclude("FrameworkReference", "Microsoft.AspNetCore.App");
                        break;
                    case "Uno":
                        Uno(app);
                        break;
                    case "Maui":
                        WithMaui(app);
                        break;
                    case "MauiWithGenericHost":
                        WithMaui(WithGenericHost(app));
                        break;
                    case "WebAssembly":
                        WebAssembly(app);
                        break;
                    case "WebAssemblyWithGenericHost":
                        WebAssembly(WithGenericHost(app));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(host), host, null);
                }
            }
        );

        var helpers = ( await BuildAndReadHelpers(project) ).Where(z => z != HostingContext).ToArray();

        // Both target frameworks share one snapshot per host.
        var settings = new VerifySettings();
        settings.IgnoreConstructorParameters();
        await Verify(new { Host = host, LifecycleHelperEmitted = helpers.Contains(HostLifecycle), Helpers = helpers }, settings: settings);
    }

    /// <summary>
    /// The Uno host's <c>ConfigureDovetail&lt;TShell&gt;</c> entry points navigate with Uno.Extensions.Navigation, so they (and the helper
    /// they call) are only emitted when it is referenced; the Dovetail SDK declares them for every Uno app with Hosting.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Uno_EmitsTheNavigateEntryPoint_OnlyWithNavigation(bool navigation)
    {
        using var project = CreateApplication(
            app =>
            {
                Uno(app);
                if (navigation)
                {
                    app.ItemPackageReference("Uno.Extensions.Navigation.WinUI", Config.CentralPackageVersion("Uno.Extensions.Navigation.WinUI", targetFramework));
                }
            }
        );

        var helpers = await BuildAndReadHelpers(project);

        helpers.ShouldBe(
            navigation ? [GenericHost, HostLifecycle, HostingContext, "DovetailUnoHelpers", "DovetailUnoNavigationHelpers"] : [GenericHost, HostLifecycle, HostingContext, "DovetailUnoHelpers"]
        );
        GeneratedFiles(project, "DovetailHost.Uno.Navigate.g.cs").ShouldBe(navigation ? ["DovetailHost.Uno.Navigate.g.cs"] : []);
    }

    /// <summary>
    /// The SDK declares a WebAssembly host for every Microsoft.NET.Sdk.BlazorWebAssembly or Microsoft.NET.Sdk.WebAssembly project. Its
    /// entry points call the WebAssembly helper, so without Blazor's Microsoft.AspNetCore.Components.WebAssembly the project gets none,
    /// and still builds.
    /// </summary>
    [Test]
    public async Task WebAssemblyHost_WithoutBlazor_GetsNoEntryPoints()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("app/app.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.BlazorWebAssembly"))
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");

        ( await BuildAndReadHelpers(project) ).ShouldBe(WithSharedFrameworkHostingContext());
        GeneratedFiles(project, "DovetailHost.*.g.cs").ShouldBeEmpty();
    }

    /// <summary>
    ///     A generic-host application that references Joinery.Dovetail.OpenTelemetry turns OpenTelemetry on with
    ///     OpenTelemetry.Extensions.Hosting's <c>AddOpenTelemetry()</c>, which starts the providers with the host.
    /// </summary>
    [Test]
    public async Task GenericHost_WithOpenTelemetry_EmitsTheOpenTelemetryJoint()
    {
        using var project = CreateApplication(app => WithOpenTelemetryHosting(WithDovetailOpenTelemetry(WithGenericHost(app))));

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, HostLifecycle, HostingContext, "DovetailOpenTelemetryJoint"]);
    }

    [Test]
    public async Task GenericHost_WithOpenTelemetry_WithoutOpenTelemetryHosting_FailsToCompile()
    {
        using var project = CreateApplication(app => WithDovetailOpenTelemetry(WithGenericHost(app)));

        var exception = await Should.ThrowAsync<TUnitException>(() => project.BuildProject("app"));

        exception.Message.ShouldContain("error CS1029");
        exception.Message.ShouldContain("Add a PackageReference to OpenTelemetry.Extensions.Hosting");
    }

    /// <summary>
    ///     .NET MAUI doesn't run hosted services, so OpenTelemetry.Extensions.Hosting couldn't start OpenTelemetry: the app adds it to
    ///     the services directly, and a start joint resolves the providers once the app is built.
    /// </summary>
    [Test]
    public async Task Maui_WithOpenTelemetry_EmitsTheServicesJoint_AndTheStartJoint()
    {
        using var project = CreateApplication(
            app => WithDovetailOpenTelemetry(app.ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework)))
        );

        ( await BuildAndReadHelpers(project) ).ShouldBe([HostingContext, "DovetailMauiHelpers", "DovetailOpenTelemetryMauiStartJoint", "DovetailOpenTelemetryServicesJoint"]);
    }

    /// <summary>
    ///     The <c>#error</c> is only for an application none of whose hosts can turn OpenTelemetry on: a MAUI app that also gets
    ///     Microsoft.Extensions.Hosting (often through another package) still adds OpenTelemetry to its services without
    ///     OpenTelemetry.Extensions.Hosting.
    /// </summary>
    [Test]
    public async Task Maui_WithTheGenericHost_WithoutOpenTelemetryHosting_EmitsTheServicesJoint()
    {
        using var project = CreateApplication(
            app => WithDovetailOpenTelemetry(
                WithGenericHost(app).ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework))
            )
        );

        ( await BuildAndReadHelpers(project) ).ShouldBe(
            [GenericHost, HostingContext, "DovetailMauiHelpers", "DovetailOpenTelemetryMauiStartJoint", "DovetailOpenTelemetryServicesJoint"]
        );
    }

    /// <summary>
    ///     Blazor WebAssembly only runs hosted services from .NET 11, so it doesn't need OpenTelemetry.Extensions.Hosting either.
    /// </summary>
    [Test]
    public async Task WebAssembly_WithOpenTelemetry_EmitsTheServicesJoint_AndTheStartJoint()
    {
        using var project = CreateApplication(app => WithDovetailOpenTelemetry(WebAssembly(app)));

        ( await BuildAndReadHelpers(project) ).ShouldBe(
            WithSharedFrameworkHostingContext(
                "DovetailOpenTelemetryServicesJoint",
                "DovetailOpenTelemetryWebAssemblyStartJoint",
                "DovetailWebAssemblyContextExtensions",
                "DovetailWebAssemblyHelpers"
            )
        );
    }

    /// <summary>
    ///     An application that references OpenTelemetry.Extensions.Hosting gets its <c>AddOpenTelemetry()</c> whatever the host, and
    ///     still the start joint where the host might not run its hosted service (WebAssembly before .NET 11).
    /// </summary>
    [Test]
    public async Task WebAssembly_WithOpenTelemetryHosting_EmitsTheOpenTelemetryJoint_AndTheStartJoint()
    {
        using var project = CreateApplication(app => WithOpenTelemetryHosting(WithDovetailOpenTelemetry(WebAssembly(app))));

        ( await BuildAndReadHelpers(project) ).ShouldBe(
            [HostingContext, "DovetailOpenTelemetryJoint", "DovetailOpenTelemetryWebAssemblyStartJoint", "DovetailWebAssemblyContextExtensions", "DovetailWebAssemblyHelpers"]
        );
    }

    /// <summary>
    ///     Uno Platform uses the generic host, but its host only runs hosted services once started (<c>NavigateAsync</c> does,
    ///     <c>ConfigureDovetail</c> doesn't), so it isn't asked for OpenTelemetry.Extensions.Hosting.
    /// </summary>
    [Test]
    public async Task Uno_WithOpenTelemetry_WithoutOpenTelemetryHosting_EmitsTheServicesJoint_AndTheStartJoint()
    {
        using var project = CreateApplication(app => WithDovetailOpenTelemetry(Uno(app)));

        ( await BuildAndReadHelpers(project) ).ShouldBe(
            [GenericHost, HostLifecycle, HostingContext, "DovetailOpenTelemetryServicesJoint", "DovetailOpenTelemetryUnoStartJoint", "DovetailUnoHelpers"]
        );
    }

    /// <summary>
    ///     A library's joint would be exported to every application that references it, so only applications (and test projects) get the
    ///     OpenTelemetry joint, and a library isn't asked for OpenTelemetry.Extensions.Hosting.
    /// </summary>
    [Test]
    public async Task Library_WithOpenTelemetry_GetsNoOpenTelemetryJoint()
    {
        var library = ProjectCreator
                     .Templates.SdkCsproj(targetFramework: targetFramework)
                     .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"));
        WithDovetailOpenTelemetry(WithGenericHost(library));
        using var project = new SdkTestProject();
        project.AddProject("app/app.csproj", library);

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, HostLifecycle, HostingContext]);
    }

    /// <summary>
    ///     The Dovetail SDK adds Joinery.Dovetail.OpenTelemetry to a project that references OpenTelemetry (or
    ///     OpenTelemetry.Extensions.Hosting), so the application gets the OpenTelemetry joint without referencing it.
    /// </summary>
    [Test]
    public async Task Sdk_AddsDovetailOpenTelemetry_WhenOpenTelemetryIsReferenced()
    {
        using var project = CreateSdkApplication(app => WithOpenTelemetryHosting(WithGenericHost(app)));

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, HostLifecycle, HostingContext, "DovetailOpenTelemetryJoint"]);
    }

    [Test]
    public async Task Sdk_DovetailEnableOpenTelemetryFalse_LeavesOpenTelemetryToTheApplication()
    {
        using var project = CreateSdkApplication(
            app => WithGenericHost(app)
                  .Property("DovetailEnableOpenTelemetry", "false")
                  .ItemPackageReference("OpenTelemetry", Config.CentralPackageVersion("OpenTelemetry", targetFramework))
        );

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, HostLifecycle, HostingContext]);
    }

    private ProjectCreator WithGenericHost(ProjectCreator app) =>
        app.ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework));

    private static ProjectCreator WithDovetailOpenTelemetry(ProjectCreator app) =>
        app.ItemPackageReference("Joinery.Dovetail.OpenTelemetry", Config.DovetailPackageVersion("Joinery.Dovetail.OpenTelemetry"));

    private ProjectCreator WithOpenTelemetryHosting(ProjectCreator app) =>
        app.ItemPackageReference("OpenTelemetry.Extensions.Hosting", Config.CentralPackageVersion("OpenTelemetry.Extensions.Hosting", targetFramework));

    // A plain Microsoft.NET.Sdk application stands in for an Uno app, which would need Uno.Sdk and its workloads.
    private ProjectCreator Uno(ProjectCreator app) =>
        app.Property("NoWarn", "$(NoWarn);UNOB0025")
           .Property("UnoDisableUNOB0029Validation", "true")
           .ItemPackageReference("Uno.Extensions.Hosting.WinUI", Config.CentralPackageVersion("Uno.Extensions.Hosting.WinUI", targetFramework))
           .ItemInclude(
                "DovetailHost",
                "Uno",
                metadata: new Dictionary<string, string?>
                {
                    ["HostBuilderType"] = "global::Uno.Extensions.Hosting.IApplicationBuilder",
                    ["HostBuilderName"] = "builder",
                    ["HostType"] = "global::Microsoft.Extensions.Hosting.IHost",
                    ["HelperName"] = "global::Joinery.Dovetail.DovetailUnoHelpers",
                    ["BuildExpression"] = ", static b => b.Build(), typeof(GeneratedDovetailUnoExtensions).Assembly",
                    ["NavigateEntryPoint"] = "true",
                }
            );

    /// <summary>
    ///     From .NET 11 Microsoft.Extensions.Hosting.Abstractions is part of the shared framework, so every project gets
    ///     <c>context.Environment</c>, not only the ones whose host puts an <c>IHostEnvironment</c> in the context.
    /// </summary>
    private string[] WithSharedFrameworkHostingContext(params string[] helpers) =>
        Version.Parse(targetFramework["net".Length..]).Major >= 11 ? [.. helpers.Append(HostingContext).Order(StringComparer.Ordinal)] : helpers;

    private ProjectCreator WithMaui(ProjectCreator app) =>
        app.ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework));

    private ProjectCreator WebAssembly(ProjectCreator app) =>
        app.ItemPackageReference(
            "Microsoft.AspNetCore.Components.WebAssembly",
            Config.CentralPackageVersion("Microsoft.AspNetCore.Components.WebAssembly", targetFramework)
        );

    private SdkTestProject CreateApplication(Action<ProjectCreator>? configure = null)
    {
        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"));
        configure?.Invoke(app);

        var project = new SdkTestProject();
        project
           .AddProject("app/app.csproj", app)
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        return project;
    }

    private SdkTestProject CreateSdkApplication(Action<ProjectCreator> configure)
    {
        var app = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe");
        configure(app);

        var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("app/app.csproj", app)
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        return project;
    }

    private static async Task<IReadOnlyList<string>> BuildAndReadHelpers(SdkTestProject project)
    {
        await project.BuildProject("app");
        return GeneratedFiles(project, "DovetailHosting.*.g.cs")
              .Select(z => z["DovetailHosting.".Length..^".g.cs".Length])
              .Concat(GeneratedFiles(project, "DovetailEnvironment.*.g.cs").Select(z => z["DovetailEnvironment.".Length..^".g.cs".Length]))
              .Order(StringComparer.Ordinal)
              .ToArray();
    }

    private static IReadOnlyList<string> GeneratedFiles(SdkTestProject project, string pattern) =>
        Directory
           .EnumerateFiles(Path.Combine(project.Directory, "app", "obj"), pattern, SearchOption.AllDirectories)
           .Select(Path.GetFileName)
           .OfType<string>()
           .Order(StringComparer.Ordinal)
           .ToArray();
}
