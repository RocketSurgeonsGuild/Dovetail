using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     Creates the default (unnamed) <typeparamref name="TOptions" /> from the creating sections registered for it, for types the
///     options pipeline can't create with a public parameterless constructor. The base class then runs the configure,
///     post-configure and validation steps as usual.
/// </summary>
/// <remarks>
///     Named options are created by the base class.
/// </remarks>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailOptionsFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>
    : OptionsFactory<TOptions>
    where TOptions : class
{
    private readonly IConfiguration _configuration;
    private readonly DovetailConfigurationSection<TOptions>[] _sections;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DovetailOptionsFactory{TOptions}" /> class.
    /// </summary>
    /// <param name="configuration">The configuration to create from.</param>
    /// <param name="sections">The sections registered for <typeparamref name="TOptions" />, in registration order.</param>
    /// <param name="setups">The configuration actions to run.</param>
    /// <param name="postConfigures">The initialization actions to run.</param>
    /// <param name="validations">The validations to run.</param>
    public DovetailOptionsFactory(
        IConfiguration configuration,
        IEnumerable<DovetailConfigurationSection<TOptions>> sections,
        IEnumerable<IConfigureOptions<TOptions>> setups,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
        IEnumerable<IValidateOptions<TOptions>> validations
    ) : base(setups, postConfigures, validations)
    {
        _configuration = configuration;
        DovetailConfigurationSection<TOptions>.Arrange(sections, out _sections, out _);
    }

    /// <summary>
    ///     Creates the instance. For the default name, one creating section creates it from its configuration section. Several
    ///     are merged first, in order, into one configuration, so later sections override earlier ones key by key, and the first
    ///     section creates the instance from that.
    /// </summary>
    /// <param name="name">The name of the options instance to create.</param>
    /// <returns>The created instance.</returns>
    protected override TOptions CreateInstance(string name)
    {
        if (name != Options.DefaultName || _sections.Length == 0)
        {
            return base.CreateInstance(name);
        }

        var first = _sections[0];
        if (_sections.Length == 1)
        {
            return first.Create!(_configuration.GetSection(first.Path));
        }

        var builder = new ConfigurationBuilder();
        foreach (var section in _sections)
        {
            builder.AddConfiguration(_configuration.GetSection(section.Path), shouldDisposeConfiguration: false);
        }

        // The merged root registers a change callback on each section's reload token (that is, on the root's), so dispose it
        // rather than leave one more registration behind every time the options are created.
        var merged = builder.Build();
        try
        {
            return first.Create!(merged);
        }
        finally
        {
            ( merged as IDisposable )?.Dispose();
        }
    }
}
