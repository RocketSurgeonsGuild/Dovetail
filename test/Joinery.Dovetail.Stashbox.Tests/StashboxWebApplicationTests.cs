using Joinery.Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;

namespace Joinery.Dovetail.Stashbox.Tests;

public class StashboxWebApplicationTests : LoggerTest<TestRecord>
{
    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(StashboxFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<StashboxFixtures.IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc.Mock().Object);
                                                       container.RegisterInstance(StashboxFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(StashboxFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<StashboxFixtures.IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc3.Mock().Object);
                                                       container.RegisterInstance(StashboxFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<StashboxFixtures.IAbc3>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(StashboxFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();

        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<StashboxFixtures.IAbc>().Should().NotBeNull();
        await sp.GetService<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await sp.GetService<StashboxFixtures.IAbc3>().Should().BeNull();
        await sp.GetService<StashboxFixtures.IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc.Mock().Object);
                                                       container.RegisterInstance(StashboxFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(StashboxFixtures.IAbc2.Mock().Object))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<StashboxFixtures.IAbc>().Should().NotBeNull();
        await sp.GetService<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await sp.GetService<StashboxFixtures.IAbc3>().Should().BeNull();
        await sp.GetService<StashboxFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_ServiceProvider()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(
                                         rb => rb
                                              .ConfigureStashbox(
                                                   (context, container) =>
                                                   {
                                                       container.RegisterInstance(StashboxFixtures.IAbc3.Mock().Object);
                                                       container.RegisterInstance(StashboxFixtures.IAbc4.Mock().Object);
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        await sp.GetService<StashboxFixtures.IAbc3>().Should().NotBeNull();
        await sp.GetService<StashboxFixtures.IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<StashboxFixtures.IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        await items.ResolveOrDefault<StashboxFixtures.IAbc>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc2>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc3>().Should().BeNull();
        await items.ResolveOrDefault<StashboxFixtures.IAbc4>().Should().BeNull();
        await items.ResolveOrDefault<StashboxFixtures.IOtherAbc3>().Should().NotBeNull();
        await items.ResolveOrDefault<StashboxFixtures.IOtherAbc3>().Should().NotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_Stashbox()
    {
        await using var host = await WebApplication
                                    .CreateBuilder([])
                                    .ConfigureDovetail();

        var container = host.Services.GetRequiredService<IStashboxContainer>();
        await container.Should().NotBeNull();
    }

    public StashboxWebApplicationTests() : base(TestRecord.Create(LogEventLevel.Information)) { }
}
