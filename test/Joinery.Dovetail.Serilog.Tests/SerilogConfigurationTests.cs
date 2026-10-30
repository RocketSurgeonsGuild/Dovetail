using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Extensions.Testing;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     The <c>Serilog</c> section of the application's configuration controls the logger: its minimum level, its enrichers and its sinks.
/// </summary>
public class SerilogConfigurationTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task The_Minimum_Level_And_Its_Overrides_Are_Read_From_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            Settings(sinkName, ("Serilog:MinimumLevel:Default", "Warning"), ("Serilog:MinimumLevel:Override:Noisy", "Error"))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hidden by the default level");
        logger.Warning("Shown");
        var noisy = logger.ForContext(Constants.SourceContextPropertyName, "Noisy.Component");
        noisy.Warning("Hidden by the override");
        noisy.Error("Shown from the override");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown", "Shown from the override",]);
    }

    [Test]
    public async Task The_Minimum_Level_Follows_The_Configuration_When_It_Reloads()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(Settings(sinkName, ("Serilog:MinimumLevel:Default", "Warning")));
        var logger = host.Services.GetRequiredService<SerilogILogger>();

        logger.Debug("Hidden before the change");
        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();
        configuration["Serilog:MinimumLevel:Default"] = "Debug";
        configuration.Reload();
        logger.Debug("Shown after the change");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown after the change",]);
    }

    [Test]
    public async Task Sinks_Are_Read_From_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(Settings(sinkName));

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");
        host.Services.GetRequiredService<ILogger<SerilogConfigurationTests>>().LogInformation("Hello again");

        CapturingSink.Named(sinkName).Events.Count.ShouldBe(2);
    }

    [Test]
    public async Task Enrichers_Are_Read_From_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            Settings(sinkName, ("Serilog:Enrich:0:Name", nameof(CapturingSinkExtensions.WithMarker)), ("Serilog:Enrich:0:Args:value", "from configuration"))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        var logEvent = CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
        ( (ScalarValue)logEvent.Properties["Marker"] ).Value.ShouldBe("from configuration");
    }

    [Test]
    public async Task The_Configuration_Adds_To_The_Sinks_And_Enrichers_Of_The_Joints()
    {
        var jointSinkName = NewSinkName();
        var configuredSinkName = NewSinkName();
        using var host = await BuildAsync(
            Settings(configuredSinkName, ("Serilog:Properties:FromConfiguration", "yes")),
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.Enrich.WithProperty("FromJoint", "yes").WriteTo.Capturing(jointSinkName))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        foreach (var logEvent in new[] { CapturingSink.Named(jointSinkName).Events.ShouldHaveSingleItem(), CapturingSink.Named(configuredSinkName).Events.ShouldHaveSingleItem(), })
        {
            logEvent.Properties.Keys.ShouldContain("FromJoint");
            logEvent.Properties.Keys.ShouldContain("FromConfiguration");
        }
    }

    [Test]
    public async Task The_Configuration_Wins_Over_The_Minimum_Level_Of_The_Joints()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.MinimumLevel.Debug().WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hidden");
        logger.Warning("Shown");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Options_Run_After_The_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            Settings(sinkName, ("Serilog:MinimumLevel:Default", "Warning")),
            rb => rb.ConfigureServices((_, services) => services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.MinimumLevel.Error()))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Warning("Hidden by the options");
        logger.Error("Shown");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Without_A_Serilog_Section_Only_The_Joints_Configure_The_Logger()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["SomethingElse:MinimumLevel:Default"] = "Fatal", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Debug("Hidden by Serilog's own default");
        logger.Information("Shown");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown",]);
    }

    // Serilog's SelfLog is process-wide, so nothing else may run while it is on.
    [Test, NotInParallel]
    public async Task A_Sink_The_Configuration_Cannot_Find_Is_Skipped_And_Reported_To_The_SelfLog()
    {
        var sinkName = NewSinkName();
        var selfLog = new StringWriter();
        global::Serilog.Debugging.SelfLog.Enable(selfLog);
        try
        {
            using var host = await BuildAsync(Settings(sinkName, ("Serilog:WriteTo:1:Name", "NoSuchSink")));
            host.Services.GetRequiredService<SerilogILogger>().Information("Hello");
        }
        finally
        {
            global::Serilog.Debugging.SelfLog.Disable();
        }

        CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
        selfLog.ToString().ShouldContain("NoSuchSink");
    }

    /// <summary>
    ///     A <c>Serilog</c> section with a sink that writes to the <see cref="CapturingSink" /> of the given name, and any more settings. <c>Using</c> names
    ///     the assembly that declares the sink (and the enricher) rather than leaving Serilog.Settings.Configuration to find it.
    /// </summary>
    private static Dictionary<string, string?> Settings(string sinkName, params (string Key, string? Value)[] more)
    {
        var settings = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            ["Serilog:Using:0"] = typeof(CapturingSinkExtensions).Assembly.GetName().Name,
            ["Serilog:WriteTo:0:Name"] = nameof(CapturingSinkExtensions.Capturing),
            ["Serilog:WriteTo:0:Args:name"] = sinkName,
        };
        foreach (var (key, value) in more)
        {
            settings[key] = value;
        }

        return settings;
    }

    private static IEnumerable<Exception> Flatten(Exception exception)
    {
        for (var current = exception; current is { }; current = current.InnerException)
        {
            yield return current;
            if (current is AggregateException aggregate)
            {
                foreach (var inner in aggregate.InnerExceptions.SelectMany(Flatten))
                {
                    yield return inner;
                }
            }
        }
    }
}
