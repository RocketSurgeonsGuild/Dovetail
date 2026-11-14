using DryIoc;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using static Joinery.Dovetail.DryIoc.Tests.DryIocFixtures;

#pragma warning disable CA1040, CA1034, CA2000, IDE0058, RCS1021

namespace Joinery.Dovetail.DryIoc.Tests;

public class DryIocBuilderTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.RegisterInstance(IAbc.Mock().Object);
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefault).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefault).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.RegisterInstance(IAbc.Mock().Object);
                                                 container.RegisterInstance(IAbc4.Mock().Object);
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefault).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefault).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.RegisterInstance(IAbc3.Mock().Object);
                                                 container.RegisterInstance(IAbc4.Mock().Object);
                                             }
                                         )
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefault).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.RegisterInstance(IAbc.Mock().Object);
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();

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
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.Use(IAbc.Mock().Object);
                                                 container.Use(IAbc4.Mock().Object);
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();
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
                                        .ConfigureDryIoc(
                                             (context, container) =>
                                             {
                                                 container.RegisterInstance(IAbc3.Mock().Object);
                                                 container.RegisterInstance(IAbc4.Mock().Object);
                                             }
                                         )
                               );

        var items = host.Services.GetRequiredService<IResolverContext>();
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

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IOtherAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IOtherAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_DryIoc()
    {
        using var host = await Host
                              .CreateApplicationBuilder([])
                              .ConfigureDovetail();

        var container = host.Services.GetRequiredService<IContainer>();
        await container.Should().NotBeNull();
    }

    public DryIocBuilderTests() : base(TestRecord.Create(LogEventLevel.Information)) { }

}
