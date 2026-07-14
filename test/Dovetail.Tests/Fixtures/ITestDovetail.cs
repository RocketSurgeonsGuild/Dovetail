using Dovetail.Infrastructure;

namespace Dovetail.Tests.Fixtures;

public interface ITestDovetail : IDovetailJoint
{
    void Register(ITestDovetailContext context);
}
