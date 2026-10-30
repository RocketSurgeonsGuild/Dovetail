using System.Diagnostics;
using System.Diagnostics.Metrics;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.OpenTelemetry.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Sample.BlazorWasm;

/// <summary>
///     Traces the sample's activity source through Dovetail's OpenTelemetry joints, so the browser tests can see OpenTelemetry running
///     in the browser: Joinery.Dovetail.Hosting adds it to the services and starts it once the host is built, without
///     OpenTelemetry.Extensions.Hosting. Activities are exported through a batch processor, as a real exporter (OTLP) would be, which
///     the browser runs on a task instead of a thread.
/// </summary>
[DovetailExport]
public class BrowserTelemetryJoint : ITracerProviderJoint
{
    public void Register(IDovetailContext context, TracerProviderBuilder builder) =>
        builder
           .AddSource(BrowserTelemetry.SourceName)
           .AddProcessor(new BatchActivityExportProcessor(new BrowserTelemetryExporter(), scheduledDelayMilliseconds: 100));
}

/// <summary>
///     Measures the sample's meter, exported by a periodic metric reader, which the browser also runs on a task.
/// </summary>
[DovetailExport]
public class BrowserMetricsJoint : IMeterProviderBuilderJoint
{
    public void Register(IDovetailContext context, MeterProviderBuilder builder) =>
        builder
           .AddMeter(BrowserTelemetry.SourceName)
           .AddReader(new PeriodicExportingMetricReader(new BrowserMetricExporter(), exportIntervalMilliseconds: 100));
}

/// <summary>
///     Exports the sample's log records through a batch processor too, which is how the logging provider hands the application's
///     <c>ILogger</c> output to OpenTelemetry in the browser.
/// </summary>
[DovetailExport]
public class BrowserLoggerJoint : ILoggerProviderBuilderJoint
{
    public void Register(IDovetailContext context, LoggerProviderBuilder builder) =>
        builder.AddProcessor(new BatchLogRecordExportProcessor(new BrowserLogExporter(), scheduledDelayMilliseconds: 100));
}

/// <summary>
///     Describes the sample in every provider's resource, which all three exporters report.
/// </summary>
[DovetailExport]
public class BrowserResourceJoint : IResourceBuilderJoint
{
    public void Register(IDovetailContext context, ResourceBuilder builder) =>
        builder.AddAttributes([new KeyValuePair<string, object>(BrowserTelemetry.ResourceAttribute, "browser")]);
}

public static class BrowserTelemetry
{
    public const string SourceName = "Sample.BlazorWasm";
    public const string ResourceAttribute = "sample.host";

    /// <summary>
    ///     The category of the one log record the OpenTelemetry parity page waits for, among everything else the browser logs.
    /// </summary>
    public const string LogCategory = "Sample.BlazorWasm.Parity";

    private static readonly TaskCompletionSource<string> _exported = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static readonly TaskCompletionSource<string> _measured = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static readonly TaskCompletionSource<string> _logged = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public static ActivitySource Source { get; } = new(SourceName);

    public static Meter Meter { get; } = new(SourceName);

    public static Counter<long> Renders { get; } = Meter.CreateCounter<long>("sample.renders");

    /// <summary>
    ///     Completes with the name of the first activity the exporter receives and its provider's <see cref="ResourceAttribute" />.
    /// </summary>
    public static Task<string> Exported => _exported.Task;

    /// <summary>
    ///     Completes with the first sum of <see cref="Renders" /> the exporter receives and its provider's <see cref="ResourceAttribute" />.
    /// </summary>
    public static Task<string> Measured => _measured.Task;

    /// <summary>
    ///     Completes with the category, level, body and <c>Name</c> state of the first log record in <see cref="LogCategory" /> the
    ///     exporter receives and its provider's <see cref="ResourceAttribute" />.
    /// </summary>
    public static Task<string> Logged => _logged.Task;

    internal static void MarkLogged(string description, BaseProvider? provider) => _logged.TrySetResult($"{description} from {GetResourceAttribute(provider)}");

    internal static void MarkExported(string name, BaseProvider? provider) => _exported.TrySetResult($"{name} from {GetResourceAttribute(provider)}");

    internal static void MarkMeasured(long sum, BaseProvider? provider) => _measured.TrySetResult($"{sum} from {GetResourceAttribute(provider)}");

    private static object? GetResourceAttribute(BaseProvider? provider) =>
        provider?.GetResource().Attributes.FirstOrDefault(attribute => attribute.Key == ResourceAttribute).Value;
}

internal sealed class BrowserTelemetryExporter : BaseExporter<Activity>
{
    public override ExportResult Export(in Batch<Activity> batch)
    {
        foreach (var activity in batch)
        {
            BrowserTelemetry.MarkExported(activity.DisplayName, ParentProvider);
        }

        return ExportResult.Success;
    }
}

internal sealed class BrowserMetricExporter : BaseExporter<Metric>
{
    public override ExportResult Export(in Batch<Metric> batch)
    {
        foreach (var metric in batch)
        {
            foreach (ref readonly var point in metric.GetMetricPoints())
            {
                // The reader exports on a timer, so only report once the page has recorded something.
                if (point.GetSumLong() > 0)
                {
                    BrowserTelemetry.MarkMeasured(point.GetSumLong(), ParentProvider);
                }
            }
        }

        return ExportResult.Success;
    }
}

internal sealed class BrowserLogExporter : BaseExporter<LogRecord>
{
    public override ExportResult Export(in Batch<LogRecord> batch)
    {
        foreach (var record in batch)
        {
            if (record.CategoryName != BrowserTelemetry.LogCategory)
                continue;

            var name = record.Attributes?.FirstOrDefault(attribute => attribute.Key == "Name").Value;
            BrowserTelemetry.MarkLogged($"{record.CategoryName}|{record.LogLevel}|{record.Body}|{name}", ParentProvider);
        }

        return ExportResult.Success;
    }
}
