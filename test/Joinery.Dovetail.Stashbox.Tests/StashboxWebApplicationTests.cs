using Joinery.Dovetail.Joints;
using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;

namespace Joinery.Dovetail.Stashbox.Tests;

public class StashboxWebApplicationTests : AutoFakeTest<TestRecord>
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc>());
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(A.Fake<StashboxFixtures.IAbc2>()))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<StashboxFixtures.IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc4>().ShouldBeNull();
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc>());
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc4>());
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(A.Fake<StashboxFixtures.IAbc2>()))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<StashboxFixtures.IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc4>().ShouldNotBeNull();
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc3>());
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc4>());
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<StashboxFixtures.IAbc3>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc4>().ShouldNotBeNull();
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc>());
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(A.Fake<StashboxFixtures.IAbc2>()))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();

        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<StashboxFixtures.IAbc>().ShouldNotBeNull();
        sp.GetService<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        sp.GetService<StashboxFixtures.IAbc3>().ShouldBeNull();
        sp.GetService<StashboxFixtures.IAbc4>().ShouldBeNull();
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc>());
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc4>());
                                                   }
                                               )
                                              .ConfigureServices((context, services) => services.AddSingleton(A.Fake<StashboxFixtures.IAbc2>()))
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<StashboxFixtures.IAbc>().ShouldNotBeNull();
        sp.GetService<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        sp.GetService<StashboxFixtures.IAbc3>().ShouldBeNull();
        sp.GetService<StashboxFixtures.IAbc4>().ShouldNotBeNull();
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
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc3>());
                                                       container.RegisterInstance(A.Fake<StashboxFixtures.IAbc4>());
                                                   }
                                               )
                                     );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<StashboxFixtures.IAbc3>().ShouldNotBeNull();
        sp.GetService<StashboxFixtures.IAbc4>().ShouldNotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<StashboxFixtures.IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc4>().ShouldBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        await using var host = await WebApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<StashboxFixtures.IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<StashboxFixtures.IAbc4>().ShouldBeNull();
        items.ResolveOrDefault<StashboxFixtures.IOtherAbc3>().ShouldNotBeNull();
        items.ResolveOrDefault<StashboxFixtures.IOtherAbc3>().ShouldNotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_Stashbox()
    {
        await using var host = await WebApplication
                                    .CreateBuilder([])
                                    .ConfigureDovetail();

        var container = host.Services.GetRequiredService<IStashboxContainer>();
        container.ShouldNotBeNull();
    }

    public StashboxWebApplicationTests() : base(TestRecord.Create(LogEventLevel.Information)) =>
        AutoFake.Provide<IDictionary<object, object?>>(new Dictionary<object, object?>());
}
