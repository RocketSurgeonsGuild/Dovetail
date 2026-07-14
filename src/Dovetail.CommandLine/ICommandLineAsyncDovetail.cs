using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     ICommandLineDovetail
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface ICommandLineAsyncDovetail : IDovetailJoint
{
    /// <summary>
    ///     Register additional services with the command line
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="app"></param>
    /// <param name="cancellationToken"></param>
    ValueTask Register(IDovetailContext context, IConfigurator app, CancellationToken cancellationToken);
}
