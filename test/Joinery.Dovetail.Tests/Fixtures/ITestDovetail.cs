using Joinery.Dovetail.Infrastructure;

namespace Joinery.Dovetail.Tests.Fixtures;

public interface ITestDovetail : IDovetailJoint
{
    void Register(ITestDovetailContext context);
}
