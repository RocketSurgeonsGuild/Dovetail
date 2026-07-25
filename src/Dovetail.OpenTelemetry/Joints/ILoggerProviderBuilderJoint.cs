using Dovetail.Infrastructure;
using Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Dovetail.OpenTelemetry.Joints;

/// <summary>
///     Joint interface for <see cref="LoggerProvider" />.
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface ILoggerProviderBuilderJoint : IOpenTelemetryJoint
{
    /// <summary>
    ///     Register the <see cref="LoggerProviderBuilder" /> part.
    /// </summary>
    void Register(IDovetailContext context, LoggerProviderBuilder loggerProviderBuilder);

    void IOpenTelemetryJoint.Register(IDovetailContext context, IOpenTelemetryBuilder builder) => builder.WithLogging(b => Register(context, b));
}

/// <summary>
///     Delegate for the <see cref="LoggerProviderBuilder" /> part.
/// </summary>
[global::System.CodeDom.Compiler.GeneratedCode("Dovetail", "generated"), JointName("OpenTelemetry")]
public delegate void LoggerProviderJointDelegate(global::Dovetail.IDovetailContext context, global::OpenTelemetry.Logs.LoggerProviderBuilder builder);
