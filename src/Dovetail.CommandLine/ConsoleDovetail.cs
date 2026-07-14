using Dovetail.Hosting;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Dovetail.CommandLine;

/// <summary>
///     Dovetail for console applications
/// </summary>
[DovetailExport]
public class ConsoleDovetail : IHostApplicationAsyncJoint<IHostApplicationBuilder>
{
    /// <inheritdoc />
    public async ValueTask Register(IDovetailContext context, IHostApplicationBuilder builder, CancellationToken cancellationToken)
    {
        var sourcesToRemove = builder.Configuration.Sources.OfType<CommandLineConfigurationSource>().ToList();
        var appSettings = new AppSettingsConfigurationSource(sourcesToRemove.FirstOrDefault()?.Args ?? Array.Empty<string>());
        builder.Configuration.Add(appSettings);
        context.Set(appSettings);

        var registry = new DovetailTypeRegistrar();
        var command = new CommandApp(registry);
        var consoleResult = new ConsoleResult();
        var found = false;

        command.Configure(
            configurator =>
            {
                var interceptor = new ConsoleInterceptor(
                    // ReSharper disable once NullableWarningSuppressionIsUsed RedundantSuppressNullableWarningExpression
                    context.Get<AppSettingsConfigurationSource>()!
                );
                configurator.SetInterceptor(interceptor);
            }
        );

        foreach (var item in context.Dovetails.GetAll())
        {
            switch (item)
            {
                case ICommandAppDovetail convention:
                    convention.Register(context, command);
                    found = true;
                    break;
                case CommandAppDovetail @delegate:
                    @delegate(context, command);
                    found = true;
                    break;
                case ICommandAppAsyncDovetail convention:
                    await convention.Register(context, command, cancellationToken);
                    found = true;
                    break;
                case CommandAppAsyncDovetail @delegate:
                    await @delegate(context, command, cancellationToken);
                    found = true;
                    break;
                case ICommandLineDovetail convention:
                    command.Configure(configurator => convention.Register(context, configurator));
                    found = true;
                    break;
                case CommandLineDovetail @delegate:
                    command.Configure(configurator => @delegate(context, configurator));
                    found = true;
                    break;
                case ICommandLineAsyncDovetail convention:
                    {
                        var itcs = new TaskCompletionSource();
                        cancellationToken.Register(() => itcs.TrySetCanceled());
                        // ReSharper disable once AsyncVoidLambda
                        command.Configure(
                            async configurator =>
                            {
                                try
                                {
                                    await convention.Register(context, configurator, cancellationToken);
                                    itcs.SetResult();
                                }
                                catch (Exception e)
                                {
                                    itcs.SetException(e);
                                }
                            }
                        );
                        await itcs.Task;
                    }
                    found = true;
                    break;
                case CommandLineAsyncDovetail @delegate:
                    {
                        var dtcs = new TaskCompletionSource();
                        cancellationToken.Register(() => dtcs.TrySetCanceled());
                        // ReSharper disable once AsyncVoidLambda
                        command.Configure(
                            async configurator =>
                            {
                                try
                                {
                                    await @delegate(context, configurator, cancellationToken);
                                    dtcs.SetResult();
                                }
                                catch (Exception e)
                                {
                                    dtcs.SetException(e);
                                }
                            }
                        );
                        found = true;
                        await dtcs.Task;
                    }
                    break;
                default:
                    break;
            }
        }

        command.Configure(
            configurator =>
            {
                var defaultCommandProperty = configurator.GetType().GetProperty("DefaultCommand");
                if (defaultCommandProperty?.GetValue(configurator) == null) command.SetDefaultCommand<DefaultCommand>();
            }
        );

        // We don't want to run if there were no possible command conventions.
        if (!found) return;

        // ReSharper disable once NullableWarningSuppressionIsUsed RedundantSuppressNullableWarningExpression
        builder.Services.AddSingleton(_ => (IAnsiConsole)registry.GetService(typeof(IAnsiConsole))!);
        // ReSharper disable once NullableWarningSuppressionIsUsed RedundantSuppressNullableWarningExpression
        builder.Services.AddSingleton(_ => (IRemainingArguments)registry.GetService(typeof(IRemainingArguments))!);
        builder.Services.AddSingleton(consoleResult);
        builder.Services.AddHostedService<ConsoleWorker>();
        // ReSharper disable once NullableWarningSuppressionIsUsed RedundantSuppressNullableWarningExpression
        builder.Services.AddSingleton(context.Get<AppSettingsConfigurationSource>()!);
        builder.Services.AddSingleton<ICommandApp>(
            provider =>
            {
                registry.SetServiceProvider(provider);
                return command;
            }
        );
    }
}
