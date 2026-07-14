using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;

namespace Dovetail.Hosting;

[DovetailExport]
internal class OpenTelemetryJoint : IServiceAsyncJoint, IOpenTelemetryAsyncJoint
{
    public ValueTask Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default)
    {
        var builder = new Builder(services);
        return builder.ApplyOpenTelemetry(context, cancellationToken);
    }

    public ValueTask Register(IDovetailContext context, IOpenTelemetryBuilder builder, CancellationToken cancellationToken = default)
    {
        builder.ConfigureResource(rb => rb.ApplyResourceBuilder(context, cancellationToken));
        builder.WithTracing(tp => tp.ApplyTracerProvider(context, cancellationToken));
        builder.WithMetrics(mp => mp.ApplyMeterProvider(context, cancellationToken));
        builder.WithLogging(lp => lp.ApplyLoggerProvider(context, cancellationToken));
        return ValueTask.CompletedTask;
    }

    private class Builder(IServiceCollection services) : IOpenTelemetryBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}
