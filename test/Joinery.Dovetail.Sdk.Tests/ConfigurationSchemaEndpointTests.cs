using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// <c>DovetailConfigurationSchemaEndpoint=true</c> makes <c>Joinery.Dovetail.Hosting.Web.targets</c> emit a host joint that maps
/// <c>GET /.well-known/dovetail/configuration-schema</c>; with the property off nothing is generated. These build and run a real web
/// application, so the generated joint is compiled, discovered by the source generator and applied by <c>ConfigureDovetail</c>.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaEndpointTests(string targetFramework)
{
    private const string GeneratedFileName = "DovetailConfigurationSchemaEndpoint.g.cs";

    // Starts the application on a free port and requests the endpoint. It writes the schema itself before the request so that the
    // result does not depend on whether the build already produced one (the property under test is only about the endpoint).
    private const string Program = """
        using System;
        using System.IO;
        using System.Linq;
        using System.Net.Http;
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Hosting;
        using Microsoft.Extensions.Hosting;
        using web;

        const string Schema = "{\"type\":\"object\",\"title\":\"sdk-test\"}";

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, EnvironmentName = args.Length > 0 ? args[0] : "Production" });
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        await using var app = await builder.ConfigureDovetail();
        await app.StartAsync();

        File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.schema.json"), Schema);

        using var http = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await http.GetAsync("/.well-known/dovetail/configuration-schema");
        Console.WriteLine("Status=" + (int)response.StatusCode);
        Console.WriteLine("ContentType=" + response.Content.Headers.ContentType?.MediaType);
        Console.WriteLine("Body=" + await response.Content.ReadAsStringAsync());

        if (response.Headers.ETag is { } eTag)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/.well-known/dovetail/configuration-schema");
            request.Headers.IfNoneMatch.Add(eTag);
            using var conditional = await http.SendAsync(request);
            Console.WriteLine("Conditional=" + (int)conditional.StatusCode);
        }

        await app.StopAsync();
        """;

    [Test]
    public async Task PropertyOn_GeneratesTheJoint_AndTheApplicationServesTheSchema()
    {
        using var project = CreateProject(endpoint: "true", environments: null);

        var output = await project.Run("web");

        output.ShouldContain("Status=200");
        output.ShouldContain("ContentType=application/schema+json");
        output.ShouldContain("""Body={"type":"object","title":"sdk-test"}""");
        output.ShouldContain("Conditional=304");
        var generated = ReadGeneratedJoint(project)!;
        generated.ShouldContain("global::Joinery.Dovetail.Attributes.DovetailExport]");
        generated.ShouldContain("IHostCreatedJoint<global::Microsoft.AspNetCore.Builder.WebApplication>");
        generated.ShouldContain("MapDovetailConfigurationSchema(host, \"");
        generated.ShouldContain("obj");
        generated.ShouldContain(", null);");
    }

    [Test]
    public async Task PropertyOff_GeneratesNothing_AndTheRouteIsNotMapped()
    {
        using var project = CreateProject(endpoint: null, environments: null);

        var output = await project.Run("web");

        output.ShouldContain("Status=404");
        ReadGeneratedJoint(project).ShouldBeNull();
    }

    [Test]
    public async Task PropertyFalse_GeneratesNothing()
    {
        using var project = CreateProject(endpoint: "false", environments: null);

        var output = await project.Run("web");

        output.ShouldContain("Status=404");
        ReadGeneratedJoint(project).ShouldBeNull();
    }

    [Test]
    [Arguments("Production;Staging", "Status=200")]
    [Arguments("Development;Staging", "Status=404")]
    public async Task Environments_RestrictTheEndpointToTheListedEnvironments(string environments, string expected)
    {
        // The application runs in the default (Production) environment.
        using var project = CreateProject(endpoint: "true", environments: environments);

        var output = await project.Run("web");

        output.ShouldContain(expected);
        ReadGeneratedJoint(project)!.ShouldContain($", \"{environments}\");");
    }

    [Test]
    public async Task SourceFallback_CanBeLeftOutOfTheCompiledApplication()
    {
        using var project = CreateProject(endpoint: "true", environments: null, sourceFallback: "false");

        var output = await project.Run("web");

        output.ShouldContain("Status=200");
        var generated = ReadGeneratedJoint(project)!;
        generated.ShouldContain("MapDovetailConfigurationSchema(host, null, null);");
        generated.ShouldNotContain("obj");
    }

    private SdkTestProject CreateProject(string? endpoint, string? environments, string? sourceFallback = null)
    {
        var web = ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.Web");
        if (endpoint is not null) web.Property("DovetailConfigurationSchemaEndpoint", endpoint);
        if (environments is not null) web.Property("DovetailConfigurationSchemaEndpointEnvironments", environments);
        if (sourceFallback is not null) web.Property("DovetailConfigurationSchemaEndpointSourceFallback", sourceFallback);

        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("web/web.csproj", web)
           .AddFile("web/Program.cs", Program);
        return project;
    }

    private static string? ReadGeneratedJoint(SdkTestProject project)
    {
        var objDirectory = Path.Combine(project.Directory, "web", "obj");
        if (!Directory.Exists(objDirectory)) return null;

        var files = Directory.EnumerateFiles(objDirectory, GeneratedFileName, SearchOption.AllDirectories).ToArray();
        return files.Length == 0 ? null : File.ReadAllText(files.Single());
    }
}
