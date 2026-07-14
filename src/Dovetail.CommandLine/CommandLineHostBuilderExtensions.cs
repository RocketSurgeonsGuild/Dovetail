using Dovetail.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

// ReSharper disable once CheckNamespace
namespace Dovetail;

/// <summary>
///     Helper method for working with <see cref="DovetailContextBuilder" />
/// </summary>
[PublicAPI]
public static partial class CommandAppHostBuilderExtensions
{
    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandLine(
        this DovetailContextBuilder container,
        CommandLineDovetail @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(@delegate, priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandLine(
        this DovetailContextBuilder container,
        CommandLineAsyncDovetail @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(@delegate, priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandLine(
        this DovetailContextBuilder container,
        Action<IConfigurator> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandLineDovetail((_, context) => @delegate(context)), priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandLine(
        this DovetailContextBuilder container,
        Func<IConfigurator, ValueTask> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandLineAsyncDovetail((_, context, _) => @delegate(context)), priority, category);
        return container;
    }


    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandLine(
        this DovetailContextBuilder container,
        Func<IConfigurator, CancellationToken, ValueTask> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandLineAsyncDovetail((_, context, ct) => @delegate(context, ct)), priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandApp(
        this DovetailContextBuilder container,
        CommandAppDovetail @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(@delegate, priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandApp(
        this DovetailContextBuilder container,
        CommandAppAsyncDovetail @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(@delegate, priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandApp(
        this DovetailContextBuilder container,
        Action<CommandApp> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandAppDovetail((_, context) => @delegate(context)), priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandApp(
        this DovetailContextBuilder container,
        Func<CommandApp, ValueTask> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandAppAsyncDovetail((_, context, _) => @delegate(context)), priority, category);
        return container;
    }


    /// <summary>
    ///     Configure the commandline delegate to the convention scanner
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="delegate">The delegate.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder ConfigureCommandApp(
        this DovetailContextBuilder container,
        Func<CommandApp, CancellationToken, ValueTask> @delegate,
        int priority = 0,
        DovetailCategory? category = null
    )
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandAppAsyncDovetail((_, context, ct) => @delegate(context, ct)), priority, category);
        return container;
    }

    /// <summary>
    ///     Configure the default command
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="priority">The priority.</param>
    /// <param name="category">The category.</param>
    /// <returns>IDovetailHostBuilder.</returns>
    public static DovetailContextBuilder SetDefaultCommand<TDefaultCommand>(
        this DovetailContextBuilder container,
        int priority = 0,
        DovetailCategory? category = null
    )
        where TDefaultCommand : class, ICommand
    {
        ArgumentNullException.ThrowIfNull(container);
        container.AppendDelegate(new CommandAppDovetail((_, context) => context.SetDefaultCommand<TDefaultCommand>()), priority, category);
        return container;
    }

    /// <summary>
    ///     Run the host as a commandline application and return the result
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<int> RunConsoleAppAsync(this ValueTask<IHost> host, CancellationToken cancellationToken = default) => await RunConsoleAppAsync(await host, cancellationToken);

    /// <summary>
    ///     Run the host as a commandline application and return the result
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<int> RunConsoleAppAsync(this Task<IHost> host, CancellationToken cancellationToken = default) => await RunConsoleAppAsync(await host, cancellationToken);

    /// <summary>
    ///     Run the host as a commandline application and return the result
    /// </summary>
    /// <param name="host"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<int> RunConsoleAppAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        var result = host.Services.GetService<ConsoleResult>();
        if (result == null) LogWarning(host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(CommandAppHostBuilderExtensions)));

        await host.StartAsync(cancellationToken);
        await host.WaitForShutdownAsync(cancellationToken);
        return result.ExitCode ?? Environment.ExitCode;
    }

    [LoggerMessage(
        Message = "No commands have been configured, are you trying to run a console app? Try adding some commands for it to work correctly.",
        Level = LogLevel.Warning
    )]
    static partial void LogWarning(ILogger logger);
}
