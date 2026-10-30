using Joinery.Dovetail.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     Runtime tests of the logger that applying the Serilog joints produces. Every test builds a real host through <c>ConfigureDovetail</c>
///     and looks at what a <see cref="CapturingSink" /> was given, rather than at the registrations.
/// </summary>
public class SerilogLoggerTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task A_Serilog_Joint_Produces_A_Logger_That_Writes_Events()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hello {Name}", "Dovetail");

        var logEvent = EventsOf(sinkName).ShouldHaveSingleItem();
        logEvent.Level.ShouldBe(LogEventLevel.Information);
        logEvent.RenderMessage().ShouldBe("Hello \"Dovetail\"");
        host.Services.GetRequiredService<Logger>().ShouldBeSameAs(logger);
    }

    [Test]
    public async Task Generic_Loggers_Resolve_Backed_By_Serilog()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<ILoggerFactory>().ShouldBeOfType<SerilogLoggerFactory>();
        host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogWarning("Hello {Name}", "Dovetail");

        var logEvent = EventsOf(sinkName).ShouldHaveSingleItem();
        logEvent.Level.ShouldBe(LogEventLevel.Warning);
        logEvent.RenderMessage().ShouldBe("Hello \"Dovetail\"");
        ( (ScalarValue)logEvent.Properties[Constants.SourceContextPropertyName] ).Value.ShouldBe(typeof(SerilogLoggerTests).FullName);
    }

    [Test]
    public async Task Loggers_From_The_Logger_Factory_Write_To_Serilog()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Some.Category").LogError("Broken");

        var logEvent = EventsOf(sinkName).ShouldHaveSingleItem();
        logEvent.Level.ShouldBe(LogEventLevel.Error);
        ( (ScalarValue)logEvent.Properties[Constants.SourceContextPropertyName] ).Value.ShouldBe("Some.Category");
    }

    [Test]
    public async Task Every_Serilog_Joint_Contributes_To_The_One_Logger()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(
            configure: rb => rb
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.Enrich.WithProperty("First", 1))
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.Enrich.WithProperty("Second", 2))
        );

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        var logEvent = EventsOf(sinkName).ShouldHaveSingleItem();
        logEvent.Properties.Keys.ShouldContain("First");
        logEvent.Properties.Keys.ShouldContain("Second");
    }

    [Test]
    public async Task Serilog_Joints_Can_Register_Services()
    {
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, services, _) => services.AddSingleton<RegisteredByAJoint>()));

        host.Services.GetService<RegisteredByAJoint>().ShouldNotBeNull();
    }

    [Test]
    public async Task The_Default_Logging_Providers_Are_Replaced()
    {
        var sinkName = NewSinkName();
        var builder = Host.CreateApplicationBuilder();
        ProviderTypes(builder.Services).ShouldContain("Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider", "the host registers its default providers before Dovetail");

        using var host = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        ProviderTypes(builder.Services).ShouldBeEmpty();
        host.Services.GetServices<ILoggerProvider>().ShouldBeEmpty();
    }

    [Test]
    public async Task The_Default_Logging_Providers_Are_Replaced_On_A_Web_Application()
    {
        var sinkName = NewSinkName();
        var builder = WebApplication.CreateBuilder();
        ProviderTypes(builder.Services).ShouldContain("Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider", "the host registers its default providers before Dovetail");

        await using var app = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        ProviderTypes(builder.Services).ShouldBeEmpty();
        app.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogInformation("Hello");
        EventsOf(sinkName).ShouldHaveSingleItem();
    }

    [Test]
    public async Task Events_Are_Written_Once_And_The_Logger_Factory_Is_Registered_Once()
    {
        var sinkName = NewSinkName();
        var builder = Host.CreateApplicationBuilder();
        using var host = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogInformation("Hello");
        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        EventsOf(sinkName).Count.ShouldBe(2, "each call writes one event, none are repeated by a second logging provider");
        builder.Services.Count(descriptor => descriptor.ServiceType == typeof(ILoggerFactory)).ShouldBe(1);
        builder.Services.Count(descriptor => descriptor.ServiceType == typeof(SerilogILogger)).ShouldBe(1);
        builder.Services.Count(descriptor => descriptor.ServiceType == typeof(Logger)).ShouldBe(1);
        builder.Services.Count(descriptor => descriptor.ServiceType == typeof(ILogger<>)).ShouldBe(1);
    }

    [Test]
    public async Task Disposing_The_Host_Disposes_The_Logger()
    {
        var sinkName = NewSinkName();
        var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");
        CapturingSink.Named(sinkName).IsDisposed.ShouldBeFalse();

        host.Dispose();

        CapturingSink.Named(sinkName).IsDisposed.ShouldBeTrue("the container owns the logger, so its sinks are flushed and disposed with the host");
    }

    [Test]
    public async Task Without_Any_Serilog_Joint_The_Logger_Still_Works()
    {
        using var host = await BuildAsync();

        var logger = host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>();

        Should.NotThrow(() => logger.LogInformation("Hello"));
        host.Services.GetRequiredService<ILoggerFactory>().ShouldBeOfType<SerilogLoggerFactory>();
    }

    private static IReadOnlyList<LogEvent> EventsOf(string sinkName) => CapturingSink.Named(sinkName).Events;

    private static string[] ProviderTypes(IServiceCollection services) =>
    [
        .. services
          .Where(descriptor => descriptor.ServiceType == typeof(ILoggerProvider))
          .Select(descriptor => descriptor.ImplementationType?.FullName ?? "<factory>"),
    ];

    public sealed class RegisteredByAJoint;
}
