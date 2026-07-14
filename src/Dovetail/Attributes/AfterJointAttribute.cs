using Dovetail.Infrastructure;

namespace Dovetail.Attributes;
/// <summary>
///     An attribute that ensures the convention runs after the given <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="Attribute" />
/// <remarks>
///     The type to be used with the convention type
/// </remarks>
/// <param name="type">The type.</param>
/// <exception cref="NotSupportedException">Type must inherit from " + nameof(IDovetailJoint)</exception>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AfterJointAttribute(
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
    Type type
) : Attribute, IDovetailDependency
{
    private readonly Type _type = ThrowHelper.EnsureTypeIsDovetail(type);

    DependencyDirection IDovetailDependency.Direction => DependencyDirection.DependsOn;
    Type IDovetailDependency.Type => _type;
}

/// <summary>
///     An attribute that ensures the convention runs after the given <see cref="IDovetailJoint" />
/// </summary>
/// <seealso cref="Attribute" />
[PublicAPI]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AfterJointAttribute<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
T> : Attribute,
    IDovetailDependency
    where T : IDovetailJoint
{
    DependencyDirection IDovetailDependency.Direction => DependencyDirection.DependsOn;
    Type IDovetailDependency.Type => typeof(T);
}
