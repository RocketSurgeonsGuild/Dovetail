using Dovetail.Attributes;
using Dovetail.Testing.Joints;
using Dovetail.Joints;

// ReSharper disable once CheckNamespace
namespace Dovetail;

 /// <summary>
/// This joint allows you to use this PackageReference as a way to share test libraries
/// and configurations between your teams, without a specific dependency on any testing
/// framework.  These are however limted to only when the host type is the unit test host.
/// This host type is configured automatically if the project setting IsTestProject is true.
///
/// By having this as a package reference, you can share those services with others.
/// </summary>
[DovetailExport]
internal class TestingJoint : ISetupAsyncJoint
{
    public ValueTask Register(IDovetailContext context, CancellationToken cancellationToken = default) =>
        context.IsUnitTestHost()
            ? context.ApplyTesting(cancellationToken)
            : ValueTask.CompletedTask;
}
