using Dovetail.Configuration.Json;
using Dovetail.Configuration.Yaml;
using Dovetail.Hosting.AspNetCore.Tests.Startups;
using Dovetail.Joints;
using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;



namespace Dovetail.Hosting.AspNetCore.Tests;

public class RocketWebApplicationTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
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
        content.ShouldBe("SimpleStartup -> Configure");
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
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(12);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(24);
#else
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(8);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(12);
#endif
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = A.Fake<HostCreatedAsyncJointDelegate<WebApplication>>();
        var delegate2 = A.Fake<HostCreatedJointDelegate<IHost>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate).ConfigureHostCreated(delegate2));

        A.CallTo(() => @delegate.Invoke(A<IDovetailContext>._, A<WebApplication>._, A<CancellationToken>._)).MustHaveHappened();
        A.CallTo(() => delegate2.Invoke(A<IDovetailContext>._, A<IHost>._)).MustHaveHappened();
        host.Services.ShouldNotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = A.Fake<HostApplicationJointDelegate<IHostApplicationBuilder>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IHostApplicationBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_ConfigureHosting_HostApplication()
    {
        var convention = A.Fake<HostApplicationJointDelegate<WebApplicationBuilder>>();
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                            .ConfigureHostApplication(convention)
                                     );

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<WebApplicationBuilder>._)).MustHaveHappened();
    }

    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithModifiedConfiguration_Json()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ExceptJoint(typeof(YamlJoint)));
        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();

#if NET10_0_OR_GREATER
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(12);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(0);
#else
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(8);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(0);
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
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(2);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(24);
#else
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(2);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(12);
#endif
    }
}
