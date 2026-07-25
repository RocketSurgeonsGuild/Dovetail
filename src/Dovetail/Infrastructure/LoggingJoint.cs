using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Infrastructure;

/// <summary>
///   A joint that registers logging services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<ILoggingJoint>]
public sealed class LoggingConnector : IServiceAsyncJoint
{
    /// <inheritdoc />
    public ValueTask Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default) =>
        new LoggingBuilder(services).ApplyLogging(context, cancellationToken);
}
