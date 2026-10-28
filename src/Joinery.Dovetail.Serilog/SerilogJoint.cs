using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Serilog;

/// <summary>
///   A joint that registers logging services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<ILoggingJoint>, DependsOnJoint<LoggingConnector>]
public sealed class SerilogJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken) =>
        new global::Serilog.LoggerConfiguration().ApplySerilog(context, services, cancellationToken);
}
