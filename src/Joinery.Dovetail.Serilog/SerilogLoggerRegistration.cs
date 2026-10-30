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
///     <see cref="ILogger{TCategoryName}" />) are registered, and replace the providers the host adds by default. The providers that are
///     registered besides those (OpenTelemetry's, say) are written to by the logger, so they keep receiving events.
/// </summary>
internal static class SerilogLoggerRegistration
{
    /// <summary>
    ///     The full names of the logging providers that the host builders register by default and that Serilog replaces: the console, debug, event
    ///     source and (on Windows) event log providers of Microsoft.Extensions.Logging. Names rather than types, so the package needs none of the
    ///     packages that hold them. Other providers in the same namespaces (Application Insights', Azure App Service's) are not among them.
    /// </summary>
    private static readonly HashSet<string> DefaultProviderTypeNames =
    [
        "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider",
        "Microsoft.Extensions.Logging.Debug.DebugLoggerProvider",
        "Microsoft.Extensions.Logging.EventSource.EventSourceLoggerProvider",
        "Microsoft.Extensions.Logging.EventLog.EventLogLoggerProvider",
    ];

    /// <summary>
    ///     Reads the minimum levels of the <c>Serilog</c> configuration section into <paramref name="loggerConfiguration" />, drops the default
    ///     logging providers and registers the logger, <see cref="SerilogILogger" /> and <see cref="ILoggerFactory" />.
    /// </summary>
    /// <remarks>
    ///     The logger is created the first time it is asked for, so that the <see cref="IConfigureOptions{TOptions}" /> and
    ///     <see cref="IPostConfigureOptions{TOptions}" /> registered for <see cref="LoggerConfiguration" /> can use the built service provider.
    ///     The logger writes to a <see cref="LoggerProviderCollection" /> as well, which the logger factory fills with the
    ///     <see cref="ILoggerProvider" />s of the container when it is created, so the providers registered after this ran are included.
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

        // The other providers receive every event the logger writes: the logger factory adds them to this collection once the container can be asked.
        LoggerProviderCollection loggerProviders = new();
        loggerConfiguration.WriteTo.Providers(loggerProviders);

        // AddLogging makes sure ILogger<T> is registered even when the host didn't, then the default providers and factory give way to Serilog's.
        services.AddLogging();
        RemoveDefaultLoggerProviders(services);
        services.RemoveAll<ILoggerFactory>();
        services.RemoveAll<SerilogILogger>();
        services.RemoveAll<SerilogLogger>();

        // The container owns the logger and disposes it, which flushes the sinks, so the factory must not dispose it a second time.
        services.AddSingleton(provider => CreateLogger(provider, loggerConfiguration));
        services.AddSingleton<SerilogILogger>(provider => provider.GetRequiredService<SerilogLogger>());
        services.AddSingleton<ILoggerFactory>(provider => CreateLoggerFactory(provider, loggerProviders));
    }

    /// <summary>
    ///     Creates the factory over the logger, and adds the providers of the container to the collection that the logger writes to.
    /// </summary>
    /// <remarks>
    ///     The container owns the providers and disposes them. The logger disposes the collection, and so the providers, with its sinks as well, so a
    ///     provider can be disposed twice, which <see cref="IDisposable" /> allows. A provider that needs the logger factory itself to be created
    ///     can't be forwarded to: that is a cycle, which the container reports.
    /// </remarks>
    private static SerilogLoggerFactory CreateLoggerFactory(IServiceProvider services, LoggerProviderCollection loggerProviders)
    {
        // The container owns the logger and disposes it, which flushes the sinks, so the factory must not dispose it a second time.
        SerilogLoggerFactory factory = new(services.GetRequiredService<SerilogLogger>(), dispose: false, loggerProviders);
        foreach (var loggerProvider in services.GetServices<ILoggerProvider>())
        {
            if (IsForwardedTo(loggerProvider))
            {
                factory.AddProvider(loggerProvider);
            }
        }

        return factory;
    }

    /// <summary>
    ///     Whether events are forwarded to the provider. Not to Serilog's own provider (the logger would write to itself, without end), and not to
    ///     the default providers, which Serilog replaces, in case one is registered after they were removed.
    /// </summary>
    private static bool IsForwardedTo(ILoggerProvider loggerProvider) => loggerProvider is not SerilogLoggerProvider && !IsDefaultLoggerProvider(loggerProvider.GetType());

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
    ///     Removes the providers that the host registers by default (<see cref="DefaultProviderTypeNames" />). The factory is replaced
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
        descriptor.ServiceType == typeof(ILoggerProvider) && IsDefaultLoggerProvider(descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType);

    private static bool IsDefaultLoggerProvider(Type? providerType) => providerType?.FullName is { } name && DefaultProviderTypeNames.Contains(name);
}
