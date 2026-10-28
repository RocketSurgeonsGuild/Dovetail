using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;

namespace Joinery.Dovetail.OpenTelemetry;

/// <summary>
///  A joint that registers OpenTelemetry services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IOpenTelemetryJoint>]
public class OpenTelemetryConnector : IServiceAsyncJoint
{
#if NET11_0_OR_GREATER
    /// <summary>
    ///     ASP.NET Core 11 publishes its request activities on this <see cref="System.Diagnostics.ActivitySource" />.
    /// </summary>
    internal const string AspNetCoreActivitySourceName = "Microsoft.AspNetCore";
#endif

    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken)
    {
        var builder = new Builder(services);
#if NET11_0_OR_GREATER
        builder.WithTracing(static tracing => tracing.AddSource(AspNetCoreActivitySourceName));
#endif
        return builder.ApplyOpenTelemetry(context, cancellationToken);
    }

    private class Builder(IServiceCollection services) : IOpenTelemetryBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}
