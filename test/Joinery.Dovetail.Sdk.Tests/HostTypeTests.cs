using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

[TargetFrameworks]
public class HostTypeTests(string targetFramework)
{
    [Test]
    public async Task WebApplication_DetectsHostAndInjectsHostingWeb()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "web/web.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.Web")
            )
           .AddFile("web/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.VerifyProjects();
    }

    [Test]
    public async Task HostApplication_OutputTypeExe_DetectsHostAndInjectsHosting()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "worker/worker.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
            )
           .AddFile("worker/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.VerifyProjects();
    }

    [Test]
    public async Task WebAssembly_DetectsHostAndInjectsHostingWebAssembly()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "wasm/wasm.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.BlazorWebAssembly")
                   // As every Blazor application does; the generated entry points need it (see HostingHelperTests).
                   .ItemPackageReference(
                        "Microsoft.AspNetCore.Components.WebAssembly",
                        Config.CentralPackageVersion("Microsoft.AspNetCore.Components.WebAssembly", targetFramework)
                    )
            )
           .AddFile("wasm/Program.cs", "System.Console.WriteLine(\"hello\");");

        await project.VerifyProjects();
    }

    [Test]
    public async Task Library_NoOutputType_NoHostDetected()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
            );

        await project.VerifyProjects();
    }
}
