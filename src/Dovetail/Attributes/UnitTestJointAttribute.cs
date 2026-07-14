using Dovetail.Infrastructure;

namespace Dovetail.Attributes;
/// <summary>
///     Defines this convention as one that only runs during a unit test run
/// </summary>
/// <seealso cref="Attribute" />
[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public sealed class UnitTestJointAttribute : Attribute, IHostBasedJoint
{
    DovetailHostType IHostBasedJoint.HostType => DovetailHostType.UnitTest;
}
