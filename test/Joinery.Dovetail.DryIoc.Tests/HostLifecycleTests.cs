using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.DryIoc.Tests;

/// <summary>
///     The hosted service behind <c>OnHostStarting</c>, <c>OnHostStarted</c>, <c>OnHostStopping</c> and <c>OnHostStopped</c>
///     (emitted by Joinery.Dovetail.Hosting) is registered with DryIoc like any other hosted service.
/// </summary>
public class HostLifecycleTests
{
    [Test]
    public async Task Handlers_RunWithDryIoc()
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
                              .ConfigureDovetail(dovetail => dovetail.ConfigureDryIoc((_, _) => { }));
        await host.StartAsync(TestContext.Current!.Execution.CancellationToken);
        await host.StopAsync(TestContext.Current!.Execution.CancellationToken);

        moments.ShouldBe(["starting", "started", "stopped"]);
    }
}
