using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Stashbox.Tests;

/// <summary>
///     The hosted service behind <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c>
///     (emitted by Joinery.Dovetail.Hosting) is registered with Stashbox like any other hosted service, which refuses types marked
///     <c>CompilerGenerated</c>.
/// </summary>
public class HostLifecycleTests
{
    [Test]
    public async Task Handlers_RunWithStashbox()
    {
        List<string> moments = [];

        using var host = await Host
                              .CreateApplicationBuilder()
                              .OnHostStarting(_ => moments.Add("starting"))
                              .OnHostStarted(
                                   async (_, _) =>
                                   {
                                       await Task.Yield();
                                       moments.Add("started");
                                   }
                               )
                              .OnHostStopped(_ => moments.Add("stopped"))
                              .ConfigureDovetail(dovetail => dovetail.ConfigureStashbox((_, _) => { }));
        await host.StartAsync(TestContext.Current!.Execution.CancellationToken);
        await host.StopAsync(TestContext.Current!.Execution.CancellationToken);

        moments.ShouldBe(["starting", "started", "stopped"]);
    }
}
