using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;
using static Joinery.Dovetail.Stashbox.Tests.StashboxFixtures;

#pragma warning disable CA1040, CA1034, CA2000, IDE0058, RCS1021

namespace Joinery.Dovetail.Stashbox.Tests;

public class StashboxBuilderTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<IAbc3>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();

        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication_ServiceProvider()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_ServiceProvider()
    {
        using var host = await Host
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

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<IAbc3>().Should().NotBeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
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

        var container = host.Services.GetRequiredService<IStashboxContainer>();
        await container.Should().NotBeNull();
    }

    public StashboxBuilderTests() : base(TestRecord.Create(LogEventLevel.Information)) { }
}
