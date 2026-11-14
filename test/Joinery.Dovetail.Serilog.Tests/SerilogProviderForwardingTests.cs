using Joinery.Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Dovetail.Serilog.Tests;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Extensions.Logging;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     The logging providers registered in the container, other than the default ones that Serilog replaces, keep receiving the events: the logger
///     writes to them as well as to its sinks.
/// </summary>
public class SerilogProviderForwardingTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task A_Provider_Registered_In_The_Container_Receives_The_Events()
    {
        var provider = new CapturingProvider();
        using var host = await BuildAsync(configure: rb => rb.ConfigureServices((_, services) => services.AddSingleton<ILoggerProvider>(provider)));

        var exception = new InvalidOperationException("Broken");
        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogWarning(exception, "Hello {Name}", "Dovetail");
        host.Services.GetRequiredService<SerilogILogger>().ForContext<SerilogProviderForwardingTests>().Information("Hello from Serilog");

        var entries = provider.Entries;
        await entries.Count.Should().BeEqualTo(2);
        await entries[0].Category.Should().BeEqualTo(typeof(SerilogProviderForwardingTests).FullName);
        await entries[0].Level.Should().BeEqualTo(LogLevel.Warning);
        await entries[0].Message.Should().BeEqualTo("Hello \"Dovetail\"");
        await entries[0].Exception.Should().BeSameReferenceAs(exception);
        await entries[1].Category.Should().BeEqualTo(typeof(SerilogProviderForwardingTests).FullName);
        await entries[1].Level.Should().BeEqualTo(LogLevel.Information);
        await entries[1].Message.Should().BeEqualTo("Hello from Serilog");
    }

    [Test]
    public async Task Every_Event_Is_Written_Once_To_Serilog_And_Once_To_Each_Provider()
    {
        var sinkName = NewSinkName();
        CapturingProvider first = new(), second = new();
        using var host = await BuildAsync(
            configure: rb => rb
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                            .ConfigureServices(
                                 (_, services) =>
                                 {
                                     services.AddSingleton<ILoggerProvider>(first);
                                     services.AddSingleton<ILoggerProvider>(second);
                                 }
                             )
        );

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("From the generic logger");
        host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Some.Category").LogError("From the factory");
        host.Services.GetRequiredService<SerilogILogger>().Warning("From Serilog");

        await CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).Should().BeEquivalentTo(["From the generic logger", "From the factory", "From Serilog",], CollectionOrdering.Matching);
        foreach (var provider in new[] { first, second, })
        {
            await MessagesOf(provider).Should().BeEquivalentTo(["From the generic logger", "From the factory", "From Serilog",], CollectionOrdering.Matching);
        }
    }

    [Test]
    public async Task A_Provider_Registered_After_The_Serilog_Joint_Is_Included()
    {
        var provider = new CapturingProvider();
        var late = new LateProviders(provider);
        using var host = await BuildAsync(configure: rb => rb.Set(late));

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await late.LoggerWasRegistered.Should().BeTrue().Because("the provider was registered after the Serilog joint registered the logger");
        await provider.Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Hello",], CollectionOrdering.Matching);
    }

    [Test]
    public async Task A_Provider_Added_Through_The_Logging_Builder_Is_Included()
    {
        var provider = new CapturingProvider();
        using var host = await BuildAsync(configure: rb => rb.ConfigureLogging((_, builder) => builder.AddProvider(provider)));

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await provider.Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Hello",], CollectionOrdering.Matching);
    }

    [Test]
    public async Task The_Providers_Receive_What_The_Minimum_Level_Lets_Through()
    {
        var provider = new CapturingProvider();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", },
            rb => rb.ConfigureServices((_, services) => services.AddSingleton<ILoggerProvider>(provider))
        );

        var logger = host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>();
        logger.LogInformation("Hidden");
        logger.LogWarning("Shown");

        await provider.Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Shown",], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Serilogs_Own_Provider_Is_Not_Written_To_And_Nothing_Is_Written_Twice()
    {
        var sinkName = NewSinkName();
        var otherSinkName = NewSinkName();
        var other = new global::Serilog.LoggerConfiguration().WriteTo.Capturing(otherSinkName).CreateLogger();
        using var host = await BuildAsync(
            configure: rb => rb
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                            .ConfigureServices((_, services) => services.AddSingleton<ILoggerProvider>(_ => new SerilogLoggerProvider(other, dispose: true)))
        );

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await CapturingSink.Named(sinkName).Events.Should().HaveSingleItem();
        await CapturingSink.Named(otherSinkName).Events.Should().BeEmpty().Because("Serilog's own provider is not one that events are forwarded to, or the logger would write to itself");
    }

    [Test]
    public async Task A_Default_Provider_Registered_After_The_Serilog_Joint_Is_Not_Written_To()
    {
        var defaultProvider = FakeProviders.NewDefaultProvider();
        CapturingProvider provider = new();
        using var host = await BuildAsync(configure: rb => rb.Set(new LateProviders(defaultProvider, provider)));

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await defaultProvider.Entries.Should().BeEmpty().Because("the default providers are replaced by Serilog, not written to besides it");
        await provider.Entries.Should().HaveSingleItem();
    }

    [Test]
    public async Task A_Provider_That_Is_Not_A_Default_One_Is_Written_To_Whatever_Its_Namespace()
    {
        // Application Insights' and Azure App Service's providers live in Microsoft.Extensions.Logging.* namespaces too, and are not replaced.
        FakeNamespacedProvider provider = new();
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton<ILoggerProvider>(provider);
        var late = new LateProviders(new FakeNamespacedProvider());
        using var host = await builder.ConfigureDovetail(rb => rb.Set(late));

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await provider.Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Hello",], CollectionOrdering.Matching).Because("registered before the Serilog joint, so it was not removed");
        await ( (CapturingProvider)late.Providers.Single() ).Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Hello",], CollectionOrdering.Matching).Because("registered after it");
    }

    [Test]
    public async Task The_Default_Providers_Stay_Replaced_And_Are_Not_Written_To()
    {
        var sinkName = NewSinkName();
        var provider = new CapturingProvider();
        var builder = Host.CreateApplicationBuilder();
        using var host = await builder.ConfigureDovetail(
            rb => rb
                 .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                 .ConfigureServices((_, services) => services.AddSingleton<ILoggerProvider>(provider))
        );

        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await builder.Services.Count(descriptor => descriptor.ServiceType == typeof(ILoggerProvider)).Should().BeEqualTo(1).Because("only the provider of the test");
        await ( await host.Services.GetServices<ILoggerProvider>().Should().HaveSingleItem() ).Should().BeSameReferenceAs(provider);
        await CapturingSink.Named(sinkName).Events.Should().HaveSingleItem();
        await provider.Entries.Should().HaveSingleItem();
    }

    [Test]
    public async Task A_Provider_Receives_The_Events_On_A_Web_Application_Too()
    {
        var sinkName = NewSinkName();
        var provider = new CapturingProvider();
        await using var app = await WebApplication
                                   .CreateBuilder()
                                   .ConfigureDovetail(
                                        rb => rb
                                             .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                                             .ConfigureServices((_, services) => services.AddSingleton<ILoggerProvider>(provider))
                                    );

        app.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        await CapturingSink.Named(sinkName).Events.Should().HaveSingleItem();
        await provider.Entries.Select(entry => entry.Message).Should().BeEquivalentTo(["Hello",], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Disposing_The_Host_Disposes_The_Providers()
    {
        var provider = new CapturingProvider();
        var host = await BuildAsync(configure: rb => rb.Set(new LateProviders(provider)));
        host.Services.GetRequiredService<ILogger<SerilogProviderForwardingTests>>().LogInformation("Hello");

        host.Dispose();

        // The logger disposes the providers it writes to, and so does the container that owns them: a provider has to cope with that, as it does
        // with any Dispose, and the providers that matter (OpenTelemetry's, say) do.
        await provider.Disposals.Should().BeGreaterThanOrEqualTo(1);
    }

    private static List<string> MessagesOf(CapturingProvider provider)
    {
        var messages = new List<string>(provider.Entries.Count);
        foreach (var entry in provider.Entries)
        {
            messages.Add(entry.Message);
        }

        return messages;
    }
}
