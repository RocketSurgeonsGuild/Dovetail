using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;
using static Joinery.Dovetail.Stashbox.Tests.StashboxFixtures;

namespace Joinery.Dovetail.Stashbox.Tests;

public class StashboxCommandLineTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc.Mock().Object);
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc.Mock().Object);
                                              container.RegisterInstance(IAbc4.Mock().Object);
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc3.Mock().Object);
                                              container.RegisterInstance(IAbc4.Mock().Object);
                                          }
                                      )
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOrDefault<IAbc3>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc.Mock().Object);
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();

        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication_ServiceProvider()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc.Mock().Object);
                                              container.RegisterInstance(IAbc4.Mock().Object);
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_ServiceProvider()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(
                                rb => rb
                                     .ConfigureStashbox(
                                          (context, container) =>
                                          {
                                              container.RegisterInstance(IAbc3.Mock().Object);
                                              container.RegisterInstance(IAbc4.Mock().Object);
                                          }
                                      )
                            );

        var items = builder.GetLifetimeScope();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc3>().Should().NotBeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().BeNull();
        await items.ResolveOrDefault<IOtherAbc3>().Should().NotBeNull();
        await items.ResolveOrDefault<IOtherAbc3>().Should().NotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_Stashbox()
    {
        using var host = await Host
                              .CreateApplicationBuilder([])
                              .ConfigureDovetail();

        await host.Services.GetRequiredService<IStashboxContainer>().Should().NotBeNull();
    }

    public StashboxCommandLineTests() : base(TestRecord.Create(LogEventLevel.Information)) { }
}
