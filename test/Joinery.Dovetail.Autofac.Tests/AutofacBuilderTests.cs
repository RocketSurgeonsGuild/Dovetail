#pragma warning disable CA1040, CA1034, CA2000, IDE0058, RCS1021

using Autofac;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using static Joinery.Dovetail.Autofac.Tests.AutofacFixtures;

namespace Joinery.Dovetail.Autofac.Tests;

public class AutofacBuilderTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) => container.RegisterInstance(IAbc.Mock().Object))
                                                 .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOptional<IAbc>().Should().NotBeNull();
        await items.ResolveOptional<IAbc2>().Should().NotBeNull();
        await items.ResolveOptional<IAbc3>().Should().BeNull();
        await items.ResolveOptional<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) =>
                                                                   {
                                                                       container.RegisterInstance(IAbc.Mock().Object);
                                                                       container.RegisterInstance(IAbc4.Mock().Object);
                                                                   }
                                                  )
                                                 .ConfigureServices((context, services) => services.AddSingleton(IAbc2.Mock().Object))
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOptional<IAbc>().Should().NotBeNull();
        await items.ResolveOptional<IAbc2>().Should().NotBeNull();
        await items.ResolveOptional<IAbc3>().Should().BeNull();
        await items.ResolveOptional<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) =>
                                                                   {
                                                                       container.RegisterInstance(IAbc3.Mock().Object);
                                                                       container.RegisterInstance(IAbc4.Mock().Object);
                                                                   }
                                                  )
                            );

        var items = builder.GetLifetimeScope();
        await items.ResolveOptional<IAbc3>().Should().NotBeNull();
        await items.ResolveOptional<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) => container.RegisterInstance(IAbc.Mock().Object))
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
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) =>
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
                           .ConfigureDovetail(rb => rb
                                                 .ConfigureAutofac((context, container) =>
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
        await items.ResolveOptional<IAbc>().Should().NotBeNull();
        await items.ResolveOptional<IAbc2>().Should().NotBeNull();
        await items.ResolveOptional<IAbc3>().Should().BeNull();
        await items.ResolveOptional<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
        await items.ResolveOptional<IAbc>().Should().NotBeNull();
        await items.ResolveOptional<IAbc2>().Should().NotBeNull();
        await items.ResolveOptional<IAbc3>().Should().BeNull();
        await items.ResolveOptional<IAbc4>().Should().BeNull();
        await items.ResolveOptional<IOtherAbc3>().Should().NotBeNull();
        await items.ResolveOptional<IOtherAbc3>().Should().NotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_Autofac()
    {
        using var host = await Host
                              .CreateApplicationBuilder([])
                              .ConfigureDovetail();

        await host.Services.GetRequiredService<ILifetimeScope>().Should().NotBeNull();
    }

    public AutofacBuilderTests() : base(TestRecord.Create()) { }
}
