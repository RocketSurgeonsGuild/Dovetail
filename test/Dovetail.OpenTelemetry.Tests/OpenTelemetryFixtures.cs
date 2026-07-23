using Dovetail.Attributes;
using Dovetail.OpenTelemetry.Joints;
using OpenTelemetry.Trace;

namespace Dovetail.OpenTelemetry.Tests;

public static class OpenTelemetryFixtures
{
    /// <summary>
    ///     Activity source name used by the auto-discovered <see cref="AutoDiscoveredTracerJoint" /> (task 4.7).
    /// </summary>
    public const string AutoDiscoveredSourceName = "Dovetail.OpenTelemetry.Tests.AutoDiscovered";

    /// <summary>
    ///     A tracer part that is discovered purely via <see cref="DovetailExportAttribute" /> assembly scanning,
    ///     with no manual <c>ConfigureTracerProvider</c> call (task 4.7).
    /// </summary>
    [DovetailExport]
    public sealed class AutoDiscoveredTracerJoint : ITracerProviderJoint
    {
        public void Register(IDovetailContext context, TracerProviderBuilder builder) => builder.AddSource(AutoDiscoveredSourceName);
    }
}
