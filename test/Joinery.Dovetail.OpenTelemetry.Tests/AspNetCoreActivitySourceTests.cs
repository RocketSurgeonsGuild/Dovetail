#if NET11_0_OR_GREATER
using System.Diagnostics;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;

namespace Joinery.Dovetail.OpenTelemetry.Tests;

/// <summary>
///     On .NET 11, ASP.NET Core publishes its request activities on the <c>Microsoft.AspNetCore</c> activity source, and
///     enabling OpenTelemetry through Dovetail subscribes to it without any tracer configuration.
/// </summary>
public class AspNetCoreActivitySourceTests
{
    [Test]
    public async Task Should_Trace_The_AspNetCore_Activity_Source_By_Default()
    {
        List<Activity> exportedActivities = [];

        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureTracerProvider((_, builder) => builder.AddInMemoryExporter(exportedActivities)));

        var tracerProvider = host.Services.GetRequiredService<TracerProvider>();

        using var source = new ActivitySource(OpenTelemetryConnector.AspNetCoreActivitySourceName);
        using (source.StartActivity("Microsoft.AspNetCore.Hosting.HttpRequestIn")) { }

        tracerProvider.ForceFlush();

        exportedActivities.ShouldContain(a => a.Source.Name == "Microsoft.AspNetCore");
    }
}
#endif
