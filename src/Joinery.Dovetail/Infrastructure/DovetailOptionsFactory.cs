using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Infrastructure;

/// <summary>
///     Creates <typeparamref name="TOptions" /> from its section of the application's configuration. The base class then runs
///     the configure, post-configure and validation steps as usual.
/// </summary>
/// <typeparam name="TOptions">The options type. <see cref="OptionsFactory{TOptions}" /> is annotated as needing its public parameterless constructor, so this is too.</typeparam>
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
    ///     Creates the instance from the section of the application's configuration.
    /// </summary>
    /// <param name="name">The name of the options instance to create, which must be the default name.</param>
    /// <returns>The created instance.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="name" /> is not <see cref="Options.DefaultName" />.</exception>
    protected override TOptions CreateInstance(string name) =>
        name == Options.DefaultName
            ? _create(_configuration.GetSection(_path))
            : throw new InvalidOperationException(
                $"Dovetail creates only the default (unnamed) {typeof(TOptions).Name} options, but options named '{name}' were requested. Named options aren't supported for types Dovetail registers."
            );
}
