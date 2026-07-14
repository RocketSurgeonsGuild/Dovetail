using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     Delegate CommandAppDovetail
/// </summary>
/// <param name="context">The context.</param>
/// <param name="app"></param>
public delegate void CommandAppDovetail(IDovetailContext context, CommandApp app);
