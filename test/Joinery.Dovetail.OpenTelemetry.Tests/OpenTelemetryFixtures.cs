using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Joinery.Dovetail.OpenTelemetry.Tests;

public static class OpenTelemetryFixtures
{
    /// <summary>
    ///     Activity source name used by the auto-discovered <see cref="AutoDiscoveredTracerJoint" /> (task 4.7).
    /// </summary>
    public const string AutoDiscoveredSourceName = "Joinery.Dovetail.OpenTelemetry.Tests.AutoDiscovered";

    /// <summary>
    ///     Meter and logger category used by the exported joints that record into an <see cref="ExportedTelemetry" />.
    /// </summary>
    public const string ExportedSourceName = "Joinery.Dovetail.OpenTelemetry.Tests.Exported";

    /// <summary>
    ///     Resource attribute added by <see cref="ExportedResourceJoint" />.
    /// </summary>
    public const string ExportedResourceAttribute = "test.resource.exported";

    /// <summary>
    ///     A tracer part that is discovered purely via <see cref="DovetailExportAttribute" /> assembly scanning,
    ///     with no manual <c>ConfigureTracerProvider</c> call (task 4.7).
    /// </summary>
    [DovetailExport]
    public sealed class AutoDiscoveredTracerJoint : ITracerProviderJoint
    {
        public void Register(IDovetailContext context, TracerProviderBuilder builder) => builder.AddSource(AutoDiscoveredSourceName);
    }

    /// <summary>
    ///     What the exported joints below record, set on the Dovetail context by the test that wants to observe them. Exported joints are
    ///     applied to every host in this assembly, so they do nothing without one.
    /// </summary>
    public sealed class ExportedTelemetry
    {
        public List<MetricSnapshot> Metrics { get; } = [];

        public List<LogRecord> Logs { get; } = [];

        /// <summary>
        ///     The cancellation token <see cref="ExportedAsyncJoint" /> was given.
        /// </summary>
        public CancellationToken? AsyncCancellationToken { get; set; }
    }

    /// <summary>
    ///     A meter part, discovered through <see cref="DovetailExportAttribute" />.
    /// </summary>
    [DovetailExport]
    public sealed class ExportedMeterJoint : IMeterProviderBuilderJoint
    {
        public void Register(IDovetailContext context, MeterProviderBuilder builder)
        {
            if (context.Get<ExportedTelemetry>() is { } telemetry)
                builder.AddMeter(ExportedSourceName).AddInMemoryExporter(telemetry.Metrics);
        }
    }

    /// <summary>
    ///     A logger part, discovered through <see cref="DovetailExportAttribute" />.
    /// </summary>
    [DovetailExport]
    public sealed class ExportedLoggerJoint : ILoggerProviderBuilderJoint
    {
        public void Register(IDovetailContext context, LoggerProviderBuilder builder)
        {
            if (context.Get<ExportedTelemetry>() is { } telemetry)
                builder.AddInMemoryExporter(telemetry.Logs);
        }
    }

    /// <summary>
    ///     A resource part, discovered through <see cref="DovetailExportAttribute" />.
    /// </summary>
    [DovetailExport]
    public sealed class ExportedResourceJoint : IResourceBuilderJoint
    {
        public void Register(IDovetailContext context, ResourceBuilder builder)
        {
            if (context.Get<ExportedTelemetry>() is not null)
                builder.AddAttributes([new KeyValuePair<string, object>(ExportedResourceAttribute, "exported")]);
        }
    }

    /// <summary>
    ///     An asynchronous part of the base joint, discovered through <see cref="DovetailExportAttribute" />. It turns tracing on, so the
    ///     host has a tracer provider to carry the resource.
    /// </summary>
    [DovetailExport]
    public sealed class ExportedAsyncJoint : IOpenTelemetryAsyncJoint
    {
        public async ValueTask Register(IDovetailContext context, IOpenTelemetryBuilder builder, CancellationToken cancellationToken = default)
        {
            if (context.Get<ExportedTelemetry>() is not { } telemetry) return;

            await Task.Yield();
            telemetry.AsyncCancellationToken = cancellationToken;
            builder.WithTracing(static tracing => tracing.AddSource(ExportedSourceName));
        }
    }
}
