using Dovetail.Infrastructure;
using Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Dovetail.OpenTelemetry.Joints;

/// <summary>
///     Joint interface for <see cref="ResourceBuilder" />.
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface IResourceBuilderJoint : IOpenTelemetryJoint
{
    /// <summary>
    ///     Register the <see cref="ResourceBuilder" /> part.
    /// </summary>
    void Register(IDovetailContext context, ResourceBuilder resourceBuilder);

    void IOpenTelemetryJoint.Register(IDovetailContext context, IOpenTelemetryBuilder builder) => builder.ConfigureResource(b => Register(context, b));
}

/// <summary>
///     Delegate for the <see cref="ResourceBuilder" /> part.
/// </summary>
[global::System.CodeDom.Compiler.GeneratedCode("Dovetail", "generated"), JointName("OpenTelemetry")]
public delegate void ResourceBuilderJointDelegate(global::Dovetail.IDovetailContext context, global::OpenTelemetry.Resources.ResourceBuilder builder);
