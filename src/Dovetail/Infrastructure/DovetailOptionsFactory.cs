using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     Creates <typeparamref name="TOptions" /> from its configuration section, for every options name. The base class then runs
///     the configure, post-configure and validation steps as usual.
/// </summary>
/// <remarks>
///     The configuration for a name comes from the <see cref="IDovetailConfigurationProvider" />, which the default
///     implementation supplies for the default name only.
/// </remarks>
/// <typeparam name="TOptions">The options type. <see cref="OptionsFactory{TOptions}" /> is annotated as needing its public parameterless constructor, so this is too.</typeparam>
/// <remarks>
///     Initializes a new instance of the <see cref="DovetailOptionsFactory{TOptions}" /> class.
/// </remarks>
/// <param name="provider">Supplies the configuration to create each named instance from.</param>
/// <param name="path">The path of the section to create from.</param>
/// <param name="create">Creates an instance from the section.</param>
/// <param name="setups">The configuration actions to run.</param>
/// <param name="postConfigures">The initialization actions to run.</param>
/// <param name="validations">The validations to run.</param>
internal sealed class DovetailOptionsFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
    IDovetailConfigurationProvider provider,
    string path,
    Func<IConfiguration, TOptions> create,
    IEnumerable<IConfigureOptions<TOptions>> setups,
    IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
    IEnumerable<IValidateOptions<TOptions>> validations
    )
    : OptionsFactory<TOptions>(setups, postConfigures, validations)
    where TOptions : class
{
    private readonly IDovetailConfigurationProvider _provider = provider;
    private readonly string _path = path;
    private readonly Func<IConfiguration, TOptions> _create = create;

    /// <summary>
    ///     Creates the instance from the section of the configuration the provider returns for the name.
    /// </summary>
    /// <param name="name">The name of the options instance to create.</param>
    /// <returns>The created instance.</returns>
    protected override TOptions CreateInstance(string name) => _create(_provider.GetConfiguration(name).GetSection(_path));
}
