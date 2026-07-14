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
    /// <param name="convention"></param>
    /// <param name="hostType"></param>
    public DovetailJointMetadata(IDovetailJoint convention, DovetailHostType hostType)
    {
        Joint = convention;
        HostType = hostType;
        Category = DovetailCategory.Application;
    }

    /// <summary>
    ///     The default constructor
    /// </summary>
    /// <param name="convention"></param>
    /// <param name="hostType"></param>
    /// <param name="category"></param>
    public DovetailJointMetadata(IDovetailJoint convention, DovetailHostType hostType, DovetailCategory category)
    {
        Joint = convention;
        HostType = hostType;
        Category = category;
    }

    /// <summary>
    ///     Adds a new dependency to the list
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="type"></param>
    /// <returns></returns>
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

    /// <summary>
    ///    The equality comparer for this type
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Joint.GetHashCode();
}

/// <summary>
///     Container for conventions
/// </summary>
/// <param name="properties"></param>
/// <param name="hostType"></param>
/// <param name="category"></param>
internal sealed class DovetailDeferredJointMetadata<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IReadOnlyDovetailDictionary properties, DovetailHostType hostType, DovetailCategory category) : IDovetailJointMetadata where T : IDovetailJoint
{
    private readonly List<IDovetailDependency> _dependencies = [];

    /// <summary>
    ///     Adds a new dependency to the list
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="type"></param>
    /// <returns></returns>
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
