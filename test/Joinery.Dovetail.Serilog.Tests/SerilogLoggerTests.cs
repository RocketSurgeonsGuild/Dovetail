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
public class SerilogLoggerTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task A_Serilog_Joint_Produces_A_Logger_That_Writes_Events()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        var logger = host.Services.GetRequiredService<SerilogILogger>();
        logger.Information("Hello {Name}", "Dovetail");

        var logEvent = await EventsOf(sinkName).Should().HaveSingleItem();
        await logEvent.Level.Should().BeEqualTo(LogEventLevel.Information);
        await logEvent.RenderMessage().Should().BeEqualTo("Hello \"Dovetail\"");
        await host.Services.GetRequiredService<Logger>().Should().BeSameReferenceAs(logger);
    }

    [Test]
    public async Task Generic_Loggers_Resolve_Backed_By_Serilog()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        await Assert.That(host.Services.GetRequiredService<ILoggerFactory>()).IsTypeOf<SerilogLoggerFactory>();
        host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogWarning("Hello {Name}", "Dovetail");

        var logEvent = await EventsOf(sinkName).Should().HaveSingleItem();
        await logEvent.Level.Should().BeEqualTo(LogEventLevel.Warning);
        await logEvent.RenderMessage().Should().BeEqualTo("Hello \"Dovetail\"");
        await ( (ScalarValue)logEvent.Properties[Constants.SourceContextPropertyName] ).Value.Should().BeEqualTo(typeof(SerilogLoggerTests).FullName);
    }

    [Test]
    public async Task Loggers_From_The_Logger_Factory_Write_To_Serilog()
    {
        var sinkName = NewSinkName();
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Some.Category").LogError("Broken");

        var logEvent = await EventsOf(sinkName).Should().HaveSingleItem();
        await logEvent.Level.Should().BeEqualTo(LogEventLevel.Error);
        await ( (ScalarValue)logEvent.Properties[Constants.SourceContextPropertyName] ).Value.Should().BeEqualTo("Some.Category");
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

        var logEvent = await EventsOf(sinkName).Should().HaveSingleItem();
        await logEvent.Properties.Keys.Should().Contain("First");
        await logEvent.Properties.Keys.Should().Contain("Second");
    }

    [Test]
    public async Task Serilog_Joints_Can_Register_Services()
    {
        using var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, services, _) => services.AddSingleton<RegisteredByAJoint>()));

        await host.Services.GetService<RegisteredByAJoint>().Should().NotBeNull();
    }

    [Test]
    public async Task The_Default_Logging_Providers_Are_Replaced()
    {
        var sinkName = NewSinkName();
        var builder = Host.CreateApplicationBuilder();
        await ProviderTypes(builder.Services).Should().Contain("Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider").Because("the host registers its default providers before Dovetail");

        using var host = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        await ProviderTypes(builder.Services).Should().BeEmpty();
        await host.Services.GetServices<ILoggerProvider>().Should().BeEmpty();
    }

    [Test]
    public async Task The_Default_Logging_Providers_Are_Replaced_On_A_Web_Application()
    {
        var sinkName = NewSinkName();
        var builder = WebApplication.CreateBuilder();
        await ProviderTypes(builder.Services).Should().Contain("Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider").Because("the host registers its default providers before Dovetail");

        await using var app = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        await ProviderTypes(builder.Services).Should().BeEmpty();
        app.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogInformation("Hello");
        await EventsOf(sinkName).Should().HaveSingleItem();
    }

    [Test]
    public async Task Events_Are_Written_Once_And_The_Logger_Factory_Is_Registered_Once()
    {
        var sinkName = NewSinkName();
        var builder = Host.CreateApplicationBuilder();
        using var host = await builder.ConfigureDovetail(rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>().LogInformation("Hello");
        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");

        await EventsOf(sinkName).Count.Should().BeEqualTo(2).Because("each call writes one event, none are repeated by a second logging provider");
        await builder.Services.Count(descriptor => descriptor.ServiceType == typeof(ILoggerFactory)).Should().BeEqualTo(1);
        await builder.Services.Count(descriptor => descriptor.ServiceType == typeof(SerilogILogger)).Should().BeEqualTo(1);
        await builder.Services.Count(descriptor => descriptor.ServiceType == typeof(Logger)).Should().BeEqualTo(1);
        await builder.Services.Count(descriptor => descriptor.ServiceType == typeof(ILogger<>)).Should().BeEqualTo(1);
    }

    [Test]
    public async Task Disposing_The_Host_Disposes_The_Logger()
    {
        var sinkName = NewSinkName();
        var host = await BuildAsync(configure: rb => rb.ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName)));

        host.Services.GetRequiredService<SerilogILogger>().Information("Hello");
        await CapturingSink.Named(sinkName).IsDisposed.Should().BeFalse();

        host.Dispose();

        await CapturingSink.Named(sinkName).IsDisposed.Should().BeTrue().Because("the container owns the logger, so its sinks are flushed and disposed with the host");
    }

    [Test]
    public async Task Without_Any_Serilog_Joint_The_Logger_Still_Works()
    {
        using var host = await BuildAsync();

        var logger = host.Services.GetRequiredService<ILogger<SerilogLoggerTests>>();

        await Assert.That(() => logger.LogInformation("Hello")).ThrowsNothing();
        await Assert.That(host.Services.GetRequiredService<ILoggerFactory>()).IsTypeOf<SerilogLoggerFactory>();
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
