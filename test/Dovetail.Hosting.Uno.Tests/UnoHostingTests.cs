using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stashbox;
using static Dovetail.Hosting.Uno.Tests.UnoFixtures;

namespace Dovetail.Hosting.Uno.Tests;

/// <summary>
///     Runs the Uno host against <see cref="TestApplicationBuilder" /> (Uno's own builder needs a running app), with the same
///     <c>DovetailHost</c> item the SDK's Uno detection adds. The tests share <see cref="Observed" />, so they don't run in parallel.
/// </summary>
[NotInParallel]
public class UnoHostingTests
{
    [Test]
    public async Task ConfigureDovetail_BuildsTheHost_AndRunsEveryJoint()
    {
        using var host = await new TestApplicationBuilder().ConfigureDovetail();

        host.Services.GetService<FromApplicationBuilderJoint>().ShouldNotBeNull();
        host.Services.GetService<FromHostBuilderJoint>().ShouldNotBeNull();
        Observed.CreatedHost.ShouldBeSameAs(host);
    }

    [Test]
    public async Task ContextBuilderConfiguration_IsApplied()
    {
        using var host = await new TestApplicationBuilder().ConfigureDovetail(rb => rb.ConfigureServices((_, services) => services.AddSingleton(new Marker())));

        host.Services.GetService<Marker>().ShouldNotBeNull();
    }

    [Test]
    public async Task EnvironmentName_DefersToUno()
    {
        var builder = new TestApplicationBuilder();
        builder.Configure(host => host.UseEnvironment("Staging"));

        using var host = await builder.ConfigureDovetail();

        Observed.EnvironmentName.ShouldBe("Staging");
        host.Services.GetRequiredService<IConfiguration>()["Uno:Name"].ShouldBe("staging");
    }

    [Test]
    public async Task EnvironmentName_SetOnTheContextBuilder_Wins()
    {
        var builder = new TestApplicationBuilder();
        builder.Configure(host => host.UseEnvironment("Staging"));

        using var host = await builder.ConfigureDovetail(rb => rb.Set("EnvironmentName", "Custom"));

        Observed.EnvironmentName.ShouldBe("Custom");
        host.Services.GetRequiredService<IConfiguration>()["Uno:Name"].ShouldBe("base");
    }

    [Test]
    public async Task Configuration_IsLayeredOnTopOfTheHostsOwn()
    {
        var builder = new TestApplicationBuilder();
        builder.Configure(
            host => host.ConfigureAppConfiguration(
                configuration => configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["Uno:Name"] = "uno", ["Uno:Only"] = "uno", })
            )
        );

        using var host = await builder.ConfigureDovetail();
        var configuration = host.Services.GetRequiredService<IConfiguration>();

        configuration["Uno:Name"].ShouldBe("base");
        configuration["Uno:Only"].ShouldBe("uno");
    }

    [Test]
    public async Task ServiceJoints_SeeTheHostsRegistrations()
    {
        var builder = new TestApplicationBuilder();
        builder.Configure(host => host.ConfigureServices(services => services.AddSingleton<RegisteredByTheHost>()));

        using var host = await builder.ConfigureDovetail();

        Observed.ServiceJointSawHostRegistrations.ShouldBeTrue();
    }

    [Test]
    public async Task ContainerFactory_IsHonored_WithTheFinalServiceCollection()
    {
        var builder = new TestApplicationBuilder();
        builder.Configure(host => host.ConfigureServices(services => services.AddSingleton<RegisteredByTheHost>()));

        using var host = await builder.ConfigureDovetail();

        host.Services.GetService<IStashboxContainer>().ShouldNotBeNull();
        host.Services.GetService<RegisteredByTheHost>().ShouldNotBeNull();
    }

    private sealed class Marker;
}
