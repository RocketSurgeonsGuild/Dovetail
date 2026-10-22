using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace Dovetail.Hosting.Uno.Tests;

/// <summary>
///     Stands in for Uno.Extensions' <c>ApplicationBuilder</c>, which needs a running Uno application: it queues the
///     <c>Configure</c> callbacks and replays them onto a fresh host builder in <see cref="Build" />, as the real one does.
///     There is no application, window, or launch arguments.
/// </summary>
internal sealed class TestApplicationBuilder : IApplicationBuilder
{
    private readonly List<Action<IHostBuilder, Window>> _delegates = [];

    public Application App => null!;
    public LaunchActivatedEventArgs Arguments => null!;
    public Window Window => null!;
    public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

    public IApplicationBuilder Configure(Action<IHostBuilder> configureHost)
    {
        _delegates.Add((builder, _) => configureHost(builder));
        return this;
    }

    public IApplicationBuilder Configure(Action<IHostBuilder, Window> configureHost)
    {
        _delegates.Add(configureHost);
        return this;
    }

    public IHost Build()
    {
        // Uno's own HostBuilder, as UnoHost.CreateDefaultBuilder uses (not Microsoft.Extensions.Hosting's).
        var builder = new global::Uno.Extensions.Hosting.HostBuilder();
        foreach (var configure in _delegates)
        {
            configure(builder, null!);
        }

        return builder.Build()!;
    }
}
