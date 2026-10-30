using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.Tests;

/// <summary>
///     <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c> are emitted into every project that
///     references the generic host (<c>DovetailHostLifecycleHelpers.cs.template</c>), so this project runs its own copy.
/// </summary>
public class HostLifecycleTests
{
    private static readonly TimeSpan WaitLimit = TimeSpan.FromSeconds(30);
    private static readonly string[] AllMoments = ["starting", "started", "stopping", "stopped"];

    [Test]
    public async Task Synchronous_Handlers_RunAtEachMoment()
    {
        List<string> moments = [];

        await RunAsync(
            Host
               .CreateApplicationBuilder()
               .OnHostStarting(_ => moments.Add("starting"))
               .OnHostStarted(_ => moments.Add("started"))
               .OnHostStopping(_ => moments.Add("stopping"))
               .OnHostStopped(_ => moments.Add("stopped"))
        );

        moments.ShouldBe(AllMoments);
    }

    [Test]
    public async Task Asynchronous_Handlers_RunAtEachMoment()
    {
        List<string> moments = [];

        await RunAsync(
            Host
               .CreateApplicationBuilder()
               .OnHostStarting(
                    async _ =>
                    {
                        await Task.Yield();
                        moments.Add("starting");
                    }
                )
               .OnHostStarted(
                    async _ =>
                    {
                        await Task.Yield();
                        moments.Add("started");
                    }
                )
               .OnHostStopping(
                    async _ =>
                    {
                        await Task.Yield();
                        moments.Add("stopping");
                    }
                )
               .OnHostStopped(
                    async _ =>
                    {
                        await Task.Yield();
                        moments.Add("stopped");
                    }
                )
        );

        moments.ShouldBe(AllMoments);
    }

    [Test]
    public async Task Cancellable_Handlers_RunAtEachMoment()
    {
        List<string> moments = [];

        await RunAsync(
            Host
               .CreateApplicationBuilder()
               .OnHostStarting(
                    async (_, _) =>
                    {
                        await Task.Yield();
                        moments.Add("starting");
                    }
                )
               .OnHostStarted(
                    async (_, _) =>
                    {
                        await Task.Yield();
                        moments.Add("started");
                    }
                )
               .OnHostStopping(
                    async (_, _) =>
                    {
                        await Task.Yield();
                        moments.Add("stopping");
                    }
                )
               .OnHostStopped(
                    async (_, _) =>
                    {
                        await Task.Yield();
                        moments.Add("stopped");
                    }
                )
        );

        moments.ShouldBe(AllMoments);
    }

    [Test]
    public async Task Handlers_ReceiveTheHostsServiceProvider()
    {
        IServiceProvider? received = null;
        using var host = await Host.CreateApplicationBuilder().OnHostStarted(provider => received = provider).ConfigureDovetail();

        await host.StartAsync(CancellationToken);
        await host.StopAsync(CancellationToken);

        // The host's root scope, so it resolves what the host registered.
        received.ShouldNotBeNull().GetRequiredService<IHostEnvironment>().ShouldBeSameAs(host.Services.GetRequiredService<IHostEnvironment>());
    }

    [Test]
    public async Task HandlersOfOneMoment_RunInParallel()
    {
        // The first handler can only finish once the second one has started: awaited one after the other, it would never get there.
        var secondStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        List<string> finished = [];

        await RunAsync(
            Host
               .CreateApplicationBuilder()
               .OnHostStarted(
                    async (_, token) =>
                    {
                        await secondStarted.Task.WaitAsync(WaitLimit, token);
                        lock (finished) finished.Add("first");
                    }
                )
               .OnHostStarted(
                    async (_, _) =>
                    {
                        secondStarted.SetResult();
                        await Task.Yield();
                        lock (finished) finished.Add("second");
                    }
                )
        );

        finished.Order().ShouldBe(["first", "second"]);
    }

    [Test]
    public async Task HandlerThatThrows_DoesNotKeepTheOthersFromRunning()
    {
        var ran = false;
        using var host = await Host
                              .CreateApplicationBuilder()
                              .OnHostStarting(_ => throw new InvalidOperationException("boom"))
                              .OnHostStarting(_ => ran = true)
                              .ConfigureDovetail();

        var exception = await Should.ThrowAsync<InvalidOperationException>(host.StartAsync(CancellationToken));

        exception.Message.ShouldBe("boom");
        ran.ShouldBeTrue();
    }

    [Test]
    public async Task CancellableHandlers_ReceiveTheTokenTheHostWasStartedWith()
    {
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);
        using var host = await Host
                              .CreateApplicationBuilder()
                              .OnHostStarting(
                                   async (_, token) =>
                                   {
                                       await cancellation.CancelAsync();
                                       await Task.Delay(Timeout.InfiniteTimeSpan, token);
                                   }
                               )
                              .ConfigureDovetail();

        await Should.ThrowAsync<OperationCanceledException>(host.StartAsync(cancellation.Token));
    }

    /// <summary>
    ///     Hosts built the older way don't run Dovetail at all, but still have a service collection.
    /// </summary>
    [Test]
    public async Task ServiceCollection_Registration_RunsOnAHostBuiltWithoutDovetail()
    {
        List<string> moments = [];
        using var host = new HostBuilder()
                        .ConfigureServices(
                             services => services
                                        .OnHostStarting(_ => moments.Add("starting"))
                                        .OnHostStarted(
                                             _ =>
                                             {
                                                 moments.Add("started");
                                                 return Task.CompletedTask;
                                             }
                                         )
                                        .OnHostStopping((_, _) =>
                                         {
                                             moments.Add("stopping");
                                             return Task.CompletedTask;
                                         })
                                        .OnHostStopped(_ => moments.Add("stopped"))
                         )
                        .Build();

        await host.StartAsync(CancellationToken);
        await host.StopAsync(CancellationToken);

        moments.ShouldBe(AllMoments);
    }

    /// <summary>
    ///     A joint (a library, say) registers its handlers while the application's services are configured.
    /// </summary>
    [Test]
    public async Task Joints_RegisterHandlers_WhileTheServicesAreConfigured()
    {
        List<string> moments = [];
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   dovetail => dovetail.ConfigureServices(
                                       (_, services) => services
                                                       .OnHostStarted(_ => moments.Add("started"))
                                                       .OnHostStopped(_ => moments.Add("stopped"))
                                   )
                               );

        await host.StartAsync(CancellationToken);
        await host.StopAsync(CancellationToken);

        moments.ShouldBe(["started", "stopped"]);
    }

    [Test]
    public async Task ApplicationBuilderAndServiceCollection_ShareOneHostedService()
    {
        List<string> moments = [];
        var builder = Host.CreateApplicationBuilder();
        builder.OnHostStarted(_ => moments.Add("builder"));
        builder.Services.OnHostStarted(_ => moments.Add("services"));
        builder.OnHostStopped(_ => moments.Add("stopped"));

        builder.Services.Count(IsHostLifecycleService).ShouldBe(1);

        await RunAsync(builder);

        moments.ShouldBe(["builder", "services", "stopped"]);
    }

    [Test]
    public void WithoutHandlers_AddsNothing()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.Count(IsHostLifecycleService).ShouldBe(0);
        builder.Services.Count(static descriptor => descriptor.ServiceType == typeof(HostLifecycleRegistration)).ShouldBe(0);
    }

    /// <summary>
    ///     Another assembly's copy of the extension methods adds <see cref="HostLifecycleRegistration" /> too, and the one hosted service
    ///     whichever copy registered first runs them all.
    /// </summary>
    [Test]
    public async Task RegistrationsAddedByAnotherCopy_RunWithTheSameService()
    {
        List<string> moments = [];
        var builder = Host.CreateApplicationBuilder().OnHostStarted(_ => moments.Add("ours"));
        builder.Services.AddSingleton(
            new HostLifecycleRegistration(
                HostLifecycleMoment.Started,
                (_, _) =>
                {
                    moments.Add("theirs");
                    return Task.CompletedTask;
                }
            )
        );

        await RunAsync(builder);

        moments.ShouldBe(["ours", "theirs"]);
    }

    private static bool IsHostLifecycleService(ServiceDescriptor descriptor) =>
        descriptor.ServiceType == typeof(IHostedService) && descriptor.ImplementationType == typeof(DovetailHostLifecycleService);

    private static CancellationToken CancellationToken => TestContext.Current!.Execution.CancellationToken;

    private static async Task RunAsync(HostApplicationBuilder builder)
    {
        using var host = await builder.ConfigureDovetail();
        await host.StartAsync(CancellationToken);
        await host.StopAsync(CancellationToken);
    }
}
