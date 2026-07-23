using System.Reflection;

namespace Dovetail.Infrastructure;

/// <summary>
///    A marker interface to indicate a type is a convention and has a name
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ConnectsJointAttribute<T>() : Attribute where T : IDovetailJoint
{
    /// <summary>
    ///  The name of the joint that this joint connects to
    /// </summary>
    public string JointName => typeof(T).GetCustomAttribute<JointNameAttribute>()?.Name ?? throw new InvalidOperationException($"The joint type {typeof(T).FullName} does not have a {nameof(JointNameAttribute)} applied.");
}
