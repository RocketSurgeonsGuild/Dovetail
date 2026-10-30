using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog;
using Serilog.Events;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     The options registered for <see cref="LoggerConfiguration" /> (<c>services.Configure</c>, <c>services.PostConfigure</c>) configure the logger too.
/// </summary>
public class SerilogOptionsTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Configure_Options_Are_Applied_To_The_Logger_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb.ConfigureServices((_, services) => services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.WriteTo.Capturing(sinkName)))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
    }

    [Test]
    public async Task PostConfigure_Options_Are_Applied_To_The_Logger_Configuration()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb.ConfigureServices((_, services) => services.PostConfigure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.WriteTo.Capturing(sinkName)))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
    }

    [Test]
    public async Task Options_Run_After_The_Joints_And_PostConfigure_Runs_After_Configure()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb
                            // The minimum level is last write wins: the level that ends up in effect is that of whatever ran last.
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.MinimumLevel.Verbose().WriteTo.Capturing(sinkName))
                            .ConfigureServices(
                                 (_, services) =>
                                 {
                                     // Registered in the opposite order to make sure that it is the kind of options that decides, not the order they were added in.
                                     services.PostConfigure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.MinimumLevel.Warning());
                                     services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.MinimumLevel.Debug());
                                 }
                             )
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Debug("Hidden by PostConfigure");
        logger.Information("Hidden by PostConfigure too");
        logger.Warning("Shown");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Configure_Options_Run_After_The_Joints()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.MinimumLevel.Verbose().WriteTo.Capturing(sinkName))
                            .ConfigureServices((_, services) => services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.MinimumLevel.Error()))
        );

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Warning("Hidden");
        logger.Error("Shown");

        CapturingSink.Named(sinkName).Events.Select(logEvent => logEvent.MessageTemplate.Text).ShouldBe(["Shown",]);
    }

    [Test]
    public async Task Options_Can_Use_Other_Services()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb.ConfigureServices(
                (_, services) =>
                {
                    services.AddSingleton(new ApplicationName("Dovetail.Serilog.Tests"));
                    services
                       .AddOptions<LoggerConfiguration>()
                       .Configure<ApplicationName>((loggerConfiguration, name) => loggerConfiguration.Enrich.WithProperty("Application", name.Value).WriteTo.Capturing(sinkName));
                }
            )
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        var logEvent = CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
        ( (ScalarValue)logEvent.Properties["Application"] ).Value.ShouldBe("Dovetail.Serilog.Tests");
    }

    [Test]
    public async Task Named_Options_Are_Not_Applied()
    {
        var sinkName = NewSinkName();
        var otherSinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb.ConfigureServices(
                (_, services) =>
                {
                    services.Configure<LoggerConfiguration>(loggerConfiguration => loggerConfiguration.WriteTo.Capturing(sinkName));
                    services.Configure<LoggerConfiguration>("other", loggerConfiguration => loggerConfiguration.WriteTo.Capturing(otherSinkName));
                }
            )
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        CapturingSink.Named(sinkName).Events.ShouldHaveSingleItem();
        CapturingSink.Named(otherSinkName).Events.ShouldBeEmpty();
    }

    public sealed record ApplicationName(string Value);
}
