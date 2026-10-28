using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace Joinery.Dovetail.Hosting.Maui.Tests;

public static class MauiFixtures
{
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

    [DovetailExport]
    public class HostApplicationJoint : IHostApplicationJoint<MauiAppBuilder>
    {
        public void Register(IDovetailContext context, MauiAppBuilder builder) =>
            builder.Services.AddSingleton<IFromHostApplicationJoint, FromHostApplicationJoint>();
    }

    [DovetailExport]
    public class MauiAppBuilderJoint : IMauiAppBuilderJoint
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
    public class MauiAppJoint : IMauiAppJoint
    {
        public void Register(IDovetailContext context, MauiApp app) => Observed.App = app;
    }
}
