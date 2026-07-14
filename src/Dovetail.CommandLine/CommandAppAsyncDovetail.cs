using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     Delegate CommandAppDovetail
/// </summary>
/// <param name="context">The context.</param>
/// <param name="app"></param>
/// <param name="cancellationToken"></param>
public delegate ValueTask CommandAppAsyncDovetail(IDovetailContext context, CommandApp app, CancellationToken cancellationToken);
