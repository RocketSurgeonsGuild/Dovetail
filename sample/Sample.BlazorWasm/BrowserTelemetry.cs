using System.Diagnostics;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.OpenTelemetry.Joints;
using OpenTelemetry;
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

public static class BrowserTelemetry
{
    public const string SourceName = "Sample.BlazorWasm";

    private static readonly TaskCompletionSource<string> _exported = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public static ActivitySource Source { get; } = new(SourceName);

    /// <summary>
    ///     Completes with the name of the first activity the exporter receives.
    /// </summary>
    public static Task<string> Exported => _exported.Task;

    internal static void MarkExported(string name) => _exported.TrySetResult(name);
}

internal sealed class BrowserTelemetryExporter : BaseExporter<Activity>
{
    public override ExportResult Export(in Batch<Activity> batch)
    {
        foreach (var activity in batch)
        {
            BrowserTelemetry.MarkExported(activity.DisplayName);
        }

        return ExportResult.Success;
    }
}
