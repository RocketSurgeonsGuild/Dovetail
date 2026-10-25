using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dovetail.Infrastructure;

/// <summary>
///     Creates the default (unnamed) <typeparamref name="TOptions" /> from its configuration section, for types the options
///     pipeline can't create with a public parameterless constructor. The base class then runs the configure, post-configure and
///     validation steps as usual.
/// </summary>
/// <remarks>
///     Named options are created by the base class.
/// </remarks>
/// <typeparam name="TOptions">The options type.</typeparam>
/// <remarks>
///     Initializes a new instance of the <see cref="DovetailOptionsFactory{TOptions}" /> class.
/// </remarks>
/// <param name="configuration">The application's configuration.</param>
/// <param name="path">The path of the section to create from.</param>
/// <param name="create">Creates an instance from the section.</param>
/// <param name="setups">The configuration actions to run.</param>
/// <param name="postConfigures">The initialization actions to run.</param>
/// <param name="validations">The validations to run.</param>
internal sealed class DovetailOptionsFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(
    IConfiguration configuration,
    string path,
    Func<IConfiguration, TOptions> create,
    IEnumerable<IConfigureOptions<TOptions>> setups,
    IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
    IEnumerable<IValidateOptions<TOptions>> validations
    )
    : OptionsFactory<TOptions>(setups, postConfigures, validations)
    where TOptions : class
{
    private readonly IConfiguration _configuration = configuration;
    private readonly string _path = path;
    private readonly Func<IConfiguration, TOptions> _create = create;

    /// <summary>
    ///     Creates the instance. The default name is created from the configuration section; other names by the base class.
    /// </summary>
    /// <param name="name">The name of the options instance to create.</param>
    /// <returns>The created instance.</returns>
    protected override TOptions CreateInstance(string name) =>
        name == Options.DefaultName ? _create(_configuration.GetSection(_path)) : base.CreateInstance(name);
}
