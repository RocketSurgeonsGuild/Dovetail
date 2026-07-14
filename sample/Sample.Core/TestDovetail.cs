using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Core;

[DovetailExport]
[UnitTestJoint]
[AfterJoint(typeof(CoreConvention))]
public class TestConvention : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<IService, TestService>();
}
