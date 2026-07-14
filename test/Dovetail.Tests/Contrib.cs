using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Tests;

[DovetailExport]
internal sealed class Contrib : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}
