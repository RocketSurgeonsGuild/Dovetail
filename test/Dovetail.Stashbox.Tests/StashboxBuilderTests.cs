using Dovetail.Joints;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;
using static Dovetail.Stashbox.Tests.StashboxFixtures;

#pragma warning disable CA1040, CA1034, CA2000, IDE0058, RCS1021

namespace Dovetail.Stashbox.Tests;

public class StashboxBuilderTests : AutoFakeTest<TestRecord>
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
                                                 container.RegisterInstance(A.Fake<IAbc>());
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldBeNull();
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
                                                 container.RegisterInstance(A.Fake<IAbc>());
                                                 container.RegisterInstance(A.Fake<IAbc4>());
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldNotBeNull();
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
                                                 container.RegisterInstance(A.Fake<IAbc3>());
                                                 container.RegisterInstance(A.Fake<IAbc4>());
                                             }
                                         )
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<IAbc3>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldNotBeNull();
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
                                                 container.RegisterInstance(A.Fake<IAbc>());
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();

        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldBeNull();
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
                                                 container.RegisterInstance(A.Fake<IAbc>());
                                                 container.RegisterInstance(A.Fake<IAbc4>());
                                             }
                                         )
                                        .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
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
                                                 container.RegisterInstance(A.Fake<IAbc3>());
                                                 container.RegisterInstance(A.Fake<IAbc4>());
                                             }
                                         )
                               );

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc3>().ShouldNotBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        var items = host.Services.GetRequiredService<IDependencyResolver>();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldBeNull();
        items.ResolveOrDefault<IOtherAbc3>().ShouldNotBeNull();
        items.ResolveOrDefault<IOtherAbc3>().ShouldNotBeNull();
    }

    [Test]
    public async Task Should_Integrate_With_Stashbox()
    {
        using var host = await Host
                              .CreateApplicationBuilder([])
                              .ConfigureDovetail();

        var container = host.Services.GetRequiredService<IStashboxContainer>();
        container.ShouldNotBeNull();
    }

    public StashboxBuilderTests() : base(TestRecord.Create(LogEventLevel.Information)) =>
        AutoFake.Provide<IDictionary<object, object?>>(new Dictionary<object, object?>());
}
