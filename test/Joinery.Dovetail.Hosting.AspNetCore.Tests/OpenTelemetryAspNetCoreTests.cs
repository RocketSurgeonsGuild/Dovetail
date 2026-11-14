using System.Diagnostics;
using System.Diagnostics.Metrics;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Joinery.Dovetail.Hosting.AspNetCore.Tests;

/// <summary>
///     Dovetail's OpenTelemetry joints on ASP.NET Core. Joinery.Dovetail.Hosting turns OpenTelemetry on for the application with
///     OpenTelemetry.Extensions.Hosting, whose hosted service starts the providers with the web host, so these tests start the host the
///     way an application does. They are the ASP.NET Core half of the LaunchPad.Telemetry parity check (docs: concepts/opentelemetry-launchpad-parity).
/// </summary>
public class OpenTelemetryAspNetCoreTests
{
    [Test]
    public async Task Starting_The_Web_Application_Starts_Every_Signal_With_The_Resource()
    {
        const string name = "Joinery.Dovetail.Hosting.AspNetCore.Tests.Started";
        List<Activity> exportedActivities = [];
        List<MetricSnapshot> exportedMetrics = [];
        List<LogRecord> exportedLogs = [];
        var attribute = new KeyValuePair<string, object>("test.resource.aspnetcore", "started");

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        await using var host = await builder.ConfigureDovetail(
            rb => rb
                 .ConfigureResourceBuilder((_, resource) => resource.AddAttributes([attribute]))
                 .ConfigureTracerProvider((_, tracing) => tracing.AddSource(name).AddInMemoryExporter(exportedActivities))
                 .ConfigureMeterProvider((_, metrics) => metrics.AddMeter(name).AddInMemoryExporter(exportedMetrics))
                 .ConfigureLoggerProvider((_, logging) => logging.AddInMemoryExporter(exportedLogs))
        );

        await host.StartAsync();
        try
        {
            using (var source = new ActivitySource(name))
            using (source.StartActivity("started-span")) { }

            using var meter = new Meter(name);
            meter.CreateCounter<long>("started.counter").Add(3);
            host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(name).LogInformation("started-log");

            var tracerProvider = host.Services.GetRequiredService<TracerProvider>();
            var meterProvider = host.Services.GetRequiredService<MeterProvider>();
            var loggerProvider = host.Services.GetRequiredService<LoggerProvider>();
            tracerProvider.ForceFlush();
            meterProvider.ForceFlush();
            loggerProvider.ForceFlush();

            await exportedActivities.Should().Contain(activity => activity.DisplayName == "started-span");
            await exportedMetrics.Should().Contain(metric => metric.Name == "started.counter" && metric.MetricPoints.Sum(point => point.GetSumLong()) == 3);
            await exportedLogs.Should().Contain(record => record.Body == "started-log");
            await tracerProvider.GetResource().Attributes.Should().Contain(attribute);
            await meterProvider.GetResource().Attributes.Should().Contain(attribute);
            await loggerProvider.GetResource().Attributes.Should().Contain(attribute);
        }
        finally
        {
            await host.StopAsync();
        }
    }

    /// <summary>
    ///     The SDK's default resource, which LaunchPad's default telemetry convention only repeated, is the resource of an ASP.NET Core
    ///     application's providers.
    /// </summary>
    [Test]
    public async Task Providers_Have_The_Default_Resource()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        await using var host = await builder.ConfigureDovetail(rb => rb.ConfigureOpenTelemetry((_, otel) => otel.WithTracing().WithMetrics().WithLogging()));

        BaseProvider[] providers =
        [
            host.Services.GetRequiredService<TracerProvider>(),
            host.Services.GetRequiredService<MeterProvider>(),
            host.Services.GetRequiredService<LoggerProvider>(),
        ];
        foreach (var provider in providers)
        {
            var attributes = AttributesOf(provider);

            await attributes.Should().ContainKeyWithValue("telemetry.sdk.name", "opentelemetry");
            await attributes.Should().ContainKeyWithValue("telemetry.sdk.language", "dotnet");
            await attributes.Should().ContainKey("telemetry.sdk.version");
            await ( await Assert.That(attributes["service.name"]).IsTypeOf<string>() ).Should().StartWith("unknown_service");
        }
    }

#if NET11_0_OR_GREATER
    /// <summary>
    ///     On .NET 11 enabling OpenTelemetry through Dovetail subscribes to ASP.NET Core's <c>Microsoft.AspNetCore</c> activity source, so a
    ///     request is traced without any tracer configuration beyond the exporter.
    /// </summary>
    [Test]
    public async Task A_Request_Is_Traced_Without_Any_Tracer_Configuration()
    {
        // ASP.NET Core ends the request's activity after the test client has the response, so the test waits for it to end rather than
        // looking for it as soon as the response is read.
        var requestTraced = new TaskCompletionSource<Activity>(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        await using var host = await builder.ConfigureDovetail(
            rb => rb.ConfigureTracerProvider((_, tracing) => tracing.AddProcessor(new EndedActivityProcessor("Microsoft.AspNetCore", requestTraced)))
        );
        host.MapGet("/telemetry", () => "traced");

        await host.StartAsync();
        try
        {
            using var response = await host.GetTestClient().GetAsync("/telemetry");
            await ( await response.Content.ReadAsStringAsync() ).Should().BeEqualTo("traced");

            var activity = await requestTraced.Task.WaitAsync(TimeSpan.FromSeconds(15));

            await activity.Source.Name.Should().BeEqualTo("Microsoft.AspNetCore");
        }
        finally
        {
            await host.StopAsync();
        }
    }

    /// <summary>Completes <paramref name="ended" /> with the first activity from <paramref name="sourceName" /> to end.</summary>
    private sealed class EndedActivityProcessor(string sourceName, TaskCompletionSource<Activity> ended) : BaseProcessor<Activity>
    {
        public override void OnEnd(Activity data)
        {
            if (data.Source.Name == sourceName)
            {
                ended.TrySetResult(data);
            }
        }
    }
#endif

    private static Dictionary<string, object> AttributesOf(BaseProvider provider)
    {
        var attributes = new Dictionary<string, object>();
        foreach (var attribute in provider.GetResource().Attributes)
        {
            attributes.Add(attribute.Key, attribute.Value);
        }

        return attributes;
    }
}
