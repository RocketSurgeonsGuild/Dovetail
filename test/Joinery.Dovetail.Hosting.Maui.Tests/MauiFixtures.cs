using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using OpenTelemetry.Trace;

namespace Joinery.Dovetail.Hosting.Maui.Tests;

public static class MauiFixtures
{
    /// <summary>
    ///     The activity source <see cref="TracingJoint" /> subscribes the tracer provider to.
    /// </summary>
    public const string ActivitySourceName = "Joinery.Dovetail.Hosting.Maui.Tests";

    public interface IFromHostApplicationJoint;

    public interface IFromMauiAppBuilderJoint;

    public sealed class FromHostApplicationJoint : IFromHostApplicationJoint;

    public sealed class FromMauiAppBuilderJoint : IFromMauiAppBuilderJoint;

    /// <summary>
    ///     What the exported joints saw on their most recent run, for assertions. Tests that read it don't run in parallel.
    /// </summary>
    public static class Observed
    {
        public static MauiApp? App { get; set; }
        public static string? ApplicationName { get; set; }
        public static string? EnvironmentName { get; set; }
        public static string? Platform { get; set; }
    }

    /// <summary>
    ///     A joint for any <see cref="IHostApplicationBuilder" />, which runs for MAUI too since <see cref="MauiAppBuilder" /> is one.
    /// </summary>
    [DovetailExport]
    public class HostApplicationJoint : IHostApplicationJoint<IHostApplicationBuilder>
    {
        public void Register(IDovetailContext context, IHostApplicationBuilder builder) =>
            builder.Services.AddSingleton<IFromHostApplicationJoint, FromHostApplicationJoint>();
    }

    [DovetailExport]
    public class MauiAppBuilderJoint : IHostApplicationJoint<MauiAppBuilder>
    {
        public void Register(IDovetailContext context, MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IFromMauiAppBuilderJoint, FromMauiAppBuilderJoint>();
            Observed.ApplicationName = context.Get<string>("ApplicationName");
            Observed.EnvironmentName = context.Get<string>("EnvironmentName");
            Observed.Platform = context.Get<string>("Platform");
        }
    }

    [DovetailExport]
    public class MauiAppJoint : IHostCreatedJoint<MauiApp>
    {
        public void Register(IDovetailContext context, MauiApp app) => Observed.App = app;
    }

    [DovetailExport]
    public class TracingJoint : ITracerProviderJoint
    {
        public void Register(IDovetailContext context, TracerProviderBuilder builder) => builder.AddSource(ActivitySourceName);
    }
}
