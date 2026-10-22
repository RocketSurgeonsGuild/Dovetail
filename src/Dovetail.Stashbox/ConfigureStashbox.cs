using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Stashbox;
using Stashbox.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Dovetail.Stashbox;

[DovetailExport]
internal class ConfigureStashbox : ISetupJoint
{
    public void Register(IDovetailContext context)
    {
        context.UseServiceProviderFactory<IStashboxContainer>(
            async (context, services, ct) =>
            {
                var options = context.StashboxOptions;
                var c = context.GetOrAdd<IStashboxContainer>(() => new StashboxContainer(options.ConfigureContainer));
                context.Set(services);
                await c.ApplyStashbox(context, ct);
                return new StashboxDovetailServiceProviderFactory(context, options, c);
            }
        );
    }
}

file class StashboxDovetailServiceProviderFactory(IDovetailContext context, StashboxOptions options, IStashboxContainer container)
    : IServiceProviderFactory<IStashboxContainer>
{
    private readonly StashboxServiceProviderFactory _factory = new(container);

    public IStashboxContainer CreateBuilder(IServiceCollection services)
    {
        // The adapter overwrites parts of the container configuration while registering the services, so (re)apply ours afterward.
        var builder = _factory.CreateBuilder(services);
        // Unlike DryIoc, the adapter doesn't expose the container itself; the service provider already owns its disposal.
        builder.RegisterInstance(builder, withoutDisposalTracking: true);
        if (options.ValidateLifetimesInDevelopment && string.Equals(context.Get<string>("EnvironmentName"), "Development", StringComparison.OrdinalIgnoreCase))
        {
            builder.Configure(config => config.WithLifetimeValidation());
        }

        if (options.ConfigureContainer is { } configure)
        {
            builder.Configure(configure);
        }

        return builder;
    }

    public IServiceProvider CreateServiceProvider(IStashboxContainer containerBuilder)
    {
        var provider = _factory.CreateServiceProvider(containerBuilder);
        if (options.ValidateOnBuild)
        {
            containerBuilder.Validate();
        }

        return provider;
    }
}
