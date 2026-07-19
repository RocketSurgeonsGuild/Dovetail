using Dovetail.Infrastructure;
using Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Dovetail.OpenTelemetry.Joints;

/// <summary>
///     Joint interface for <see cref="TracerProviderBuilder" />.
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface ITracerProviderJoint : IOpenTelemetryJoint
{
    /// <summary>
    ///     Register the <see cref="TracerProviderBuilder" /> part.
    /// </summary>
    void Register(IDovetailContext context, TracerProviderBuilder tracerProvider);

    void IOpenTelemetryJoint.Register(IDovetailContext context, IOpenTelemetryBuilder builder) => builder.WithTracing(b => Register(context, b));
}

/// <summary>
///     Delegate for the <see cref="TracerProvider" /> part.
/// </summary>
[global::System.CodeDom.Compiler.GeneratedCode("Dovetail", "generated"), JointName("OpenTelemetry")]
public delegate void TracerProviderJointDelegate(IDovetailContext context, TracerProviderBuilder builder);
