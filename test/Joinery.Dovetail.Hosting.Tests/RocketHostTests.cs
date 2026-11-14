using Joinery.Dovetail.Configuration.Json;
using Joinery.Dovetail.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.Tests;

public class RocketHostTests
{
    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithConfiguration()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
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

    [Test, Skip("Configuration needs to be redone")]
    public async Task Creates_RocketHost_WithModifiedConfiguration_Json()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
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
        using var host = await Host
                              .CreateApplicationBuilder()
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
