using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;

namespace Joinery.Dovetail.Hosting.AspNetCore.Tests;

public class DovetailConfigurationSchemaEndpointTests
{
    private const string RoutePattern = "/.well-known/dovetail/configuration-schema";
    private const string SchemaFileName = "appsettings.schema.json";
    private const string Schema = """{"$schema":"http://json-schema.org/draft-07/schema#","type":"object","properties":{"Greetings":{"type":"object"}}}""";

    [Test]
    public async Task PublicContract_MatchesTheDocumentedRouteFileAndMediaType()
    {
        await DovetailConfigurationSchemaEndpointExtensions.RoutePattern.Should().BeEqualTo(RoutePattern);
        await DovetailConfigurationSchemaEndpointExtensions.SchemaFileName.Should().BeEqualTo(SchemaFileName);
        await DovetailConfigurationSchemaEndpointExtensions.SchemaMediaType.Should().BeEqualTo("application/schema+json");
    }

    [Test]
    public async Task ServesTheSchema_FromTheOutputDirectory_AsSchemaJson()
    {
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path);

        using var response = await app.GetTestClient().GetAsync(RoutePattern);

        await response.StatusCode.Should().BeEqualTo(HttpStatusCode.OK);
        await response.Content.Headers.ContentType!.MediaType.Should().BeEqualTo("application/schema+json");
        await ( await response.Content.ReadAsStringAsync() ).Should().BeEqualTo(Schema);
        await response.Headers.ETag.Should().NotBeNull();
        await response.Headers.CacheControl!.NoCache.Should().BeTrue();
    }

    [Test]
    public async Task FallsBackToTheFallbackDirectory_WhenTheOutputDirectoryHasNoSchema()
    {
        using var output = new TempDirectory();
        using var obj = new TempDirectory();
        obj.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path, obj.Path);

        using var response = await app.GetTestClient().GetAsync(RoutePattern);

        await response.StatusCode.Should().BeEqualTo(HttpStatusCode.OK);
        await ( await response.Content.ReadAsStringAsync() ).Should().BeEqualTo(Schema);
    }

    [Test]
    public async Task PrefersTheOutputDirectory_OverTheFallbackDirectory()
    {
        using var output = new TempDirectory();
        using var obj = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        obj.Write(SchemaFileName, """{"title":"obj"}""");
        await using var app = await StartAsync(output.Path, obj.Path);

        using var response = await app.GetTestClient().GetAsync(RoutePattern);

        await ( await response.Content.ReadAsStringAsync() ).Should().BeEqualTo(Schema);
    }

    [Test]
    public async Task Responds404_WhenNoSchemaFileExists()
    {
        using var output = new TempDirectory();
        using var obj = new TempDirectory();
        await using var app = await StartAsync(output.Path, obj.Path);

        using var response = await app.GetTestClient().GetAsync(RoutePattern);

        await response.StatusCode.Should().BeEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Responds404_AfterTheSchemaFileIsRemoved()
    {
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path);
        var client = app.GetTestClient();
        using var before = await client.GetAsync(RoutePattern);
        await before.StatusCode.Should().BeEqualTo(HttpStatusCode.OK);

        File.Delete(Path.Combine(output.Path, SchemaFileName));

        using var after = await client.GetAsync(RoutePattern);
        await after.StatusCode.Should().BeEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Responds304_WhenIfNoneMatchHoldsTheCurrentETag()
    {
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path);
        var client = app.GetTestClient();
        using var first = await client.GetAsync(RoutePattern);
        var eTag = first.Headers.ETag!;

        using var request = new HttpRequestMessage(HttpMethod.Get, RoutePattern);
        request.Headers.IfNoneMatch.Add(eTag);
        using var second = await client.SendAsync(request);

        await second.StatusCode.Should().BeEqualTo(HttpStatusCode.NotModified);
        await ( await second.Content.ReadAsStringAsync() ).Should().BeEmpty();
        await second.Headers.ETag.Should().BeEqualTo(eTag);
    }

    [Test]
    public async Task Serves200WithANewETag_WhenTheSchemaChangesOnDisk()
    {
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path);
        var client = app.GetTestClient();
        using var first = await client.GetAsync(RoutePattern);
        var staleETag = first.Headers.ETag!;

        const string updated = """{"type":"object","title":"updated"}""";
        output.Write(SchemaFileName, updated);
        using var request = new HttpRequestMessage(HttpMethod.Get, RoutePattern);
        request.Headers.IfNoneMatch.Add(staleETag);
        using var second = await client.SendAsync(request);

        await second.StatusCode.Should().BeEqualTo(HttpStatusCode.OK);
        await ( await second.Content.ReadAsStringAsync() ).Should().BeEqualTo(updated);
        await second.Headers.ETag.Should().NotBeEqualTo(staleETag);
    }

    [Test]
    [Arguments("Development", null, HttpStatusCode.OK)]
    [Arguments("Production", null, HttpStatusCode.OK)]
    [Arguments("Production", "", HttpStatusCode.OK)]
    [Arguments("Development", "Development", HttpStatusCode.OK)]
    [Arguments("Development", "development", HttpStatusCode.OK)]
    [Arguments("Staging", "Development;Staging", HttpStatusCode.OK)]
    [Arguments("Staging", " Development ; Staging ", HttpStatusCode.OK)]
    [Arguments("Production", "Development", HttpStatusCode.NotFound)]
    [Arguments("Production", "Development;Staging", HttpStatusCode.NotFound)]
    public async Task EnvironmentRestriction_OnlyMapsTheEndpointInTheListedEnvironments(string environment, string? environments, HttpStatusCode expected)
    {
        // The schema file exists, so a 404 here means the route was not mapped at all.
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path, environmentName: environment, environments: environments);

        using var response = await app.GetTestClient().GetAsync(RoutePattern);

        await response.StatusCode.Should().BeEqualTo(expected);
        if (expected == HttpStatusCode.OK) await ( await response.Content.ReadAsStringAsync() ).Should().BeEqualTo(Schema);
    }

    [Test]
    public async Task MapDovetailConfigurationSchema_ReturnsNull_WhenTheEnvironmentIsExcluded()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Production" });
        builder.WebHost.UseTestServer();
        await using var app = builder.Build();

        await app.MapDovetailConfigurationSchema(environments: "Development").Should().BeNull();
        await app.MapDovetailConfigurationSchema(environments: "Production").Should().NotBeNull();
    }

    [Test]
    public async Task OnlyGetIsMapped()
    {
        using var output = new TempDirectory();
        output.Write(SchemaFileName, Schema);
        await using var app = await StartAsync(output.Path);

        using var response = await app.GetTestClient().PostAsync(RoutePattern, new StringContent(""));

        await response.StatusCode.Should().BeEqualTo(HttpStatusCode.MethodNotAllowed);
    }

    private static async Task<WebApplication> StartAsync(
        string baseDirectory,
        string? fallbackDirectory = null,
        string environmentName = "Development",
        string? environments = null
    )
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = environmentName });
        builder.WebHost.UseTestServer();
        var app = builder.Build();
        DovetailConfigurationSchemaEndpointExtensions.Map(app, baseDirectory, fallbackDirectory, environments);
        await app.StartAsync();
        return app;
    }

    private sealed class TempDirectory : IDisposable
    {
        public TempDirectory() => Directory.CreateDirectory(Path);

        public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dovetail-schema-endpoint", Guid.NewGuid().ToString("N"));

        public void Write(string fileName, string content) => File.WriteAllText(System.IO.Path.Combine(Path, fileName), content);

        public void Dispose() => Directory.Delete(Path, recursive: true);
    }
}
