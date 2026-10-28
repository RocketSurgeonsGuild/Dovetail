using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Hosting;
using Joinery.Dovetail.Joints;
using Hosting.Benchmarks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// One job per target framework, side by side, so the results show what .NET 11 (and runtime async) changes.
var config = ManualConfig
            .Create(DefaultConfig.Instance)
            .AddJob(Job.ShortRun.WithRuntime(CoreRuntime.Core10_0).WithId("net10.0").AsBaseline())
            .AddJob(
                 Job.ShortRun
                    .WithRuntime(Net11Runtime())
                    .WithToolchain(CsProjCoreToolchain.From(new NetCoreAppSettings("net11.0", null, ".NET 11.0")))
                    .WithId("net11.0")
             );

BenchmarkSwitcher.FromTypes([typeof(Benchmarks)]).Run(args, config);

// BenchmarkDotNet 0.15 has no .NET 11 moniker and throws on unrecognized ones, so describe .NET 11 on the net10.0
// moniker (used for its SDK version checks) and pick the net11.0 toolchain explicitly. Use CoreRuntime.Core11_0 once a
// stable BenchmarkDotNet has it.
static CoreRuntime Net11Runtime() =>
    (CoreRuntime)typeof(CoreRuntime)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeof(RuntimeMoniker), typeof(string), typeof(string)])!
                .Invoke([RuntimeMoniker.Net10_0, "net11.0", ".NET 11.0"]);

#pragma warning disable CA1822
#pragma warning disable CA1724
#pragma warning disable CA1707

namespace Hosting.Benchmarks
{
    /// <summary>
    ///     The benchmarks to run
    /// </summary>
    public class Benchmarks
    {
        [Benchmark]
        public async Task Default_Hosting()
        {
            using var host = Host
                            .CreateDefaultBuilder()
                            .ConfigureLogging(x => x.ClearProviders())
                            .Build();
            await host.StartAsync().ConfigureAwait(false);
            await host.StopAsync().ConfigureAwait(false);
        }

        [Benchmark(Baseline = true)]
        public async Task Default_Hosting_Application()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            var host = builder.Build();
            await host.StartAsync().ConfigureAwait(false);
            await host.StopAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async Task Rocket_Surgery_Hosting_Application()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            var host = await builder.ConfigureDovetail();
            await host.StartAsync().ConfigureAwait(false);
            await host.StopAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async Task Default_Hosting_With_Service()
        {
            using var host = Host
                            .CreateDefaultBuilder()
                            .ConfigureLogging(x => x.ClearProviders())
                            .ConfigureServices(x => x.AddHostedService<HostedService>())
                            .Build();
            await host.StartAsync().ConfigureAwait(false);
            await host.StopAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async Task Rocket_Surgery_Hosting_Application_With_Service()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services.AddHostedService<HostedService>();
            builder.Logging.ClearProviders();
            var host = await builder.ConfigureDovetail();
            await host.StartAsync().ConfigureAwait(false);
            await host.StopAsync().ConfigureAwait(false);
        }

        private class HostedService : IHostedService
        {
            public Task StartAsync(CancellationToken cancellationToken) => Task.Delay(TimeSpan.FromTicks(1), cancellationToken);

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        }
    }
}

[DovetailExport]
internal class Services : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(new Item("Test", 1));
}

[DovetailExport]
internal class Services2 : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(new Item("Test2", 1));
}

[DovetailExport]
internal class Configuration : IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfigurationBuilder builder) => builder.AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Test"] = "1"
    });
}

internal record Item(string Name, int Value);
