using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Serilog;

/// <summary>
///   A joint that applies the Serilog joints and registers the Serilog logger they configure: the logger, Serilog's <c>ILogger</c> and the
///   <c>ILoggerFactory</c> (which backs every <c>ILogger&lt;T&gt;</c>) take the place of the logging providers the host adds by default.
///   This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
/// <remarks>
///   Once every Serilog joint has run, the <c>Serilog</c> section of the application's configuration is read (if it has one) and, when the logger is
///   first resolved, the <c>IConfigureOptions&lt;LoggerConfiguration&gt;</c> and <c>IPostConfigureOptions&lt;LoggerConfiguration&gt;</c> registered in the
///   services are applied to the logger configuration.
/// </remarks>
[DovetailExport, ConnectsJoint<ILoggingJoint>, DependsOnJoint<LoggingConnector>]
public sealed class SerilogJoint : IServiceAsyncJoint
{
    async ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken)
    {
        var loggerConfiguration = new global::Serilog.LoggerConfiguration();
        await loggerConfiguration.ApplySerilog(context, services, cancellationToken).ConfigureAwait(false);
        SerilogLoggerRegistration.Register(context, services, loggerConfiguration);
    }
}
