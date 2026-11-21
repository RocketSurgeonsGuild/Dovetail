using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace SdkSample.ServiceDefaults;

/// <summary>
///     Traces, metrics and logs for ASP.NET Core, <see cref="System.Net.Http.HttpClient" /> and the runtime, exported over OTLP when an
///     endpoint is configured. The AppHost sets <c>OTEL_EXPORTER_OTLP_ENDPOINT</c> to its dashboard (and <c>OTEL_SERVICE_NAME</c> to the
///     resource name); run on its own, an application exports nothing.
/// </summary>
[DovetailExport]
public sealed class TelemetryJoint : IOpenTelemetryJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IOpenTelemetryBuilder builder)
    {
        builder
           .WithLogging(static _ => { }, static options => options.IncludeFormattedMessage = true)
           .WithTracing(static tracing => tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation())
           .WithMetrics(static metrics => metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation());

        if (!string.IsNullOrWhiteSpace(context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            builder.UseOtlpExporter();
        }
    }
}
