namespace Dovetail.Infrastructure;

/// <summary>
///    A marker interface to indicate a type is a convention and has a name
/// </summary>
/// <param name="name"></param>
[AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Interface, AllowMultiple = false)]
public sealed class JointNameAttribute(string name) : Attribute
{
    /// <summary>
    ///   The name of the joint
    /// </summary>
    public string Name => name;
}
