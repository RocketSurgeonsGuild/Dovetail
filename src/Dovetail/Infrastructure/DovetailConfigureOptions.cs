using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     Binds one configuration section onto the default (unnamed) <typeparamref name="TOptions" /> instance the options pipeline
///     created. Every registration adds its own, and the pipeline applies them in registration order.
/// </summary>
/// <remarks>
///     A plain <see cref="IConfigureOptions{TOptions}" />, which <see cref="OptionsFactory{TOptions}" /> only applies to
///     <see cref="Options.DefaultName" />, so named options are left alone.
/// </remarks>
/// <param name="configuration">The application's configuration.</param>
/// <param name="path">The path of the section to bind.</param>
/// <param name="bind">Binds the section onto the instance.</param>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailConfigureOptions<TOptions>(IConfiguration configuration, string path, Action<IConfiguration, TOptions> bind)
    : IConfigureOptions<TOptions>
    where TOptions : class
{
    /// <inheritdoc />
    public void Configure(TOptions options) => bind(configuration.GetSection(path), options);
}
