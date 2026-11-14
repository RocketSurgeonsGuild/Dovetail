using DryIoc;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using static Joinery.Dovetail.DryIoc.Tests.DryIocFixtures;

namespace Joinery.Dovetail.DryIoc.Tests;

public class DryIocCommandLineTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        var builder = await Host
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

        var items = builder.GetLifetimeScope();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        var builder = await Host
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

        var items = builder.GetLifetimeScope();
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        var builder = await Host
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

        var items = builder.GetLifetimeScope();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        var builder = await Host
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
                                     .ConfigureDryIoc(
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
                                     .ConfigureDryIoc(
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
        await items.Resolve<IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
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

        await host.Services.GetRequiredService<IContainer>().Should().NotBeNull();
    }

    public DryIocCommandLineTests() : base(TestRecord.Create(LogEventLevel.Information)) { }
}
