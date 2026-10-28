using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.Uno;

/// <summary>
///     Wires a Dovetail context into the old-style <see cref="IHostBuilder" /> that Uno.Extensions builds inside
///     <c>IApplicationBuilder.Build()</c>.
/// </summary>
/// <remarks>
///     Uno only settles its environment (including the app's own <c>UseEnvironment</c> call) while it builds the host, so
///     configuration and services are applied from inside its callbacks, where the environment is known. Those callbacks are
///     synchronous: Dovetail's joints are waited on there, which only blocks when a joint completes asynchronously (synchronous
///     and embedded configuration joints never do), and every await in Dovetail resumes off the UI thread.
/// </remarks>
internal static class UnoHostBuilderBridge
{
    public static void Configure(IHostBuilder host, IDovetailContext context, CancellationToken cancellationToken)
    {
        Wait(host.ApplyUnoHostBuilder(context, cancellationToken));

        host.ConfigureAppConfiguration(
            (hostContext, configuration) =>
            {
                // Defer to Uno's environment, unless the app set one on the context builder.
                if (context.Get<string>("EnvironmentName") is not { Length: > 0, })
                {
                    context.Set("EnvironmentName", hostContext.HostingEnvironment.EnvironmentName);
                }

                context.Set(hostContext.HostingEnvironment);

                // Layered on top of whatever Uno (and the app) already added, e.g. EmbeddedSource<App>(), so Dovetail's values win.
                var builder = new ConfigurationBuilder();
                Wait(builder.ApplyConfiguration(context, cancellationToken));
                if (builder.Sources is { Count: > 0, })
                {
                    configuration.Add(new ChainedConfigurationSource { Configuration = builder.Build(), ShouldDisposeConfiguration = true, });
                }
            }
        );

        host.ConfigureServices(
            (hostContext, services) =>
            {
                context.Set(hostContext.Configuration);
                Wait(services.ApplyService(context, cancellationToken));
            }
        );

        if (context.Get<ServiceProviderFactoryAdapter>() is { } factory)
        {
            host.UseServiceProviderFactory(new DeferredServiceProviderFactory(factory, context, cancellationToken));
        }
    }

    internal static void Wait(ValueTask task)
    {
        if (task.IsCompletedSuccessfully) return;
        task.AsTask().GetAwaiter().GetResult();
    }

    internal static T Wait<T>(ValueTask<T> task) => task.IsCompletedSuccessfully ? task.Result : task.AsTask().GetAwaiter().GetResult();

    /// <summary>
    ///     A container factory registered before the build, but created from the final service collection: the container
    ///     integrations capture the collection they are given when they're created.
    /// </summary>
    private sealed class DeferredServiceProviderFactory(ServiceProviderFactoryAdapter adapter, IDovetailContext context, CancellationToken cancellationToken)
        : IServiceProviderFactory<object>
    {
        private IServiceProviderFactory<object>? _factory;

        public object CreateBuilder(IServiceCollection services)
        {
            _factory = Wait(adapter(context, services, cancellationToken));
            return _factory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder) =>
            ( _factory ?? throw new InvalidOperationException("CreateBuilder must be called before CreateServiceProvider.") ).CreateServiceProvider(containerBuilder);
    }
}
