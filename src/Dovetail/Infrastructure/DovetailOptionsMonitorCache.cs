using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Dovetail.Infrastructure;

/// <summary>
///     The cache <see cref="IOptionsMonitor{TOptions}" /> keeps its instances in. It drops the instance of a named options when
///     the configuration the <see cref="IDovetailConfigurationProvider" /> returned for that name reloads, so the next read
///     creates it again from the reloaded configuration.
/// </summary>
/// <remarks>
///     <para>
///         The default name is left to the change token source Dovetail registers for it, which also tells
///         <see cref="IOptionsMonitor{TOptions}.OnChange" /> listeners. A named options is dropped without telling them,
///         since the monitor only notifies for the names of its change token sources. A snapshot never uses this cache, so
///         a snapshot reads current values without it.
///     </para>
/// </remarks>
/// <param name="provider">Supplies the configuration to watch for a name.</param>
/// <typeparam name="TOptions">The options type. <see cref="IOptionsMonitorCache{TOptions}" /> is annotated as needing its public parameterless constructor, so this is too.</typeparam>
internal sealed class DovetailOptionsMonitorCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(IDovetailConfigurationProvider provider) : IOptionsMonitorCache<TOptions>, IDisposable
    where TOptions : class
{
    private readonly ConcurrentDictionary<string, Lazy<TOptions>> _cache = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IDisposable> _watches = [];
    private readonly Lock _watchesLock = new();
    private bool _disposed;

    /// <inheritdoc />
    public TOptions GetOrAdd(string? name, Func<TOptions> createOptions)
    {
        ArgumentNullException.ThrowIfNull(createOptions);
        var key = name ?? Options.DefaultName;
        var lazy = _cache.GetOrAdd(
            key,
            static (key, state) => new(
                () =>
                {
                    // Watching before creating means a reload during creation still drops the instance.
                    state.cache.Watch(key);
                    return state.createOptions();
                }
            ),
            (cache: this, createOptions)
        );
        try
        {
            return lazy.Value;
        }
        catch
        {
            // A failed creation (a validation failure, say) isn't cached, so the next read tries again.
            _cache.TryRemove(new KeyValuePair<string, Lazy<TOptions>>(key, lazy));
            throw;
        }
    }

    /// <inheritdoc />
    public bool TryAdd(string? name, TOptions options) => _cache.TryAdd(name ?? Options.DefaultName, new(() => options));

    /// <inheritdoc />
    public bool TryRemove(string? name) => _cache.TryRemove(name ?? Options.DefaultName, out _);

    /// <inheritdoc />
    public void Clear() => _cache.Clear();

    /// <inheritdoc />
    public void Dispose()
    {
        lock (_watchesLock)
        {
            _disposed = true;
            foreach (var watch in _watches.Values) watch.Dispose();
            _watches.Clear();
        }
    }

    private void Watch(string name)
    {
        if (name == Options.DefaultName) return;
        lock (_watchesLock)
        {
            if (_disposed || _watches.ContainsKey(name)) return;
            // OnChange asks the provider for the token straight away, so a name it has no configuration for throws here, as it
            // does when the options are created. It asks again after each change, which keeps the watch going.
            _watches[name] = ChangeToken.OnChange(() => provider.GetConfiguration(name).GetReloadToken(), () => _cache.TryRemove(name, out _));
        }
    }
}
