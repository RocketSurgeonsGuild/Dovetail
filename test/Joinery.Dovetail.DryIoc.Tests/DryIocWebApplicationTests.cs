using DryIoc;
using Joinery.Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;

namespace Joinery.Dovetail.DryIoc.Tests;

public class DryIocWebApplicationTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(DryIocFixtures.IAbc.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(DryIocFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<DryIocFixtures.IAbc>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc2>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc3>(IfUnresolved.ReturnDefault).Should().BeNull();
        await items.Resolve<DryIocFixtures.IAbc4>(IfUnresolved.ReturnDefault).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(DryIocFixtures.IAbc.Mock().Object);
                                                       container.RegisterInstance(DryIocFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(DryIocFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<DryIocFixtures.IAbc>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc2>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc3>(IfUnresolved.ReturnDefault).Should().BeNull();
        await items.Resolve<DryIocFixtures.IAbc4>(IfUnresolved.ReturnDefault).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(DryIocFixtures.IAbc3.Mock().Object);
                                                       container.RegisterInstance(DryIocFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<DryIocFixtures.IAbc3>(IfUnresolved.ReturnDefault).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc4>(IfUnresolved.ReturnDefault).Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(DryIocFixtures.IAbc.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(DryIocFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();

        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<DryIocFixtures.IAbc>().Should().NotBeNull();
        await sp.GetService<DryIocFixtures.IAbc2>().Should().NotBeNull();
        await sp.GetService<DryIocFixtures.IAbc3>().Should().BeNull();
        await sp.GetService<DryIocFixtures.IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.Use(DryIocFixtures.IAbc.Mock().Object);
                                                       container.Use(DryIocFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(DryIocFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<DryIocFixtures.IAbc>().Should().NotBeNull();
        await sp.GetService<DryIocFixtures.IAbc2>().Should().NotBeNull();
        await sp.GetService<DryIocFixtures.IAbc3>().Should().BeNull();
        await sp.GetService<DryIocFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureDryIoc(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(DryIocFixtures.IAbc3.Mock().Object);
                                                       container.RegisterInstance(DryIocFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IResolverContext>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<DryIocFixtures.IAbc3>().Should().NotBeNull();
        await sp.GetService<DryIocFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<DryIocFixtures.IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<DryIocFixtures.IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IResolverContext>();
        await items.Resolve<DryIocFixtures.IAbc>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc2>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<DryIocFixtures.IAbc4>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().BeNull();
        await items.Resolve<DryIocFixtures.IOtherAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
        await items.Resolve<DryIocFixtures.IOtherAbc3>(IfUnresolved.ReturnDefaultIfNotRegistered).Should().NotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_DryIoc()
    {
        await using var host = await WebApplication
                                    .CreateBuilder([])
                                    .ConfigureDovetail();

        var container = host.Services.GetRequiredService<IContainer>();
        await container.Should().NotBeNull();
    }

    public DryIocWebApplicationTests() : base(TestRecord.Create(LogEventLevel.Information)) { }
}
