using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Each Microsoft project SDK (and Aspire.AppHost.Sdk) has a Dovetail mirror in <c>src/Sdks</c>: <c>Joinery.Dovetail.Sdk.Web</c> is <c>Microsoft.NET.Sdk.Web</c> with
/// <c>Joinery.Dovetail.Sdk</c> chained after it, so a project swaps the one name for the other and changes nothing else. These apply the
/// mirror as the project's only SDK, with no Directory.Build.props, and build: the snapshot shows the host Dovetail detected and the packages
/// it added, which are what the Microsoft SDK with <c>Joinery.Dovetail.Sdk</c> next to it gets (<see cref="HostTypeTests" />).
/// </summary>
[TargetFrameworks]
public class MicrosoftSdkMirrorTests(string targetFramework)
{
    [Test]
    [Arguments("Web", null)]
    [Arguments("Worker", null)]
    // A Razor class library, which has no entry point.
    [Arguments("Razor", "Library")]
    [Arguments("BlazorWebAssembly", null)]
    [Arguments("WebAssembly", null)]
    // A plain library: Microsoft.NET.Sdk.WindowsDesktop builds anywhere until the project targets Windows (UseWPF, UseWindowsForms).
    [Arguments("WindowsDesktop", "Library")]
    // An Aspire AppHost: Aspire.AppHost.Sdk comes from NuGet, at the version Directory.Packages.props pins, and adds Aspire.Hosting.AppHost.
    [Arguments("Aspire.AppHost", "Exe")]
    public async Task Mirror_IsTheMicrosoftSdkWithDovetail(string name, string? outputType)
    {
        using var project = new SdkTestProject();
        var app = ProjectCreator
           .Templates.SdkCsproj(
                targetFramework: targetFramework,
                sdk: $"Joinery.Dovetail.Sdk.{name}",
                outputType: outputType
            );
        if (name == "WindowsDesktop")
        {
            // NETSDK1137: Microsoft.NET.Sdk covers Windows desktop projects since .NET 5. The mirror is for the projects that still name it.
            app.Property("NoWarn", "$(NoWarn);NETSDK1137");
        }

        project.AddProject("app/app.csproj", app);
        if (outputType != "Library")
        {
            project.AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        }

        await project.VerifyProjects();
    }

    /// <summary>
    /// An Azure Functions isolated worker: Azure.Functions.Sdk comes from NuGet, at the version Directory.Packages.props pins, and brings
    /// Microsoft.NET.Sdk.Worker. The application references Microsoft.Azure.Functions.Worker itself (Azure.Functions.Sdk asks for it, AZFW0111),
    /// and gets the generic host and the FunctionsApplication host, whose <c>ConfigureDovetail</c> extends
    /// <c>FunctionsApplicationBuilder</c>: Program.cs calls it, so the build proves the entry point and its helper compile.
    /// </summary>
    [Test]
    public async Task Mirror_AzureFunctions_IsTheFunctionsSdkWithDovetail()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "app/app.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Joinery.Dovetail.Sdk.Azure.Functions")
                   .ItemPackageReference("Microsoft.Azure.Functions.Worker", Config.CentralPackageVersion("Microsoft.Azure.Functions.Worker", "net10.0"))
            )
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using Microsoft.Azure.Functions.Worker.Builder;

                using var host = await FunctionsApplication.CreateBuilder(args).ConfigureDovetail();
                """
            );

        await project.VerifyProjects();
    }

    /// <summary>
    /// A Directory.Build.props that applies <c>Joinery.Dovetail.Sdk</c> to every project, as <c>sample/SdkSample</c> does, is imported by the
    /// Microsoft SDK the mirror brings first. The mirror then leaves <c>Joinery.Dovetail.Sdk</c> out rather than importing it a second time.
    /// </summary>
    [Test]
    public async Task Mirror_UnderADirectoryBuildPropsThatAppliesTheDovetailSdk_DoesNotApplyItTwice()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("web/web.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Joinery.Dovetail.Sdk.Web"))
           .AddFile("web/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.VerifyProjects();
        // MSB4011: a file imported a second time, which is what applying Joinery.Dovetail.Sdk twice would log.
        await project.Warnings("web").Should().All(z => !z.StartsWith("MSB4011", StringComparison.Ordinal));
    }
}
