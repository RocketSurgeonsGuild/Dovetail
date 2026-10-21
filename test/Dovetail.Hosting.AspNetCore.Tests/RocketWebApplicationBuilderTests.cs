using Dovetail.Hosting.AspNetCore.Tests.Startups;
using Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Extensions.Testing;


namespace Dovetail.Hosting.AspNetCore.Tests;

public class RocketWebApplicationBuilderTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        await using var host = await builder.ConfigureDovetail();

        new TestStartup(builder.Environment, builder.Configuration).Configure(host);
        await host.StartAsync();
        var server = host.GetTestServer();
        server.CreateClient();
        await host.StopAsync();
    }

    [Test]
    public async Task Should_Chain_Dovetail_Configuration_Into_The_Host()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        await using var host = await builder.ConfigureDovetail(
            rb => rb.ConfigureConfiguration(
                static (_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Chained"] = "yes", })
            )
        );

        host.Configuration["Dovetail:Chained"].ShouldBe("yes");
    }
}
