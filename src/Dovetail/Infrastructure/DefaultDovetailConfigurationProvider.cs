using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     The configuration provider Dovetail registers unless the application registers its own: the application's configuration
///     for the default options, and no configuration for named ones.
/// </summary>
/// <param name="configuration">The application's configuration.</param>
internal sealed class DefaultDovetailConfigurationProvider(IConfiguration configuration) : IDovetailConfigurationProvider
{
    /// <inheritdoc />
    /// <exception cref="InvalidOperationException"><paramref name="optionsName" /> is not the default name.</exception>
    public IConfiguration GetConfiguration(string optionsName) =>
        optionsName == Options.DefaultName
            ? configuration
            : throw new InvalidOperationException(
                $"No configuration is available for the options named '{optionsName}'. Dovetail creates only the default (unnamed) options from the application's configuration. "
              + $"Register an {nameof(IDovetailConfigurationProvider)} to supply configuration for named options."
            );
}
