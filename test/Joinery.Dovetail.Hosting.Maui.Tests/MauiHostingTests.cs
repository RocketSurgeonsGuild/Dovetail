using System.Diagnostics;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
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

        app.Services.GetService<IFromHostApplicationJoint>().ShouldNotBeNull();
        app.Services.GetService<IFromMauiAppBuilderJoint>().ShouldNotBeNull();
        Observed.App.ShouldBeSameAs(app);
    }

    [Test]
    public async Task ConfigureDovetail_BuildsTheApp_AndRunsEveryJoint()
    {
        await using var app = await MauiApp.CreateBuilder(useDefaults: false).ConfigureDovetail();

        app.Services.GetService<IFromHostApplicationJoint>().ShouldNotBeNull();
        app.Services.GetService<IFromMauiAppBuilderJoint>().ShouldNotBeNull();
        Observed.App.ShouldBeSameAs(app);
    }

    [Test]
    public async Task ContextBuilderConfiguration_IsApplied()
    {
        await using var app = MauiApp
                             .CreateBuilder(useDefaults: false)
                             .UseDovetail(rb => rb.ConfigureServices((_, services) => services.AddSingleton(new Marker())));

        app.Services.GetService<Marker>().ShouldNotBeNull();
    }

    [Test]
    public async Task Configuration_IsLoadedFromTheEmbeddedFiles()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();
        var configuration = app.Services.GetRequiredService<IConfiguration>();

        configuration["Maui:Name"].ShouldBe("testing");
        configuration["Maui:Source"].ShouldBe("appsettings");
    }

    [Test]
    public async Task Context_CarriesTheApplicationNameAndPlatform()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        Observed.ApplicationName.ShouldBe(typeof(MauiHostingTests).Assembly.GetName().Name);
        Observed.Platform.ShouldBe(OperatingSystem.IsMacOS() ? "macOS" : OperatingSystem.IsWindows() ? "Windows" : "Linux");
    }

    [Test]
    public async Task EnvironmentName_DefaultsToTheBuildTimeValue()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", null);
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        Observed.EnvironmentName.ShouldBe("Testing");
        app.Services.GetRequiredService<IHostEnvironment>().EnvironmentName.ShouldBe("Testing");
    }

    [Test]
    public async Task EnvironmentName_PrefersDotnetEnvironment()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", "Staging");
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        Observed.EnvironmentName.ShouldBe("Staging");
        app.Services.GetRequiredService<IHostEnvironment>().EnvironmentName.ShouldBe("Staging");
    }

    [Test]
    public void EnvironmentName_FallsBackToProduction()
    {
        using var _ = new EnvironmentVariableScope("DOTNET_ENVIRONMENT", null);

        DovetailMauiHelpers.GetEnvironmentName(typeof(object).Assembly).ShouldBe(Environments.Production);
    }

    [Test]
    public async Task ContainerFactory_IsHonored()
    {
        await using var app = MauiApp.CreateBuilder(useDefaults: false).UseDovetail();

        app.Services.GetService<IStashboxContainer>().ShouldNotBeNull();
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

        RecordingProcessor.Ended.ShouldContain(name);
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
