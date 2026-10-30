using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SerilogLogger = Serilog.Core.Logger;

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     A logging provider that keeps what it is asked to log, so that a test can look at what the logger forwarded to it.
/// </summary>
public class CapturingProvider : ILoggerProvider
{
    /// <summary>
    ///     What the provider has been asked to log, in order.
    /// </summary>
    public IReadOnlyList<LogEntry> Entries
    {
        get
        {
            lock (_entries)
            {
                return [.. _entries,];
            }
        }
    }

    /// <summary>
    ///     How many times the provider has been disposed.
    /// </summary>
    public int Disposals => Volatile.Read(ref _disposals);

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new CapturingLogger(this, categoryName);

    /// <inheritdoc />
    public void Dispose()
    {
        Interlocked.Increment(ref _disposals);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     One call to a logger of the provider.
    /// </summary>
    public sealed record LogEntry(string Category, LogLevel Level, string Message, Exception? Exception);

    private void Add(LogEntry entry)
    {
        lock (_entries)
        {
            _entries.Add(entry);
        }
    }

    private int _disposals;
    private readonly List<LogEntry> _entries = [];

    private sealed class CapturingLogger(CapturingProvider provider, string category) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            provider.Add(new(category, logLevel, formatter(state, exception), exception));
    }
}

/// <summary>
///     The providers a test would like registered in the services <em>after</em> the Serilog joint has run, as the joint's own services, and what that
///     joint saw. A test passes it to the builder (<c>Set</c>); without it <see cref="LateProviderJoint" /> does nothing.
/// </summary>
public sealed class LateProviders(params ILoggerProvider[] providers)
{
    /// <summary>
    ///     The providers to register.
    /// </summary>
    public IReadOnlyList<ILoggerProvider> Providers { get; } = providers;

    /// <summary>
    ///     Whether the logger was already registered when <see cref="LateProviderJoint" /> ran, which is what it depends on.
    /// </summary>
    public bool LoggerWasRegistered { get; set; }
}

/// <summary>
///     Registers the <see cref="LateProviders" /> of a test, after <see cref="SerilogJoint" />.
/// </summary>
[DovetailExport, DependsOnJoint<SerilogJoint>]
public sealed class LateProviderJoint : IServiceJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IServiceCollection services)
    {
        if (context.Get<LateProviders>() is not { } late)
        {
            return;
        }

        late.LoggerWasRegistered = services.Any(descriptor => descriptor.ServiceType == typeof(SerilogLogger));
        foreach (var provider in late.Providers)
        {
            // By factory, so that the container owns the provider and disposes it, as it does the providers of an application.
            services.AddSingleton(_ => provider);
        }
    }
}
