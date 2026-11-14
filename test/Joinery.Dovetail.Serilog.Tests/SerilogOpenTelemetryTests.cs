using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Extensions.Logging;
using static Joinery.Dovetail.Serilog.Tests.SerilogTestHost;
using SerilogILogger = Serilog.ILogger;

#pragma warning disable CA1848, CA2254, CA2000

namespace Joinery.Dovetail.Serilog.Tests;

/// <summary>
///     OpenTelemetry exports logs through its logging provider, which the logger factory would have written to. With Serilog's logger factory it
///     still does, because the Serilog logger forwards what it writes to the providers in the container, and the Serilog sinks get the events
///     as well.
/// </summary>
public class SerilogOpenTelemetryTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task The_OpenTelemetry_Logging_Provider_Exports_The_Records_Written_Through_A_Generic_Logger()
    {
        var sinkName = NewSinkName();
        List<LogRecord> exported = [];
        using var host = await BuildAsync(
            configure: rb => rb
                            .ConfigureSerilog((_, _, loggerConfiguration) => loggerConfiguration.WriteTo.Capturing(sinkName))
                            .ConfigureLoggerProvider((_, builder) => builder.AddInMemoryExporter(exported))
        );

        await Assert.That(host.Services.GetRequiredService<ILoggerFactory>()).IsTypeOf<SerilogLoggerFactory>();
        host.Services.GetRequiredService<ILogger<SerilogOpenTelemetryTests>>().LogWarning("Hello {Name}", "Dovetail");
        host.Services.GetRequiredService<LoggerProvider>().ForceFlush();

        var record = await exported.Should().HaveSingleItem().Because("the record is exported once");
        await record.Body.Should().BeEqualTo("Hello {Name}");
        await record.LogLevel.Should().BeEqualTo(LogLevel.Warning);
        await record.CategoryName.Should().BeEqualTo(typeof(SerilogOpenTelemetryTests).FullName);
        await CapturingSink.Named(sinkName).Events.Should().HaveSingleItem().Because("and written to Serilog's sink once");
    }

    [Test]
    public async Task The_OpenTelemetry_Logging_Provider_Exports_The_Events_Written_Through_Serilog()
    {
        List<LogRecord> exported = [];
        using var host = await BuildAsync(configure: rb => rb.ConfigureLoggerProvider((_, builder) => builder.AddInMemoryExporter(exported)));

        host.Services.GetRequiredService<SerilogILogger>().ForContext<SerilogOpenTelemetryTests>().Error(new InvalidOperationException("Broken"), "It broke");
        host.Services.GetRequiredService<LoggerProvider>().ForceFlush();

        var record = await exported.Should().HaveSingleItem();
        await record.Body.Should().BeEqualTo("It broke");
        await record.LogLevel.Should().BeEqualTo(LogLevel.Error);
        await ( await Assert.That(record.Exception).IsTypeOf<InvalidOperationException>() ).Message.Should().BeEqualTo("Broken");
    }

    [Test]
    public async Task The_OpenTelemetry_Logging_Provider_Is_The_Only_Provider_Left_Beside_Serilog()
    {
        List<LogRecord> exported = [];
        using var host = await BuildAsync(configure: rb => rb.ConfigureLoggerProvider((_, builder) => builder.AddInMemoryExporter(exported)));

        await host.Services.GetServices<ILoggerProvider>().Select(provider => provider.GetType().Name).Should().BeEquivalentTo(["OpenTelemetryLoggerProvider",], CollectionOrdering.Matching);
    }
}
