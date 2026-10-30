using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog;
using Serilog.Core;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     The <c>Serilog:MinimumLevel</c> section of the application's configuration controls the minimum level of the logger and the level of
///     each source. Sinks and enrichers are not read from configuration (that takes reflection): the Serilog joints configure them.
/// </summary>
public class SerilogConfigurationTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task The_Default_Minimum_Level_And_Its_Overrides_Are_Read_From_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", ["Serilog:MinimumLevel:Override:Noisy"] = "Error", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hidden by the default level");
        logger.Warning("Shown");
        var noisy = logger.ForContext(Constants.SourceContextPropertyName, "Noisy.Component");
        noisy.Warning("Hidden by the override");
        noisy.Error("Shown from the override");

        MessagesOf(sinkName).ShouldBe(["Shown", "Shown from the override",]);
    }

    [Test]
    public async Task An_Override_Can_Be_Lower_Than_The_Default()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Error", ["Serilog:MinimumLevel:Override:Chatty"] = "Debug", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Warning("Hidden");
        logger.ForContext(Constants.SourceContextPropertyName, "Chatty.Component").Debug("Shown");

        MessagesOf(sinkName).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task The_Minimum_Level_Can_Be_Given_As_The_Value_Of_The_Section()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel"] = "Warning", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hidden");
        logger.Warning("Shown");

        MessagesOf(sinkName).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Levels_Are_Matched_Without_Regard_To_Casing()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["serilog:minimumlevel:default"] = "warning", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hidden");
        logger.Warning("Shown");

        MessagesOf(sinkName).ShouldBe(["Shown",]);
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

        MessagesOf(sinkName).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Options_Run_After_The_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", },
            rb => rb
                 .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                 .ConfigureServices((_, services) => services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.MinimumLevel.Error()))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Warning("Hidden by the options");
        logger.Error("Shown");

        MessagesOf(sinkName).ShouldBe(["Shown",]);
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

        MessagesOf(sinkName).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Sinks_And_Enrichers_In_Configuration_Are_Not_Read()
    {
        var sinkName = NewSinkName();
        var configuredSinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?>
            {
                ["Serilog:Using:0"] = typeof(CapturingSinkExtensions).Assembly.GetName().Name,
                ["Serilog:WriteTo:0:Name"] = nameof(CapturingSinkExtensions.Capturing),
                ["Serilog:WriteTo:0:Args:name"] = configuredSinkName,
                ["Serilog:Properties:FromConfiguration"] = "yes",
            },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem().Properties.Keys.ShouldNotContain("FromConfiguration");
        CapturingSink.Named(configuredSinkName).Events.ShouldBeEmpty();
    }

    [Test]
    public async Task The_Levels_Follow_The_Configuration_When_It_Reloads()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", ["Serilog:MinimumLevel:Override:Noisy"] = "Error", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );
        var logger = host.Services.GetRequiredService<SerilogILogger>();
        var noisy = logger.ForContext(Constants.SourceContextPropertyName, "Noisy.Component");

        logger.Debug("Hidden before the change");
        noisy.Warning("Hidden before the change");
        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();
        configuration["Serilog:MinimumLevel:Default"] = "Debug";
        configuration["Serilog:MinimumLevel:Override:Noisy"] = "Warning";
        configuration.Reload();
        logger.Debug("Shown after the change");
        noisy.Warning("Shown after the change too");

        MessagesOf(sinkName).ShouldBe(["Shown after the change", "Shown after the change too",]);
    }

    [Test]
    public async Task A_Level_That_Is_Invalid_After_A_Reload_Leaves_The_Level_As_It_Was()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            new Dictionary<string, string?> { ["Serilog:MinimumLevel:Default"] = "Warning", },
            rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
        );
        var logger = host.Services.GetRequiredService<SerilogILogger>();

        var configuration = (IConfigurationRoot)host.Services.GetRequiredService<IConfiguration>();
        configuration["Serilog:MinimumLevel:Default"] = "Loud";
        Should.NotThrow(configuration.Reload);
        logger.Information("Hidden");
        logger.Warning("Shown");

        MessagesOf(sinkName).ShouldBe(["Shown",]);
    }

    [Test]
    [Arguments("Serilog:MinimumLevel:Default", "Loud")]
    [Arguments("Serilog:MinimumLevel:Default", "Off")]
    [Arguments("Serilog:MinimumLevel:Default", "2")]
    [Arguments("Serilog:MinimumLevel:Default", "")]
    [Arguments("Serilog:MinimumLevel", "Loud")]
    [Arguments("Serilog:MinimumLevel:Override:Noisy", "$noisySwitch")]
    public async Task A_Level_That_Is_Not_The_Name_Of_A_Serilog_Level_Fails_The_Build(string key, string value)
    {
        var exception = await Should.ThrowAsync<Exception>(async () => { using var host = await BuildAsync(new Dictionary<string, string?> { [key] = value, }); });

        Flatten(exception).ShouldContain(inner => inner.Message.Contains(key, StringComparison.Ordinal) && inner.Message.Contains("not a Serilog level", StringComparison.Ordinal));
    }

    private static IReadOnlyList<string> MessagesOf(string sinkName) => [.. CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text)];

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
