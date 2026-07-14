using Dovetail.Attributes;
using Dovetail.Joints;
using DryIoc;

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
