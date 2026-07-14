using Dovetail.Infrastructure;

namespace Dovetail.Attributes;
/// <summary>
///     Defines this convention as one that only runs during live usage to avoid unit tests
/// </summary>
/// <seealso cref="Attribute" />
[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public sealed class LiveJointAttribute : Attribute, IHostBasedJoint
{
    DovetailHostType IHostBasedJoint.HostType => DovetailHostType.Live;
}
