using System.Collections.Immutable;
using Dovetail.Infrastructure;

namespace Dovetail;

/// <summary>
///     Base convention context that allows for stashing items in index keys
///     Implements the <see cref="IDovetailContext" />
/// </summary>
/// <seealso cref="IDovetailContext" />
public sealed class DovetailContext : IDovetailContext
{
    private readonly IDovetailDictionary _properties;

    /// <summary>
    ///     Create a context from a given builder
    /// </summary>
    /// <param name="builder">The builder to create the context from.</param>
    /// <param name="cancellationToken">The cancellation token used during setup.</param>
    /// <returns>The newly created and set up <see cref="IDovetailContext" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static ValueTask<IDovetailContext> FromAsync(DovetailContextBuilder builder, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.CreateAsync(cancellationToken);
    }

    /// <summary>
    ///     The categories of the convention context
    /// </summary>
    public ImmutableHashSet<DovetailCategory> Categories { get; set; }

    /// <summary>
    ///     Get the conventions from the context
    /// </summary>
    public ImmutableList<IDovetailJoint> Joints =>
    field ??= DovetailResolver.Resolve(_properties.Require<DovetailHostType>(), Categories, Metadata);

    internal ImmutableList<IDovetailJointMetadata> Metadata { get; }

    /// <summary>
    ///     The host type
    /// </summary>
    public DovetailHostType HostType => this.GetHostType();
    IDovetailDictionary IDovetailContext.Properties => _properties;
    ImmutableList<IDovetailJointMetadata> IDovetailContext.Metadata => Metadata;

    internal DovetailContext(
        IEnumerable<IDovetailJointMetadata> jointsMetadata,
        IDovetailDictionary properties,
        IEnumerable<DovetailCategory> categories)
    {
        Categories = categories.ToImmutableHashSet(DovetailCategory.ValueComparer);
        Metadata = [.. jointsMetadata];
        _properties = properties;
        _properties.AddIfMissing(DovetailHostType.Undefined);
    }

}
