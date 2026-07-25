using Dovetail.Attributes;
using Dovetail.Joints;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Dovetail.DryIoc;

[DovetailExport]
internal class ConfigureDryIoc : ISetupJoint
{
    public void Register(IDovetailContext context)
    {
        context.UseServiceProviderFactory<IContainer>(
             async (context, services, ct) =>
            {
                var c = context.GetOrAdd<IContainer>(() => new Container());
                context.Set(services);
                await c.ApplyDryIoc(context, ct);
                return new DryIocDovetailServiceProviderFactory(context, services, c);
            }
        );
    }
}

file class DryIocDovetailServiceProviderFactory(IDovetailContext context, IServiceCollection services, IContainer container) : IServiceProviderFactory<IContainer>
{
    public IContainer CreateBuilder(IServiceCollection _) => container;
    public IServiceProvider CreateServiceProvider(IContainer containerBuilder)
    {
        var provider = containerBuilder.WithDependencyInjectionAdapter(services);
        if (context.DryIocOptions.NoMoreRegistrationAllowed)
        {
            provider.Container.WithNoMoreRegistrationAllowed();
        }

        return provider;
    }
}
