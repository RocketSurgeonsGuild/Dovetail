using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Infrastructure;

/// <summary>
///     Container for conventions
/// </summary>
[PublicAPI]
public sealed class DovetailJointMetadata : IDovetailJointMetadata
{
    private readonly List<IDovetailDependency> _dependencies = [];

    /// <summary>
    ///     The default constructor
    /// </summary>
    /// <param name="joint">The joint this metadata describes.</param>
    /// <param name="hostType">The host type the convention applies to.</param>
    public DovetailJointMetadata(IDovetailJoint joint, DovetailHostType hostType)
    {
        Joint = joint;
        HostType = hostType;
        Category = DovetailCategory.Application;
    }

    /// <summary>
    ///     The default constructor
    /// </summary>
    /// <param name="joint">The joint this metadata describes.</param>
    /// <param name="hostType">The host type the convention applies to.</param>
    /// <param name="category">The category the convention belongs to.</param>
    public DovetailJointMetadata(IDovetailJoint joint, DovetailHostType hostType, DovetailCategory category)
    {
        Joint = joint;
        HostType = hostType;
        Category = category;
    }

    /// <summary>
    ///     Adds a new dependency to the list
    /// </summary>
    /// <param name="direction">The direction of the dependency.</param>
    /// <param name="type">The <see cref="IDovetailJoint" /> type that this dependency relates to.</param>
    /// <returns>This <see cref="DovetailJointMetadata" />, for chaining.</returns>
    public DovetailJointMetadata WithDependency(DependencyDirection direction, Type type)
    {
        _dependencies.Add(new DovetailDependency(direction, type));
        return this;
    }

    /// <inheritdoc />
    public IDovetailJoint Joint { get; }

    /// <summary>
    ///     The dependencies
    /// </summary>
    public IReadOnlyCollection<IDovetailDependency> Dependencies
    {
        get => _dependencies;
        set
        {
            _dependencies.Clear();
            _dependencies.AddRange(value.Select(x => new DovetailDependency(x.Direction, x.Type)).OfType<IDovetailDependency>());
        }
    }

    /// <inheritdoc />
    public DovetailHostType HostType { get; }

    /// <summary>
    ///    The category of the convention
    /// </summary>
    public DovetailCategory Category { get; }

    /// <inheritdoc />
    public override int GetHashCode() => Joint.GetHashCode();
}

/// <summary>
///     Container for conventions
/// </summary>
/// <typeparam name="T">The <see cref="IDovetailJoint" /> type that this metadata lazily constructs.</typeparam>
/// <param name="properties">The properties used to construct the deferred <typeparamref name="T" /> instance.</param>
/// <param name="hostType">The host type the convention applies to.</param>
/// <param name="category">The category the convention belongs to.</param>
internal sealed class DovetailDeferredJointMetadata<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IReadOnlyDovetailDictionary properties, DovetailHostType hostType, DovetailCategory category) : IDovetailJointMetadata where T : IDovetailJoint
{
    private readonly List<IDovetailDependency> _dependencies = [];

    /// <summary>
    ///     Adds a new dependency to the list
    /// </summary>
    /// <param name="direction">The direction of the dependency.</param>
    /// <param name="type">The <see cref="IDovetailJoint" /> type that this dependency relates to.</param>
    /// <returns>This <see cref="DovetailDeferredJointMetadata{T}" />, for chaining.</returns>
    public DovetailDeferredJointMetadata<T> WithDependency(DependencyDirection direction, Type type)
    {
        _dependencies.Add(new DovetailDependency(direction, type));
        return this;
    }

    /// <inheritdoc />
    public IDovetailJoint Joint => field ??= ActivatorUtilities.CreateInstance<T>(properties);

    /// <summary>
    ///     The dependencies
    /// </summary>
    public IReadOnlyCollection<IDovetailDependency> Dependencies
    {
        get => _dependencies;
        set
        {
            _dependencies.Clear();
            _dependencies.AddRange(value.Select(x => new DovetailDependency(x.Direction, x.Type)).OfType<IDovetailDependency>());
        }
    }

    /// <inheritdoc />
    public DovetailHostType HostType { get; } = hostType;

    /// <summary>
    ///    The category of the convention
    /// </summary>
    public DovetailCategory Category { get; } = category;
}
