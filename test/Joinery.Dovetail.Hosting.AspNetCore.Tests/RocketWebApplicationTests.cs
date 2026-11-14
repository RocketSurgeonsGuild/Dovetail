using Joinery.Dovetail.Configuration.Json;
using Joinery.Dovetail.Configuration.Yaml;
using Joinery.Dovetail.Hosting.AspNetCore.Tests.Startups;
using Joinery.Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;



namespace Joinery.Dovetail.Hosting.AspNetCore.Tests;

public class RocketWebApplicationTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_Start_Application()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        await using var host = await builder.ConfigureDovetail();

        new SimpleStartup().Configure(host);
        await host.StartAsync();
        var server = host.GetTestServer();
        var response = await server
                            .CreateRequest("/")
                            .GetAsync();

        var content = await response.Content.ReadAsStringAsync();
        await content.Should().BeEqualTo("SimpleStartup -> Configure");
        await host.StopAsync();
    }

    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithConfiguration()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();
        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();

#if NET10_0_OR_GREATER
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(12);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(24);
#else
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(8);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(12);
#endif
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = Mock.OfDelegate<HostCreatedAsyncJointDelegate<WebApplication>>();
        var delegate2 = Mock.OfDelegate<HostCreatedJointDelegate<IHost>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate.Object).ConfigureHostCreated(delegate2.Object));

        @delegate.Invoke(Any(), Any(), Any()).WasCalled();
        delegate2.Invoke(Any(), Any()).WasCalled();
        await host.Services.Should().NotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = Mock.OfDelegate<HostApplicationJointDelegate<IHostApplicationBuilder>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_ConfigureHosting_HostApplication()
    {
        var convention = Mock.OfDelegate<HostApplicationJointDelegate<WebApplicationBuilder>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                            .ConfigureHostApplication(convention.Object)
                                     );

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithModifiedConfiguration_Json()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ExceptJoint(typeof(YamlJoint)));
        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();

#if NET10_0_OR_GREATER
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(12);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(0);
#else
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(8);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(0);
#endif
    }

    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithModifiedConfiguration_Yaml()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ExceptJoint(typeof(JsonJoint)));

        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();

#if NET10_0_OR_GREATER
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(2);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(24);
#else
        await configuration.Providers.OfType<JsonConfigurationProvider>().Count().Should().BeEqualTo(2);
        await configuration.Providers.OfType<YamlConfigurationProvider>().Count().Should().BeEqualTo(12);
#endif
    }
}
