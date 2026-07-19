using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Serilog;

/// <summary>
///   A joint that registers logging services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<ILoggingJoint>, DependsOnJoint<LoggingConnector>]
public sealed class SerilogJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken) =>
        new global::Serilog.LoggerConfiguration().ApplySerilog(context, services, cancellationToken);
}
