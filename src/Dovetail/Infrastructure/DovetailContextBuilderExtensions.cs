using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Dovetail.Infrastructure;
/// <summary>
///     Dovetail Context build extensions.
/// </summary>
[PublicAPI]
public static class DovetailContextBuilderExtensions
{
    /// <summary>
    ///     Allows creation of a service provider from the convention context.  This will apply configuration
    /// </summary>
    /// <param name="context">The context to build a service provider from.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration, services, and logging.</param>
    /// <returns>The <see cref="IServiceProvider" /> built from the context.</returns>
    public static async ValueTask<IServiceProvider> CreateServiceProvider(this IDovetailContext context, CancellationToken cancellationToken = default)
    {
        var cb = new ConfigurationManager();
        await cb.ApplyConfiguration(context, cancellationToken).ConfigureAwait(false);
        context.Set(cb).Set<IConfigurationRoot>(cb).Set<IConfiguration>(cb);
        var services = new ServiceCollection();
        services.AddSingleton<IConfigurationRoot>(cb).AddSingleton(cb).AddSingleton<IConfiguration>(cb);
        await services.ApplyService(context, cancellationToken).ConfigureAwait(false);
        await new LoggingBuilder(services).ApplyLogging(context, cancellationToken).ConfigureAwait(false);

        if (context.Get<ServiceProviderFactoryAdapter>() is not { } factory)
            return services.BuildServiceProvider(context.GetOrAdd(() => new ServiceProviderOptions()));

        var adapter = await factory(context, services, cancellationToken).ConfigureAwait(false);
        var builder = adapter.CreateBuilder(services);
        return adapter.CreateServiceProvider(builder);
    }
}
