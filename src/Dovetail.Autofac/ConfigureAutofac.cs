using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Dovetail;

[DovetailExport]
internal class ConfigureAutofac : ISetupJoint
{
    public void Register(IDovetailContext context)
    {
        context.UseServiceProviderFactory<ContainerBuilder>(
             async (context, services, ct) =>
            {
                var c = context.GetOrAdd(() => new ContainerBuilder());
                context.Set(services);
                await c.ApplyAutofac(context, ct);
                return new AutofacDovetailServiceProviderFactory(c);
            }
        );
    }
}

file class AutofacDovetailServiceProviderFactory(ContainerBuilder? container = null) : IServiceProviderFactory<ContainerBuilder>
{
    private readonly ContainerBuilder _container = container ?? new ContainerBuilder();

    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _container.Populate(services);
        return _container;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder) => containerBuilder.Build().Resolve<IServiceProvider>();
}
