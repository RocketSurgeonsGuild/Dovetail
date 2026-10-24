using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     Binds every binding section registered for <typeparamref name="TOptions" /> onto the default (unnamed) options instance,
///     in the order <see cref="DovetailConfigurationSection{TOptions}.Arrange" /> gives. There is one per options type, at the
///     position the first section for the type was registered.
/// </summary>
/// <remarks>
///     A plain <see cref="IConfigureOptions{TOptions}" />, which <see cref="OptionsFactory{TOptions}" /> only applies to
///     <see cref="Options.DefaultName" />, so named options are left alone.
/// </remarks>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailConfigureOptions<TOptions> : IConfigureOptions<TOptions>
    where TOptions : class
{
    private readonly IConfiguration _configuration;
    private readonly DovetailConfigurationSection<TOptions>[] _sections;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DovetailConfigureOptions{TOptions}" /> class.
    /// </summary>
    /// <param name="configuration">The configuration to bind from.</param>
    /// <param name="sections">The sections registered for <typeparamref name="TOptions" />, in registration order.</param>
    public DovetailConfigureOptions(IConfiguration configuration, IEnumerable<DovetailConfigurationSection<TOptions>> sections)
    {
        _configuration = configuration;
        DovetailConfigurationSection<TOptions>.Arrange(sections, out _, out _sections);
    }

    /// <inheritdoc />
    public void Configure(TOptions options)
    {
        foreach (var section in _sections)
        {
            section.Bind!(_configuration.GetSection(section.Path), options);
        }
    }
}
