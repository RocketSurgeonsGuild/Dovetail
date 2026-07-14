using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     Delegate CommandLineDovetail
/// </summary>
/// <param name="context">The context.</param>
/// <param name="app"></param>
public delegate void CommandLineDovetail(IDovetailContext context, IConfigurator app);
