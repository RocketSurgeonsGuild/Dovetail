using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Dovetail.Infrastructure;

/// <summary>
///     Tells <see cref="IOptionsMonitor{TOptions}" /> to rebuild the default (unnamed) <typeparamref name="TOptions" /> when the
///     configuration reloads. A section's reload token is its root's, so one source covers every section registered for the type.
/// </summary>
/// <remarks>
///     Its own type, rather than <see cref="ConfigurationChangeTokenSource{TOptions}" />, so registering it with
///     <c>TryAddEnumerable</c> neither replaces nor is replaced by a source the application registered itself.
/// </remarks>
/// <param name="configuration">The configuration whose reloads to observe.</param>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailConfigurationChangeTokenSource<TOptions>(IConfiguration configuration) : IOptionsChangeTokenSource<TOptions>
{
    /// <inheritdoc />
    public string Name => Options.DefaultName;

    /// <inheritdoc />
    public IChangeToken GetChangeToken() => configuration.GetReloadToken();
}
