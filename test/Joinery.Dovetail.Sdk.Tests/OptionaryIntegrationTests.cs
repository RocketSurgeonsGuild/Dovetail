using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The whole contract with Optionary, against the pinned package: a library declares an <c>[Optionary]</c> type, Optionary's generator
/// emits its registration method, Dovetail's generator exports a joint that calls it, and a Dovetail web application that references the
/// library resolves <c>IOptions&lt;T&gt;</c> bound from its configuration. With <c>DovetailConfigurationSchemaEndpoint=true</c> the
/// application also serves the schema Optionary merged, with the library's section and Joinery.Dovetail.Serilog's.
/// </summary>
[TargetFrameworks]
public class OptionaryIntegrationTests(string targetFramework)
{
    private const string Program = """
        using System;
        using System.Linq;
        using System.Net.Http;
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Hosting;
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Options;
        using web;

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, EnvironmentName = "Development" });
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        await using var app = await builder.ConfigureDovetail();
        await app.StartAsync();

        Console.WriteLine("Greeting=" + app.Services.GetRequiredService<IOptions<lib.GreetingsOptions>>().Value.Message);

        using var http = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await http.GetAsync("/.well-known/dovetail/configuration-schema");
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Status=" + (int)response.StatusCode);
        Console.WriteLine("HasGreetings=" + body.Contains("\"Greetings\""));
        Console.WriteLine("HasSerilog=" + body.Contains("\"Serilog\""));

        await app.StopAsync();
        """;

    [Test]
    public async Task OptionaryType_ResolvesThroughADovetailHost_AndTheEndpointServesTheMergedSchema()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests).
        var lib = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework).ItemPackageReference("Optionary", Config.OptionaryVersion);
        project
           .AddProject("lib/lib.csproj", lib, track: false)
           .AddFile(
                "lib/GreetingsOptions.cs",
                """
                namespace lib;

                [Optionary.Optionary]
                public class GreetingsOptions
                {
                    public string Message { get; set; } = "default";
                }
                """
            );

        var web = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.Web")
                 .Property("DovetailConfigurationSchemaEndpoint", "true")
                 .ItemPackageReference("Joinery.Dovetail.Serilog", Config.DovetailPackageVersion("Joinery.Dovetail.Serilog"))
                 .ItemProjectReference(lib);
        project
           .AddProject("web/web.csproj", web)
           .AddFile("web/Program.cs", Program)
           .AddFile("web/appsettings.json", """{ "Greetings": { "Message": "hello from configuration" } }""");

        var output = await project.Run("web");

        await output.Should().Contain("Greeting=hello from configuration");
        await output.Should().Contain("Status=200");
        await output.Should().Contain("HasGreetings=True");
        await output.Should().Contain("HasSerilog=True");
    }

    /// <summary>
    /// The endpoint is there, but without Optionary nothing writes a schema for it to serve.
    /// </summary>
    [Test]
    public async Task WithoutOptionary_TheEndpointReturns404()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "web/web.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.Web")
                   .Property("DovetailConfigurationSchemaEndpoint", "true")
            )
           .AddFile(
                "web/Program.cs",
                """
                using System;
                using System.Linq;
                using System.Net.Http;
                using Microsoft.AspNetCore.Builder;
                using Microsoft.AspNetCore.Hosting;
                using web;

                var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, EnvironmentName = "Development" });
                builder.WebHost.UseUrls("http://127.0.0.1:0");
                await using var app = await builder.ConfigureDovetail();
                await app.StartAsync();
                using var http = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
                using var response = await http.GetAsync("/.well-known/dovetail/configuration-schema");
                Console.WriteLine("Status=" + (int)response.StatusCode);
                await app.StopAsync();
                """
            );

        var output = await project.Run("web");

        await output.Should().Contain("Status=404");
    }
}
