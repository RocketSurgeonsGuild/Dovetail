using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     ICommandLineDovetail
///     Implements the <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="IDovetailJoint" />
public interface ICommandAppDovetail : IDovetailJoint
{
    /// <summary>
    ///     Register additional services with the <see cref="CommandApp" />
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="app"></param>
    void Register(IDovetailContext context, CommandApp app);
}
