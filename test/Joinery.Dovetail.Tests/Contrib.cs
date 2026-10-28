using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Tests;

[DovetailExport]
internal sealed class Contrib : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}
