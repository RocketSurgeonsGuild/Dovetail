using System.Collections.Concurrent;
using System.Diagnostics;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.OpenTelemetry.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Joinery.Dovetail.Hosting.Uno.Tests;

public static class UnoFixtures
{
    /// <summary>
    ///     The activity source <see cref="TracingJoint" /> subscribes the tracer provider to.
    /// </summary>
    public const string ActivitySourceName = "Joinery.Dovetail.Hosting.Uno.Tests";

    public sealed class FromApplicationBuilderJoint;

    public sealed class FromHostBuilderJoint;

    /// <summary>
    ///     A service the host registers before Dovetail's service joints run, standing in for Uno's own registrations.
    /// </summary>
    public sealed class RegisteredByTheHost;

    /// <summary>
    ///     What the exported joints saw on their most recent run, for assertions. Tests that read it don't run in parallel.
    /// </summary>
    public static class Observed
    {
        public static string? EnvironmentName { get; set; }
        public static bool ServiceJointSawHostRegistrations { get; set; }
        public static IHost? CreatedHost { get; set; }
    }

    [DovetailExport]
    public class ApplicationBuilderJoint : IHostApplicationJoint<IApplicationBuilder>
    {
        public void Register(IDovetailContext context, IApplicationBuilder builder) =>
            builder.Configure(host => host.ConfigureServices(services => services.AddSingleton<FromApplicationBuilderJoint>()));
    }

    [DovetailExport]
    public class HostBuilderJoint : IHostApplicationJoint<IHostBuilder>
    {
        public void Register(IDovetailContext context, IHostBuilder builder) =>
            builder.ConfigureServices(services => services.AddSingleton<FromHostBuilderJoint>());
    }

    [DovetailExport]
    public class ServiceJoint : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services)
        {
            Observed.EnvironmentName = context.Get<string>("EnvironmentName");
            Observed.ServiceJointSawHostRegistrations = services.Any(descriptor => descriptor.ServiceType == typeof(RegisteredByTheHost));
        }
    }

    [DovetailExport]
    public class HostCreatedJoint : IHostCreatedJoint<IHost>
    {
        public void Register(IDovetailContext context, IHost host) => Observed.CreatedHost = host;
    }

    /// <summary>
    ///     Subscribes the tracer provider to <see cref="ActivitySourceName" /> and records the activities it ends. Only a running
    ///     OpenTelemetry tracer provider records them; TUnit's own listener makes every activity source listened to anyway.
    /// </summary>
    [DovetailExport]
    public class TracingJoint : ITracerProviderJoint
    {
        public void Register(IDovetailContext context, TracerProviderBuilder builder) =>
            builder.AddSource(ActivitySourceName).AddProcessor(new RecordingProcessor());
    }

    public sealed class RecordingProcessor : BaseProcessor<Activity>
    {
        public static ConcurrentQueue<string> Ended { get; } = new();

        public override void OnEnd(Activity data) => Ended.Enqueue(data.DisplayName);
    }
}
