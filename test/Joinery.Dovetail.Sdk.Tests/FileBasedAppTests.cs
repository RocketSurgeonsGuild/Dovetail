namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// A file-based app (<c>dotnet run app.cs</c>) applies the SDK with a single <c>#:sdk</c> directive: the SDK brings Microsoft.NET.Sdk, which
/// that directive replaces. It is a console application like any other, so it gets the generic host, Joinery.Dovetail.Hosting,
/// Joinery.Dovetail.Serilog and Joinery.Dovetail.OpenTelemetry without a single <c>#:package</c> directive.
/// </summary>
[TargetFrameworks]
public class FileBasedAppTests(string targetFramework)
{
    [Test]
    public async Task FileBasedApp_GetsTheGenericHostSerilogAndOpenTelemetry()
    {
        using var project = new SdkTestProject();
        project.AddFile(
            "app/app.cs",
            $$"""
            #:sdk Joinery.Dovetail.Sdk
            #:property TargetFramework={{targetFramework}}

            using app;
            using Microsoft.Extensions.Hosting;

            using var host = await Host.CreateApplicationBuilder(args).ConfigureDovetail();
            System.Console.WriteLine("Serilog=" + typeof(Joinery.Dovetail.Serilog.SerilogJoint).Assembly.GetName().Name);
            System.Console.WriteLine("OpenTelemetry=" + typeof(Joinery.Dovetail.OpenTelemetry.Joints.IResourceBuilderJoint).Assembly.GetName().Name);
            """
        );

        var output = await project.RunFile("app/app.cs");

        output.ShouldContain("Serilog=Joinery.Dovetail.Serilog");
        output.ShouldContain("OpenTelemetry=Joinery.Dovetail.OpenTelemetry");
    }
}
