using Dovetail.Configuration.Json;
using Dovetail.Configuration.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dovetail.Hosting.Tests;

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
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(12);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(24);
#else
        configuration.Providers.OfType<JsonConfigurationProvider>().Count().ShouldBe(8);
        configuration.Providers.OfType<YamlConfigurationProvider>().Count().ShouldBe(12);
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
        using var host = await Host
                              .CreateApplicationBuilder()
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
