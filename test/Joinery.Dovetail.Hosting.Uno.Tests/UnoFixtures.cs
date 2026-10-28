using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.Uno.Tests;

public static class UnoFixtures
{
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
}
