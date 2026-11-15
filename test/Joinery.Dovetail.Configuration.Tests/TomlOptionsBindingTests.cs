using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tomlyn.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Tests;

/// <summary>
///     The TOML <see cref="IConfigurationSource" /> composed with
///     <see cref="DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions{TOptions}(IServiceCollection, string, Func{IConfiguration, TOptions})" />
///     (with <c>CreateSample</c> standing in for the generated <c>Create…</c> method): <see cref="IOptions{TOptions}" /> resolution
///     and <see cref="IOptionsMonitor{TOptions}" /> reload-on-file-change. The JSON and YAML cases are in
///     <c>Joinery.Dovetail.Configuration.Runtime.Tests</c>.
/// </summary>
public sealed class TomlOptionsBindingTests
{
    [Test]
    public async Task Should_Resolve_IOptions_And_IOptionsMonitor_For_Toml()
    {
        await RunResolveScenarioAsync(
            "appsettings.toml",
            """
            [Sample]
            Name = "initial"
            Count = 1
            """,
            (directory, fileName) => new ConfigurationBuilder()
                                     .SetBasePath(directory)
                                     .AddTomlFile(fileName, optional: false, reloadOnChange: true)
                                     .Build()
        );
    }

    [Test]
    public async Task Should_Reload_IOptionsMonitor_On_File_Change_For_Toml()
    {
        await RunReloadScenarioAsync(
            "appsettings.toml",
            """
            [Sample]
            Name = "initial"
            Count = 1
            """,
            """
            [Sample]
            Name = "updated"
            Count = 2
            """,
            (directory, fileName) => new ConfigurationBuilder()
                                     .SetBasePath(directory)
                                     .AddTomlFile(fileName, optional: false, reloadOnChange: true)
                                     .Build()
        );
    }

    private static async Task RunResolveScenarioAsync(string fileName, string initialContent, Func<string, string, IConfigurationRoot> buildConfiguration)
    {
        var directory = CreateTempDirectory();
        try
        {
            await File.WriteAllTextAsync(Path.Combine(directory, fileName), initialContent);

            var configuration = buildConfiguration(directory, fileName);
            try
            {
                var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
                services.AddDovetailConfigurationOptions("Sample", CreateSample);

                await using var provider = services.BuildServiceProvider();

                var options = provider.GetRequiredService<IOptions<SampleOptions>>().Value;
                await options.Name.Should().BeEqualTo("initial");
                await options.Count.Should().BeEqualTo(1);

                var monitor = provider.GetRequiredService<IOptionsMonitor<SampleOptions>>();
                await monitor.CurrentValue.Name.Should().BeEqualTo("initial");
                await monitor.CurrentValue.Count.Should().BeEqualTo(1);
            }
            finally
            {
                ( configuration as IDisposable )?.Dispose();
            }
        }
        finally
        {
            TryDeleteDirectory(directory);
        }
    }

    private static async Task RunReloadScenarioAsync(
        string fileName,
        string initialContent,
        string updatedContent,
        Func<string, string, IConfigurationRoot> buildConfiguration
    )
    {
        var directory = CreateTempDirectory();
        try
        {
            var filePath = Path.Combine(directory, fileName);
            await File.WriteAllTextAsync(filePath, initialContent);

            var configuration = buildConfiguration(directory, fileName);
            try
            {
                var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
                services.AddDovetailConfigurationOptions("Sample", CreateSample);

                await using var provider = services.BuildServiceProvider();

                var monitor = provider.GetRequiredService<IOptionsMonitor<SampleOptions>>();
                await monitor.CurrentValue.Name.Should().BeEqualTo("initial");

                var changeReceived = new TaskCompletionSource<SampleOptions>(TaskCreationOptions.RunContinuationsAsynchronously);
                using var subscription = monitor.OnChange(
                    updated =>
                    {
                        if (updated.Name == "updated") changeReceived.TrySetResult(updated);
                    }
                );

                // Give the file watcher a moment to attach before mutating the file.
                await Task.Delay(TimeSpan.FromMilliseconds(250));
                await File.WriteAllTextAsync(filePath, updatedContent);

                using var timeout = new CancellationTokenSource();
                var completed = await Task.WhenAny(changeReceived.Task, Task.Delay(TimeSpan.FromSeconds(15), timeout.Token));
                await timeout.CancelAsync();
                await completed.Should().BeEqualTo(changeReceived.Task).Because("Expected IOptionsMonitor<SampleOptions> to observe the file-system change within the timeout.");

                var updated = await changeReceived.Task;
                await updated.Name.Should().BeEqualTo("updated");
                await updated.Count.Should().BeEqualTo(2);
            }
            finally
            {
                ( configuration as IDisposable )?.Dispose();
            }
        }
        finally
        {
            TryDeleteDirectory(directory);
        }
    }

    private static SampleOptions CreateSample(IConfiguration section) =>
        new() { Name = section["Name"] ?? "", Count = int.Parse(section["Count"] ?? "0", CultureInfo.InvariantCulture) };

    private static string CreateTempDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), "dovetail-config-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        return directory;
    }

    private static void TryDeleteDirectory(string directory)
    {
        try
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, recursive: true);
        }
        catch (IOException)
        {
            // Best-effort cleanup; a lingering file watcher handle on some platforms can briefly hold the
            // directory open after disposal. Not fatal to the test outcome.
        }
    }
}

/// <summary>A settable-property options class, as generated configuration classes are, bound from the <c>Sample</c> section.</summary>
public sealed class SampleOptions
{
    public string Name { get; set; } = "";

    public int Count { get; set; }
}
