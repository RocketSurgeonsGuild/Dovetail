using System.Diagnostics;
using System.Diagnostics.Metrics;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Stashbox;
using static Joinery.Dovetail.Hosting.Maui.Tests.MauiFixtures;

namespace Joinery.Dovetail.Hosting.Maui.Tests;

/// <summary>
///     Runs the MAUI host on the plain target frameworks (no MAUI workload), with the same <c>DovetailHost</c> item the SDK's MAUI
///     detection adds and a build-time environment name of <c>Testing</c> (see the project file). The tests share
///     <see cref="Observed" /> and the process environment, so they don't run in parallel.
/// </summary>
[NotInParallel]
public class MauiHostingTests
{
    [Test]
    public async Task UseDovetail_BuildsTheApp_AndRunsEveryJoint()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        await app.Services.GetService<IFromHostApplicationJoint>().Should().NotBeNull();
        await app.Services.GetService<IFromMauiAppBuilderJoint>().Should().NotBeNull();
        await Observed.App.Should().BeSameReferenceAs(app);
    }

    [Test]
    public async Task ConfigureDovetail_BuildsTheApp_AndRunsEveryJoint()
    {
        await using var app = await MauiApp.CreateBuilder(useDefaults: false).ConfigureDovetail();

        await app.Services.GetService<IFromHostApplicationJoint>().Should().NotBeNull();
        await app.Services.GetService<IFromMauiAppBuilderJoint>().Should().NotBeNull();
        await Observed.App.Should().BeSameReferenceAs(app);
    }

    [Test]
    public async Task ContextBuilderConfiguration_IsApplied()
    {
        await using var app = MauiApp
                             .CreateBuilder(useDefaults: false)
                             .UseDovetail(rb => rb.ConfigureServices((_, services) => services.AddSingleton(new Marker())));

        await app.Services.GetService<Marker>().Should().NotBeNull();
    }

    [Test]
    public async Task Configuration_IsLoadedFromTheEmbeddedFiles()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();
        var configuration = app.Services.GetRequiredService<IConfiguration>();

        await configuration["Maui:Name"].Should().BeEqualTo("testing");
        await configuration["Maui:Source"].Should().BeEqualTo("appsettings");
    }

    [Test]
    public async Task Context_CarriesTheApplicationNameAndPlatform()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        await Observed.ApplicationName.Should().BeEqualTo(typeof(MauiHostingTests).Assembly.GetName().Name);
        await Observed.Platform.Should().BeEqualTo(OperatingSystem.IsMacOS() ? "macOS" : OperatingSystem.IsWindows() ? "Windows" : "Linux");
    }

    [Test]
    public async Task EnvironmentName_DefaultsToTheBuildTimeValue()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", null);
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        await Observed.EnvironmentName.Should().BeEqualTo("Testing");
        await app.Services.GetRequiredService<IHostEnvironment>().EnvironmentName.Should().BeEqualTo("Testing");
    }

    [Test]
    public async Task EnvironmentName_PrefersDotnetEnvironment()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", "Staging");
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        await Observed.EnvironmentName.Should().BeEqualTo("Staging");
        await app.Services.GetRequiredService<IHostEnvironment>().EnvironmentName.Should().BeEqualTo("Staging");
    }

    [Test]
    public async Task EnvironmentName_FallsBackToProduction()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", null);

        await DovetailMauiHelpers.GetEnvironmentName(typeof(object).Assembly).Should().BeEqualTo(Environments.Production);
    }

    [Test]
    public async Task ContainerFactory_IsHonored()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        await app.Services.GetService<IStashboxContainer>().Should().NotBeNull();
    }

    /// <summary>
    ///     MAUI doesn't run hosted services, so OpenTelemetry.Extensions.Hosting couldn't start OpenTelemetry here. Joinery.Dovetail.Hosting
    ///     adds it to the services with the OpenTelemetry joints (<see cref="TracingJoint" />) and starts the providers once the app is
    ///     built, so the app's activity source is listened to without anything resolving <see cref="TracerProvider" />.
    /// </summary>
    [Test]
    public async Task OpenTelemetry_IsStarted_WhenTheAppIsBuilt()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        var name = Guid.NewGuid().ToString("N");
        using (var source = new ActivitySource(ActivitySourceName))
        using (source.StartActivity(name)) { }

        await RecordingProcessor.Ended.Should().Contain(name);
    }

    /// <summary>
    ///     Dovetail starts the meter provider once the app is built too, so a measurement the app records before anything resolves the
    ///     provider is still collected by <see cref="MetricsJoint" />'s exporter.
    /// </summary>
    [Test]
    public async Task OpenTelemetry_CollectsMetrics_FromWhenTheAppIsBuilt()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        var name = "dovetail.test.counter." + Guid.NewGuid().ToString("N");
        using var meter = new Meter(ActivitySourceName);
        meter.CreateCounter<long>(name).Add(3);
        app.Services.GetRequiredService<MeterProvider>().ForceFlush();

        await ExportedMetrics.Should().Contain(metric => metric.Name == name && metric.MetricPoints.Sum(point => point.GetSumLong()) == 3);
    }

    /// <summary>
    ///     <see cref="ResourceJoint" />'s attribute is on the resource of every signal's provider.
    /// </summary>
    [Test]
    public async Task OpenTelemetry_AppliesTheResourceJoints_ToEveryProvider()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        var attribute = new KeyValuePair<string, object>(ResourceAttribute, "maui");
        await app.Services.GetRequiredService<TracerProvider>().GetResource().Attributes.Should().Contain(attribute);
        await app.Services.GetRequiredService<MeterProvider>().GetResource().Attributes.Should().Contain(attribute);
    }

    private sealed class Marker;

    private sealed class EnvironmentVariableScope : IDisposable
    {
        private readonly string _name;
        private readonly string? _previous;

        public EnvironmentVariableScope(string name, string? value)
        {
            _name = name;
            _previous = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose() => Environment.SetEnvironmentVariable(_name, _previous);
    }
}
