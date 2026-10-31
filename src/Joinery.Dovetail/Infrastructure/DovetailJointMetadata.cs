using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.Infrastructure;

/// <summary>
///     Container for conventions
/// </summary>
[PublicAPI]
public sealed class DovetailJointMetadata : IDovetailJointMetadata
{
    private readonly List<IDovetailDependency> _dependencies = [];
    private readonly Func<IServiceProvider, IDovetailJoint>? _factory;
    private IServiceProvider? _services;
    private IDovetailJoint? _joint;

    /// <summary>
    ///     The default constructor
    /// </summary>
    /// <param name="joint">The joint this metadata describes.</param>
    /// <param name="hostType">The host type the convention applies to.</param>
    public DovetailJointMetadata(IDovetailJoint joint, DovetailHostType hostType)
    {
        _joint = joint;
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
        _joint = joint;
        HostType = hostType;
        Category = category;
    }

    /// <summary>
    ///     Describes a joint that is created the first time <see cref="Joint" /> is read, rather than up front.
    /// </summary>
    /// <remarks>
    ///     This is how the code generated for an exported joint with constructor parameters creates it: the factory resolves each
    ///     parameter from the properties of the <see cref="DovetailContextBuilder" /> the metadata was given to, so a value set on the
    ///     builder after the metadata was created is still seen. A parameter with a default value falls back to it when the
    ///     properties hold nothing of that type, and a parameter without one makes the factory throw
    ///     <see cref="InvalidOperationException" />. Read before a builder has the metadata, <see cref="Joint" /> resolves
    ///     against empty properties. The joint is created once.
    /// </remarks>
    /// <param name="factory">Creates the joint from the properties of the context builder.</param>
    /// <param name="hostType">The host type the convention applies to.</param>
    /// <param name="category">The category the convention belongs to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <see langword="null" />.</exception>
    public DovetailJointMetadata(Func<IServiceProvider, IDovetailJoint> factory, DovetailHostType hostType, DovetailCategory category)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        HostType = hostType;
        Category = category;
    }

    /// <summary>
    ///     Gives a deferred joint the properties of the builder that now holds it. A joint that is already created keeps the
    ///     values it was created with.
    /// </summary>
    /// <param name="services">The properties of the context builder.</param>
    internal void Bind(IServiceProvider services)
    {
        if (_joint is null) _services = services;
    }

    /// <summary>
    ///     Adds a new dependency to the list
    /// </summary>
    /// <param name="direction">The direction of the dependency.</param>
    /// <param name="type">The <see cref="IDovetailJoint" /> type that this dependency relates to.</param>
    /// <returns>This <see cref="DovetailJointMetadata" />, for chaining.</returns>
    public DovetailJointMetadata WithDependency(DependencyDirection direction, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type)
    {
        _dependencies.Add(new DovetailDependency(direction, type));
        return this;
    }

    /// <inheritdoc />
    public IDovetailJoint Joint => _joint ??= _factory!(_services ?? EmptyServiceProvider.Instance);

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

    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public static EmptyServiceProvider Instance { get; } = new();

        public object? GetService(Type serviceType) => null;
    }
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
    public DovetailDeferredJointMetadata<T> WithDependency(DependencyDirection direction, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type)
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
