using System.Diagnostics;
using Joinery.Dovetail.Joints;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;
using static Joinery.Dovetail.Stashbox.Tests.StashboxFixtures;

namespace Joinery.Dovetail.Stashbox.Tests;

public class StashboxCommandLineTests : AutoFakeTest<TestRecord>
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
                                              container.RegisterInstance(A.Fake<IAbc>());
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                            );

        var items = builder.GetLifetimeScope();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldBeNull();
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
                                              container.RegisterInstance(A.Fake<IAbc>());
                                              container.RegisterInstance(A.Fake<IAbc4>());
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                            );

        var items = builder.GetLifetimeScope();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldNotBeNull();
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
                                              container.RegisterInstance(A.Fake<IAbc3>());
                                              container.RegisterInstance(A.Fake<IAbc4>());
                                          }
                                      )
                            );

        var items = builder.GetLifetimeScope();
        items.ResolveOrDefault<IAbc3>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldNotBeNull();
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
                                              container.RegisterInstance(A.Fake<IAbc>());
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                            );

        var items = builder.GetLifetimeScope();

        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldBeNull();
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
                                              container.RegisterInstance(A.Fake<IAbc>());
                                              container.RegisterInstance(A.Fake<IAbc4>());
                                          }
                                      )
                                     .ConfigureServices((context, services) => services.AddSingleton(A.Fake<IAbc2>()))
                            );

        var items = builder.GetLifetimeScope();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
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
                                              container.RegisterInstance(A.Fake<IAbc3>());
                                              container.RegisterInstance(A.Fake<IAbc4>());
                                          }
                                      )
                            );

        var items = builder.GetLifetimeScope();
        var sp = items.Resolve<IServiceProvider>();
        sp.GetService<IAbc3>().ShouldNotBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
        items.ResolveOrDefault<IAbc>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc2>().ShouldNotBeNull();
        items.ResolveOrDefault<IAbc3>().ShouldBeNull();
        items.ResolveOrDefault<IAbc4>().ShouldBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail_IncludingOtherBits()
    {
        var builder = await Host
                           .CreateApplicationBuilder()
                           .ConfigureDovetail();

        var items = builder.GetLifetimeScope();
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

        host.Services.GetRequiredService<IStashboxContainer>().ShouldNotBeNull();
    }

    public StashboxCommandLineTests() : base(TestRecord.Create(LogEventLevel.Information)) =>
        AutoFake.Provide<DiagnosticSource>(new DiagnosticListener("Test"));
}
