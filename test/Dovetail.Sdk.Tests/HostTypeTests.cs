using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

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
                   .Sdk("Dovetail.Sdk")
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
                   .Sdk("Dovetail.Sdk")
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
                   .Sdk("Dovetail.Sdk")
            )
           .AddProject(
                "wasm/wasm.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.BlazorWebAssembly")
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
                   .Sdk("Dovetail.Sdk")
            )
           .AddProject(
                "lib/lib.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
            );

        await project.VerifyProjects();
    }
}
