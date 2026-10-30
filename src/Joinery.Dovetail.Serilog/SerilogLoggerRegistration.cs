using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using SerilogILogger = Serilog.ILogger;
using SerilogLogger = Serilog.Core.Logger;

namespace Joinery.Dovetail.Serilog;

/// <summary>
///     Turns the <see cref="LoggerConfiguration" /> that the Serilog joints have configured into the application's logging: the
///     <see cref="SerilogLogger" />, Serilog's <see cref="SerilogILogger" />, the <see cref="ILoggerFactory" /> (and with it every
///     <see cref="ILogger{TCategoryName}" />) are registered, and replace the providers the host adds by default.
/// </summary>
internal static class SerilogLoggerRegistration
{
    /// <summary>
    ///     The namespace the logging providers that ship with Microsoft.Extensions.Logging (console, debug, event source, event log, ...) live in.
    /// </summary>
    private const string DefaultProviderNamespacePrefix = "Microsoft.Extensions.Logging.";

    /// <summary>
    ///     Reads the minimum levels of the <c>Serilog</c> configuration section into <paramref name="loggerConfiguration" />, drops the default
    ///     logging providers and registers the logger, <see cref="SerilogILogger" /> and <see cref="ILoggerFactory" />.
    /// </summary>
    /// <remarks>
    ///     The logger is created the first time it is asked for, so that the <see cref="IConfigureOptions{TOptions}" /> and
    ///     <see cref="IPostConfigureOptions{TOptions}" /> registered for <see cref="LoggerConfiguration" /> can use the built service provider.
    /// </remarks>
    public static void Register(IDovetailContext context, IServiceCollection services, LoggerConfiguration loggerConfiguration)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        // The configuration comes after the joints, so the levels in it win over the ones they set (a minimum level is last write wins).
        // The options-based configuration comes last, once the service provider can be used. A host that has no configuration to offer
        // (a bare service collection in a test, say) simply has no section to read.
        if (context.Get<IConfiguration>() is { } configuration && SerilogMinimumLevelConfiguration.Apply(configuration, loggerConfiguration) is { } minimumLevels)
        {
            // Registered so that the container disposes it with the logger (CreateLogger resolves it), which stops it following reloads.
            services.AddSingleton(_ => minimumLevels);
        }

        // AddLogging makes sure ILogger<T> is registered even when the host didn't, then the default providers and factory give way to Serilog's.
        services.AddLogging();
        RemoveDefaultLoggerProviders(services);
        services.RemoveAll<ILoggerFactory>();
        services.RemoveAll<SerilogILogger>();
        services.RemoveAll<SerilogLogger>();

        // The container owns the logger and disposes it, which flushes the sinks, so the factory must not dispose it a second time.
        services.AddSingleton(provider => CreateLogger(provider, loggerConfiguration));
        services.AddSingleton<SerilogILogger>(provider => provider.GetRequiredService<SerilogLogger>());
        services.AddSingleton<ILoggerFactory>(provider => new SerilogLoggerFactory(provider.GetRequiredService<SerilogLogger>(), dispose: false));
    }

    /// <summary>
    ///     Applies the <see cref="IConfigureOptions{TOptions}" /> and then the <see cref="IPostConfigureOptions{TOptions}" /> of the default
    ///     (unnamed) <see cref="LoggerConfiguration" /> options to <paramref name="loggerConfiguration" /> and creates the logger, the same
    ///     order the options pattern builds an options instance in.
    /// </summary>
    private static SerilogLogger CreateLogger(IServiceProvider services, LoggerConfiguration loggerConfiguration)
    {
        _ = services.GetService<SerilogMinimumLevelConfiguration>();

        foreach (var setup in services.GetServices<IConfigureOptions<LoggerConfiguration>>())
        {
            if (setup is IConfigureNamedOptions<LoggerConfiguration> namedSetup)
            {
                namedSetup.Configure(Options.DefaultName, loggerConfiguration);
            }
            else
            {
                setup.Configure(loggerConfiguration);
            }
        }

        foreach (var postSetup in services.GetServices<IPostConfigureOptions<LoggerConfiguration>>())
        {
            postSetup.PostConfigure(Options.DefaultName, loggerConfiguration);
        }

        return loggerConfiguration.CreateLogger();
    }

    /// <summary>
    ///     Removes the providers that <c>Microsoft.Extensions.Logging</c> ships and the host registers by default. The factory is replaced
    ///     as well, so nothing would read them, but leaving them registered would have them show up as providers that are still there.
    /// </summary>
    private static void RemoveDefaultLoggerProviders(IServiceCollection services)
    {
        foreach (var descriptor in services.Where(IsDefaultLoggerProvider).ToArray())
        {
            services.Remove(descriptor);
        }
    }

    private static bool IsDefaultLoggerProvider(ServiceDescriptor descriptor) =>
        descriptor.ServiceType == typeof(ILoggerProvider)
     && ( descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType )?.FullName is { } name
     && name.StartsWith(DefaultProviderNamespacePrefix, StringComparison.Ordinal);
}
