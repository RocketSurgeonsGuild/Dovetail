using Microsoft.Build.Utilities.ProjectCreation;
using TUnit.Core.Exceptions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Joinery.Dovetail.Hosting emits the helpers the generated <c>ConfigureDovetail</c> entry points call from templates
/// (<c>src/Joinery.Dovetail.Hosting/build/Joinery.Dovetail.Hosting.targets</c>), each only when the assemblies it compiles against
/// are among the project's references, so the package itself depends on no host framework, the generic host's
/// Microsoft.Extensions.Hosting included. These build an application that references the package and one host framework, which also
/// proves the emitted helpers compile there, and read which helpers landed in <c>obj/</c>.
/// </summary>
[TargetFrameworks]
public class HostingHelperTests(string targetFramework)
{
    private const string GenericHost = "DovetailHostApplicationHelpers";

    [Test]
    public async Task GenericHost_EmitsOnlyTheGenericHostHelper()
    {
        using var project = CreateApplication(app => WithGenericHost(app));

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost]);
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

        ( await BuildAndReadHelpers(project) ).ShouldBeEmpty();
        GeneratedFiles(project, "DovetailHost.*.g.cs").ShouldBeEmpty();
    }

    [Test]
    public async Task AspNetCore_AlsoEmitsTheSchemaEndpointExtensions()
    {
        using var project = CreateApplication(app => app.ItemInclude("FrameworkReference", "Microsoft.AspNetCore.App"));

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailConfigurationSchemaEndpointExtensions", GenericHost]);
    }

    [Test]
    public async Task Maui_EmitsTheMauiHelpers()
    {
        using var project = CreateApplication(app => app.ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework)));

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailMauiHelpers"]);
    }

    [Test]
    public async Task WebAssembly_EmitsTheWebAssemblyHelpers()
    {
        using var project = CreateApplication(app => WebAssembly(app));

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailWebAssemblyContextExtensions", "DovetailWebAssemblyHelpers"]);
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
                // A plain Microsoft.NET.Sdk application stands in for an Uno app, which would need Uno.Sdk and its workloads.
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
                if (navigation)
                {
                    app.ItemPackageReference("Uno.Extensions.Navigation.WinUI", Config.CentralPackageVersion("Uno.Extensions.Navigation.WinUI", targetFramework));
                }
            }
        );

        var helpers = await BuildAndReadHelpers(project);

        helpers.ShouldBe(navigation ? [GenericHost, "DovetailUnoHelpers", "DovetailUnoNavigationHelpers"] : [GenericHost, "DovetailUnoHelpers"]);
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

        ( await BuildAndReadHelpers(project) ).ShouldBeEmpty();
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

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, "DovetailOpenTelemetryJoint"]);
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
    ///     .NET MAUI and Blazor WebAssembly don't use the generic host, so their applications add OpenTelemetry to the services directly.
    /// </summary>
    [Test]
    public async Task Maui_WithOpenTelemetry_EmitsTheServicesJoint()
    {
        using var project = CreateApplication(
            app => WithDovetailOpenTelemetry(app.ItemPackageReference("Microsoft.Maui.Core", Config.CentralPackageVersion("Microsoft.Maui.Core", targetFramework)))
        );

        ( await BuildAndReadHelpers(project) ).ShouldBe(["DovetailMauiHelpers", "DovetailOpenTelemetryServicesJoint"]);
    }

    [Test]
    public async Task WebAssembly_WithOpenTelemetry_EmitsTheServicesJoint()
    {
        using var project = CreateApplication(app => WithDovetailOpenTelemetry(WebAssembly(app)));

        ( await BuildAndReadHelpers(project) ).ShouldBe(
            ["DovetailOpenTelemetryServicesJoint", "DovetailWebAssemblyContextExtensions", "DovetailWebAssemblyHelpers"]
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

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost]);
    }

    /// <summary>
    ///     The Dovetail SDK adds Joinery.Dovetail.OpenTelemetry to a project that references OpenTelemetry (or
    ///     OpenTelemetry.Extensions.Hosting), so the application gets the OpenTelemetry joint without referencing it.
    /// </summary>
    [Test]
    public async Task Sdk_AddsDovetailOpenTelemetry_WhenOpenTelemetryIsReferenced()
    {
        using var project = CreateSdkApplication(app => WithOpenTelemetryHosting(WithGenericHost(app)));

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost, "DovetailOpenTelemetryJoint"]);
    }

    [Test]
    public async Task Sdk_DovetailEnableOpenTelemetryFalse_LeavesOpenTelemetryToTheApplication()
    {
        using var project = CreateSdkApplication(
            app => WithGenericHost(app)
                  .Property("DovetailEnableOpenTelemetry", "false")
                  .ItemPackageReference("OpenTelemetry", Config.CentralPackageVersion("OpenTelemetry", targetFramework))
        );

        ( await BuildAndReadHelpers(project) ).ShouldBe([GenericHost]);
    }

    private ProjectCreator WithGenericHost(ProjectCreator app) =>
        app.ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework));

    private static ProjectCreator WithDovetailOpenTelemetry(ProjectCreator app) =>
        app.ItemPackageReference("Joinery.Dovetail.OpenTelemetry", Config.DovetailPackageVersion("Joinery.Dovetail.OpenTelemetry"));

    private ProjectCreator WithOpenTelemetryHosting(ProjectCreator app) =>
        app.ItemPackageReference("OpenTelemetry.Extensions.Hosting", Config.CentralPackageVersion("OpenTelemetry.Extensions.Hosting", targetFramework));

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
