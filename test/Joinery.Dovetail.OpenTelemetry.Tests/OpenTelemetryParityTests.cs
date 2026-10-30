using System.Diagnostics;
using System.Diagnostics.Metrics;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static Joinery.Dovetail.OpenTelemetry.Tests.OpenTelemetryFixtures;

namespace Joinery.Dovetail.OpenTelemetry.Tests;

/// <summary>
///     Evidence that Dovetail's OpenTelemetry joints cover what LaunchPad.Telemetry offered (docs: concepts/opentelemetry-launchpad-parity).
///     Each test names the LaunchPad abstraction it stands in for, and runs the joint the way an application does, on a generic host, and
///     observes it through the in-memory exporters.
/// </summary>
public class OpenTelemetryParityTests
{
    private const string TelemetrySdkName = "telemetry.sdk.name";
    private const string TelemetrySdkLanguage = "telemetry.sdk.language";
    private const string TelemetrySdkVersion = "telemetry.sdk.version";
    private const string ServiceName = "service.name";

    // IOpenTelemetryConvention / OpenTelemetryConvention -> IOpenTelemetryJoint
    [Test]
    public async Task Base_Joint_Configures_Services_And_Every_Signal()
    {
        const string name = "Joinery.Dovetail.OpenTelemetry.Tests.Base";
        List<Activity> exportedActivities = [];
        List<MetricSnapshot> exportedMetrics = [];
        List<LogRecord> exportedLogs = [];
        var marker = new BaseJointMarker();

        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb.ConfigureOpenTelemetry(
                                       (_, builder) =>
                                       {
                                           builder.Services.AddSingleton(marker);
                                           builder
                                              .WithTracing(tracing => tracing.AddSource(name).AddInMemoryExporter(exportedActivities))
                                              .WithMetrics(metrics => metrics.AddMeter(name).AddInMemoryExporter(exportedMetrics))
                                              .WithLogging(logging => logging.AddInMemoryExporter(exportedLogs));
                                       }
                                   )
                               );

        host.Services.GetRequiredService<BaseJointMarker>().ShouldBeSameAs(marker);

        // The providers listen to activity sources and meters from when they are first resolved.
        var tracerProvider = host.Services.GetRequiredService<TracerProvider>();
        var meterProvider = host.Services.GetRequiredService<MeterProvider>();
        var loggerProvider = host.Services.GetRequiredService<LoggerProvider>();

        using (var source = new ActivitySource(name))
        using (source.StartActivity("base-span")) { }

        using var meter = new Meter(name);
        meter.CreateCounter<long>("base.counter").Add(1);
        host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(name).LogInformation("base-log");

        tracerProvider.ForceFlush();
        meterProvider.ForceFlush();
        loggerProvider.ForceFlush();

        exportedActivities.ShouldContain(activity => activity.DisplayName == "base-span");
        exportedMetrics.ShouldContain(metric => metric.Name == "base.counter");
        exportedLogs.ShouldContain(record => record.Body == "base-log");
    }

    // IOpenTelemetryAsyncConvention / OpenTelemetryAsyncConvention -> IOpenTelemetryAsyncJoint / OpenTelemetryAsyncJointDelegate
    [Test]
    public async Task Async_Delegate_Is_Awaited_With_The_Cancellation_Token_Given_To_ConfigureDovetail()
    {
        const string name = "Joinery.Dovetail.OpenTelemetry.Tests.Async";
        List<Activity> exportedActivities = [];
        using var cancellation = new CancellationTokenSource();
        CancellationToken received = default;

        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb.ConfigureOpenTelemetry(
                                       async (_, builder, cancellationToken) =>
                                       {
                                           received = cancellationToken;
                                           // Whatever the delegate does after it awaits has to be in place before the host is built.
                                           await Task.Delay(10, CancellationToken.None);
                                           builder.WithTracing(tracing => tracing.AddSource(name).AddInMemoryExporter(exportedActivities));
                                       }
                                   ),
                                   cancellation.Token
                               );

        received.ShouldBe(cancellation.Token);

        var tracerProvider = host.Services.GetRequiredService<TracerProvider>();
        using (var source = new ActivitySource(name))
        using (source.StartActivity("async-span")) { }

        tracerProvider.ForceFlush();

        exportedActivities.ShouldContain(activity => activity.DisplayName == "async-span");
    }

    // [ExportConvention] class : IOpenTelemetryAsyncConvention -> [DovetailExport] class : IOpenTelemetryAsyncJoint
    [Test]
    public async Task Exported_Async_Joint_Is_Discovered_And_Awaited()
    {
        var telemetry = new ExportedTelemetry();
        using var cancellation = new CancellationTokenSource();

        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.Set(telemetry), cancellation.Token);

        telemetry.AsyncCancellationToken.ShouldBe(cancellation.Token);
        host.Services.GetService<TracerProvider>().ShouldNotBeNull("the exported async joint should have turned tracing on");
    }

    // IOpenTelemetryMetricsConvention -> IMeterProviderBuilderJoint
    [Test]
    public async Task Exported_Meter_Joint_Is_Discovered_And_Applied()
    {
        var telemetry = new ExportedTelemetry();

        using var host = await Host.CreateApplicationBuilder().ConfigureDovetail(rb => rb.Set(telemetry));

        var meterProvider = host.Services.GetRequiredService<MeterProvider>();
        using var meter = new Meter(ExportedSourceName);
        meter.CreateCounter<long>("exported.counter").Add(2);
        meterProvider.ForceFlush();

        telemetry.Metrics.ShouldContain(metric => metric.Name == "exported.counter");
    }

    // IOpenTelemetryLoggingConvention -> ILoggerProviderBuilderJoint
    [Test]
    public async Task Exported_Logger_Joint_Is_Discovered_And_Applied()
    {
        var telemetry = new ExportedTelemetry();

        using var host = await Host.CreateApplicationBuilder().ConfigureDovetail(rb => rb.Set(telemetry));

        host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(ExportedSourceName).LogInformation("exported-log");
        host.Services.GetRequiredService<LoggerProvider>().ForceFlush();

        telemetry.Logs.ShouldContain(record => record.Body == "exported-log");
    }

    // IOpenTelemetryResourceConvention -> IResourceBuilderJoint
    [Test]
    public async Task Exported_Resource_Joint_Is_Discovered_And_Applied_To_Every_Provider()
    {
        var telemetry = new ExportedTelemetry();
        var attribute = new KeyValuePair<string, object>(ExportedResourceAttribute, "exported");

        using var host = await Host.CreateApplicationBuilder().ConfigureDovetail(rb => rb.Set(telemetry));

        // The exported meter, logger and async joints between them turn all three signals on.
        host.Services.GetRequiredService<TracerProvider>().GetResource().Attributes.ShouldContain(attribute);
        host.Services.GetRequiredService<MeterProvider>().GetResource().Attributes.ShouldContain(attribute);
        host.Services.GetRequiredService<LoggerProvider>().GetResource().Attributes.ShouldContain(attribute);
    }

    // The IConfiguration parameter of every LaunchPad convention -> IDovetailContext.Configuration
    [Test]
    public async Task Joints_Read_The_Application_Configuration_From_The_Context()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["Telemetry:Environment"] = "from-configuration", });

        using var host = await builder.ConfigureDovetail(
            rb => rb
                 .ConfigureResourceBuilder((context, resource) => resource.AddAttributes([new KeyValuePair<string, object>("deployment.environment.name", context.Configuration["Telemetry:Environment"]!)]))
                 .ConfigureTracerProvider((_, tracing) => tracing.AddSource("Joinery.Dovetail.OpenTelemetry.Tests.Configuration"))
        );

        host.Services.GetRequiredService<TracerProvider>().GetResource().Attributes.ShouldContain(new KeyValuePair<string, object>("deployment.environment.name", "from-configuration"));
    }

    // TelemetryConvention (ILoggingConvention: ILoggingBuilder.AddOpenTelemetry()) -> WithLogging(), which every logger joint calls
    [Test]
    public async Task Logs_Are_Exported_Through_The_Logging_Provider()
    {
        List<LogRecord> exportedLogs = [];

        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureLoggerProvider((_, logging) => logging.AddInMemoryExporter(exportedLogs)));

        // The OpenTelemetry provider is an ordinary ILoggerProvider of the host's logging, as ILoggingBuilder.AddOpenTelemetry() added it.
        host.Services.GetServices<ILoggerProvider>().ShouldContain(provider => provider is OpenTelemetryLoggerProvider);

        // ILogger<T> writes through ILoggerFactory, so the application's own loggers reach the exporter: category, level and structured state.
        var logger = host.Services.GetRequiredService<ILogger<OpenTelemetryParityTests>>();
        logger.LogWarning("parity {Name}", "log");
        host.Services.GetRequiredService<LoggerProvider>().ForceFlush();

        var record = exportedLogs.ShouldHaveSingleItem();
        record.CategoryName.ShouldBe(typeof(OpenTelemetryParityTests).FullName);
        record.LogLevel.ShouldBe(LogLevel.Warning);
        record.Body.ShouldBe("parity {Name}");
        record.Attributes.ShouldNotBeNull().ShouldContain(attribute => attribute.Key == "Name" && Equals(attribute.Value, "log"));
    }

    // DefaultTelemetryConvention: ConfigureResource(r => r.AddTelemetrySdk().AddEnvironmentVariableDetector())
    [Test]
    public async Task Default_Resource_Has_The_Telemetry_Sdk_Attributes_On_Every_Signal()
    {
        using var host = await CreateHostWithEverySignalAsync();

        foreach (var provider in Providers(host))
        {
            var attributes = provider.GetResource().Attributes.ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

            attributes.ShouldContainKeyAndValue(TelemetrySdkName, "opentelemetry");
            attributes.ShouldContainKeyAndValue(TelemetrySdkLanguage, "dotnet");
            attributes[TelemetrySdkVersion].ShouldBeOfType<string>().ShouldNotBeNullOrWhiteSpace();
            attributes[ServiceName].ShouldBeOfType<string>().ShouldStartWith("unknown_service");
        }
    }

    [Test]
    public async Task Default_Resource_Has_The_Environment_Variable_Detector()
    {
        var builder = Host.CreateApplicationBuilder();
        // The detector reads the host's configuration (environment variables are one of its sources), so one test with in-memory values
        // covers OTEL_SERVICE_NAME without changing the process's environment, which Default_Resource_Reads_The_Process_Environment does.
        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["OTEL_SERVICE_NAME"] = "from-configuration",
                ["OTEL_RESOURCE_ATTRIBUTES"] = "test.detected=from-configuration,deployment.environment.name=detector",
            }
        );

        using var host = await CreateHostWithEverySignalAsync(builder);

        foreach (var provider in Providers(host))
        {
            var attributes = provider.GetResource().Attributes.ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

            attributes.ShouldContainKeyAndValue(ServiceName, "from-configuration");
            attributes.ShouldContainKeyAndValue("test.detected", "from-configuration");
            attributes.ShouldContainKeyAndValue("deployment.environment.name", "detector");
        }
    }

    [Test]
    [NotInParallel(EnvironmentVariableKey)]
    public async Task Default_Resource_Reads_The_Process_Environment()
    {
        var previous = Environment.GetEnvironmentVariable(EnvironmentVariableKey);
        Environment.SetEnvironmentVariable(EnvironmentVariableKey, "test.detected=from-environment");
        try
        {
            using var host = await CreateHostWithEverySignalAsync(Host.CreateApplicationBuilder());

            foreach (var provider in Providers(host))
            {
                provider.GetResource().Attributes.ShouldContain(new KeyValuePair<string, object>("test.detected", "from-environment"));
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableKey, previous);
        }
    }

    /// <summary>
    ///     LaunchPad's DefaultTelemetryConvention adds the telemetry SDK attributes and the environment variable detector. Adding them
    ///     again through a resource joint changes nothing, so Dovetail doesn't need a joint of its own for them.
    /// </summary>
    [Test]
    [NotInParallel(EnvironmentVariableKey)]
    public async Task LaunchPads_Default_Resource_Convention_Changes_Nothing()
    {
        var previous = Environment.GetEnvironmentVariable(EnvironmentVariableKey);
        Environment.SetEnvironmentVariable(EnvironmentVariableKey, "test.detected=from-environment");
        try
        {
            using var sdkDefault = await CreateHostWithEverySignalAsync(Host.CreateApplicationBuilder());
            using var withLaunchPadDefault = await CreateHostWithEverySignalAsync(
                Host.CreateApplicationBuilder(),
                rb => rb.ConfigureResourceBuilder((_, resource) => resource.AddTelemetrySdk().AddEnvironmentVariableDetector())
            );

            var expected = Providers(sdkDefault).Select(provider => Describe(provider.GetResource())).ToArray();
            var actual = Providers(withLaunchPadDefault).Select(provider => Describe(provider.GetResource())).ToArray();

            actual.ShouldBe(expected);
        }
        finally
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableKey, previous);
        }
    }

    private const string EnvironmentVariableKey = "OTEL_RESOURCE_ATTRIBUTES";

    private static async Task<IHost> CreateHostWithEverySignalAsync(HostApplicationBuilder? builder = null, Action<DovetailContextBuilder>? configure = null) =>
        await ( builder ?? Host.CreateApplicationBuilder() ).ConfigureDovetail(
            rb =>
            {
                rb.ConfigureOpenTelemetry((_, otel) => otel.WithTracing().WithMetrics().WithLogging());
                configure?.Invoke(rb);
            }
        );

    private static BaseProvider[] Providers(IHost host) =>
    [
        host.Services.GetRequiredService<TracerProvider>(),
        host.Services.GetRequiredService<MeterProvider>(),
        host.Services.GetRequiredService<LoggerProvider>(),
    ];

    private static string Describe(Resource resource) =>
        string.Join(';', resource.Attributes.OrderBy(attribute => attribute.Key, StringComparer.Ordinal).Select(attribute => $"{attribute.Key}={attribute.Value}"));

    private sealed class BaseJointMarker;
}
