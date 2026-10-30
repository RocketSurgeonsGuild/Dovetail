using System.Collections.Concurrent;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     A sink that keeps the events it is given, in memory, so a test can look at what a Serilog logger wrote. Sinks are looked up by name,
///     so that the same one can be reached from code (<c>WriteTo.Capturing("name")</c>) and from the <c>Serilog</c> configuration section
///     (<c>{ "Name": "Capturing", "Args": { "name": "name" } }</c>).
/// </summary>
public sealed class CapturingSink : ILogEventSink, IDisposable
{
    /// <summary>
    ///     Gets the sink with the given name, creating it the first time. Tests use a name of their own, so they don't see each other's events.
    /// </summary>
    public static CapturingSink Named(string name) => Sinks.GetOrAdd(name, static _ => new CapturingSink());

    /// <summary>
    ///     The events written so far.
    /// </summary>
    public IReadOnlyList<LogEvent> Events
    {
        get
        {
            lock (_events)
            {
                return [.. _events,];
            }
        }
    }

    /// <summary>
    ///     Whether the logger that owns the sink has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <inheritdoc />
    public void Emit(LogEvent logEvent)
    {
        lock (_events)
        {
            _events.Add(logEvent);
        }
    }

    /// <inheritdoc />
    public void Dispose() => IsDisposed = true;

    private static readonly ConcurrentDictionary<string, CapturingSink> Sinks = new(StringComparer.Ordinal);
    private readonly List<LogEvent> _events = [];
}

/// <summary>
///     The configuration methods Serilog.Settings.Configuration finds by name in this assembly (it is listed under <c>Using</c>).
/// </summary>
public static class CapturingSinkExtensions
{
    /// <summary>
    ///     Writes to the <see cref="CapturingSink" /> with the given name.
    /// </summary>
    public static LoggerConfiguration Capturing(this LoggerSinkConfiguration sinkConfiguration, string name, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
    {
        ArgumentNullException.ThrowIfNull(sinkConfiguration);
        return sinkConfiguration.Sink(CapturingSink.Named(name), restrictedToMinimumLevel);
    }

    /// <summary>
    ///     Adds a <c>Marker</c> property to every event.
    /// </summary>
    public static LoggerConfiguration WithMarker(this LoggerEnrichmentConfiguration enrichmentConfiguration, string value)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration);
        return enrichmentConfiguration.WithProperty("Marker", value);
    }
}
