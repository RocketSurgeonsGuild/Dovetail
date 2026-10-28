using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace Joinery.Dovetail.OpenTelemetry.Joints;

/// <summary>
///     Joint interface for <see cref="MeterProviderBuilder" />.
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface IMeterProviderBuilderJoint : IOpenTelemetryJoint
{
    /// <summary>
    ///     Register the <see cref="MeterProviderBuilder" /> part.
    /// </summary>
    void Register(IDovetailContext context, MeterProviderBuilder meterProvider);

    void IOpenTelemetryJoint.Register(IDovetailContext context, IOpenTelemetryBuilder builder) => builder.WithMetrics(b => Register(context, b));
}

/// <summary>
///     Delegate for the <see cref="MeterProvider" /> part.
/// </summary>
[global::System.CodeDom.Compiler.GeneratedCode("Dovetail", "generated"), JointName("OpenTelemetry")]
public delegate void MeterProviderJointDelegate(global::Joinery.Dovetail.IDovetailContext context, global::OpenTelemetry.Metrics.MeterProviderBuilder builder);
