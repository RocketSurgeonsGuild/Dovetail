using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Hosting.AspNetCore.Tests;

/// <summary>
///     <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c> are emitted into every project that
///     references the generic host, ASP.NET Core's included (<c>DovetailHostLifecycleHelpers.cs.template</c>), so this project runs its
///     own copy on a <see cref="WebApplicationBuilder" />.
/// </summary>
public class HostLifecycleTests
{
    private static readonly string[] AllMoments = ["starting", "started", "stopping", "stopped"];

    [Test]
    public async Task Synchronous_Handlers_RunAtEachMoment()
    {
        List<string> moments = [];

        await RunAsync(
            CreateBuilder()
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
            CreateBuilder()
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
            CreateBuilder()
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
    public async Task ServiceCollection_Registration_RunsAtEachMoment()
    {
        List<string> moments = [];
        var builder = CreateBuilder();
        builder
           .Services
           .OnHostStarting(_ => moments.Add("starting"))
           .OnHostStarted(_ => moments.Add("started"))
           .OnHostStopping(_ => moments.Add("stopping"))
           .OnHostStopped(_ => moments.Add("stopped"));

        await RunAsync(builder);

        moments.ShouldBe(AllMoments);
    }

    [Test]
    public async Task Handlers_ReceiveTheApplicationsServiceProvider()
    {
        IWebHostEnvironment? environment = null;

        await RunAsync(CreateBuilder().OnHostStarted(provider => environment = provider.GetRequiredService<IWebHostEnvironment>()));

        environment.ShouldNotBeNull();
    }

    [Test]
    public async Task HandlersOfOneMoment_RunInParallel()
    {
        // The first handler can only finish once the second one has started: awaited one after the other, it would never get there.
        var secondStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        List<string> finished = [];

        await RunAsync(
            CreateBuilder()
               .OnHostStarted(
                    async (_, token) =>
                    {
                        await secondStarted.Task.WaitAsync(TimeSpan.FromSeconds(30), token);
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

    private static WebApplicationBuilder CreateBuilder()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        return builder;
    }

    private static async Task RunAsync(WebApplicationBuilder builder)
    {
        await using var host = await builder.ConfigureDovetail();
        await host.StartAsync(CancellationToken);
        await host.StopAsync(CancellationToken);
    }

    private static CancellationToken CancellationToken => TestContext.Current!.Execution.CancellationToken;
}
