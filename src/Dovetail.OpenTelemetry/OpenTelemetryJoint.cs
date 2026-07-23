using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;

namespace Dovetail.OpenTelemetry;

/// <summary>
///  A joint that registers OpenTelemetry services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IOpenTelemetryJoint>]
public class OpenTelemetryConnector : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken)
    {
        var builder = new Builder(services);
        return builder.ApplyOpenTelemetry(context, cancellationToken);
    }

    private class Builder(IServiceCollection services) : IOpenTelemetryBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}
