using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Dovetail.Infrastructure;

/// <summary>
///     Signals a reload of the options with one name when the configuration the <see cref="IDovetailConfigurationProvider" />
///     returns for that name reloads.
/// </summary>
/// <remarks>
///     An <see cref="IOptionsChangeTokenSource{TOptions}" /> has one fixed name, so each name needs its own. The provider is
///     asked for the configuration each time a token is requested, which is when the options monitor starts watching and after
///     each change.
/// </remarks>
/// <param name="provider">Supplies the configuration for the name.</param>
/// <param name="name">The options name this source signals changes for.</param>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailConfigurationChangeTokenSource<TOptions>(IDovetailConfigurationProvider provider, string name) : IOptionsChangeTokenSource<TOptions>
{
    /// <inheritdoc />
    public string Name => name;

    /// <inheritdoc />
    public IChangeToken GetChangeToken() => provider.GetConfiguration(name).GetReloadToken();
}

/// <summary>
///     Records that a <see cref="DovetailConfigurationChangeTokenSource{TOptions}" /> is registered for an options name, so that
///     registering one again doesn't add a second.
/// </summary>
/// <param name="Name">The options name.</param>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed record DovetailChangeTokenSourceMarker<TOptions>(string Name);
