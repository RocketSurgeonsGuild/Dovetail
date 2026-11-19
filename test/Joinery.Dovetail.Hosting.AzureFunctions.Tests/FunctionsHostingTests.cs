using Joinery.Dovetail.Joints;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Joinery.Dovetail.Hosting.AzureFunctions.Tests.FunctionsFixtures;

namespace Joinery.Dovetail.Hosting.AzureFunctions.Tests;

/// <summary>
///     Builds an Azure Functions isolated worker host (without starting it, which needs the Functions host) with the same
///     <c>DovetailHost</c> items the SDK's detection adds (see the project file). The tests share <see cref="Observed" />, so they don't run
///     in parallel.
/// </summary>
[NotInParallel]
public class FunctionsHostingTests
{
    [Test]
    public async Task ConfigureDovetail_BuildsTheHost_AndRunsEveryJoint()
    {
        var builder = FunctionsApplication.CreateBuilder([]);

        using var host = await builder.ConfigureDovetail();

        await host.Services.GetService<IFromHostApplicationJoint>().Should().NotBeNull();
        await host.Services.GetService<IFromFunctionsApplicationBuilderJoint>().Should().NotBeNull();
        await host.Services.GetService<IFromFunctionsWorkerJoint>().Should().NotBeNull();
        await Observed.Builder.Should().BeSameReferenceAs(builder);
    }

    [Test]
    public async Task FunctionsWorkerJoints_RunAgainstTheApplicationBuilder()
    {
        var builder = FunctionsApplication.CreateBuilder([]);

        using var host = await builder.ConfigureDovetail();

        await Observed.WorkerBuilder.Should().BeSameReferenceAs(builder);
        // UseMiddleware registers the middleware with the worker's services.
        await host.Services.GetService<MarkerMiddleware>().Should().NotBeNull();
    }

    [Test]
    public async Task Context_CarriesTheWorkerBuilder()
    {
        var builder = FunctionsApplication.CreateBuilder([]);

        using var host = await builder.ConfigureDovetail();

        await Observed.ContextWorkerBuilder.Should().BeSameReferenceAs(builder);
    }

    [Test]
    public async Task ConfigureDovetail_OnATaskOfTheBuilder()
    {
        using var host = await Task.FromResult(FunctionsApplication.CreateBuilder([])).ConfigureDovetail();

        await host.Services.GetService<IFromFunctionsWorkerJoint>().Should().NotBeNull();
    }

    [Test]
    public async Task ContextBuilderConfiguration_IsChainedIntoTheHost()
    {
        var builder = FunctionsApplication.CreateBuilder([]);

        using var host = await builder.ConfigureDovetail(
            rb => rb.ConfigureConfiguration(
                static (_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Chained"] = "yes", })
            )
        );

        await host.Services.GetRequiredService<IConfiguration>()["Dovetail:Chained"].Should().BeEqualTo("yes");
    }
}
