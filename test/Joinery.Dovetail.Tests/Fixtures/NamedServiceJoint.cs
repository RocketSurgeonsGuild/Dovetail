using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Tests.Fixtures;

/// <summary>
///     A service joint that does nothing and prints as its name, so a snapshot of the scanned joints shows which ones were
///     found and in what order.
/// </summary>
internal sealed class NamedServiceJoint(string name) : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }

    public override string ToString() => name;
}
