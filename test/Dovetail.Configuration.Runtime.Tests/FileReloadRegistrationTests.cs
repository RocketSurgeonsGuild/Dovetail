using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Checks that a change to a JSON file reaches options registered through both reflection-free overloads of
///     <see cref="DovetailConfigurationOptionsExtensions" />, through <see cref="IOptionsMonitor{TOptions}" /> and
///     <see cref="IOptionsSnapshot{TOptions}" />.
/// </summary>
public sealed class FileReloadRegistrationTests
{
    [Test]
    public async Task JsonFileChange_ReachesMonitorAndSnapshot_ForBindingAndCreatingSections()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dovetail-config-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var filePath = Path.Combine(directory, "appsettings.json");
            await File.WriteAllTextAsync(filePath, Settings("initial", 8080));

            var configuration = new ConfigurationBuilder()
                               .SetBasePath(directory)
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                               .Build();
            try
            {
                var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
                services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind);
                services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

                await using var provider = services.BuildServiceProvider();

                var greetings = provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>();
                var servers = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
                greetings.CurrentValue.Message.ShouldBe("initial");
                servers.CurrentValue.Port.ShouldBe(8080);

                var greetingsChanged = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                var serversChanged = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                using var greetingsSubscription = greetings.OnChange(
                    options =>
                    {
                        if (options.Message == "updated") greetingsChanged.TrySetResult();
                    }
                );
                using var serversSubscription = servers.OnChange(
                    options =>
                    {
                        if (options.Port == 9090) serversChanged.TrySetResult();
                    }
                );

                // Give the file watcher a moment to attach before changing the file.
                await Task.Delay(TimeSpan.FromMilliseconds(250));
                await File.WriteAllTextAsync(filePath, Settings("updated", 9090));

                await Task.WhenAll(greetingsChanged.Task, serversChanged.Task).WaitAsync(TimeSpan.FromSeconds(15));

                greetings.CurrentValue.Message.ShouldBe("updated");
                servers.CurrentValue.Port.ShouldBe(9090);
                using var scope = provider.CreateScope();
                scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<GreetingsOptions>>().Value.Message.ShouldBe("updated");
                scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>().Value.Port.ShouldBe(9090);
            }
            finally
            {
                ( configuration as IDisposable )?.Dispose();
            }
        }
        finally
        {
            try
            {
                Directory.Delete(directory, recursive: true);
            }
            catch (IOException)
            {
                // Best effort: a file watcher can hold the directory open briefly after disposal.
            }
        }
    }

    private static string Settings(string message, int port) =>
        $$"""
          {
            "Greetings": { "Message": "{{message}}" },
            "Server": { "Host": "example.org", "Port": {{port}} }
          }
          """;
}
