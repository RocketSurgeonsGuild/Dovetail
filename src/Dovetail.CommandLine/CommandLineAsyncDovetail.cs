using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     Delegate CommandLineDovetail
/// </summary>
/// <param name="context">The context.</param>
/// <param name="app"></param>
/// <param name="cancellationToken"></param>
public delegate ValueTask CommandLineAsyncDovetail(IDovetailContext context, IConfigurator app, CancellationToken cancellationToken);
