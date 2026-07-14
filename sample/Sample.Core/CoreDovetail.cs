using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Core;

[DovetailExport]
public class CoreConvention : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<IService, AService>();
}
