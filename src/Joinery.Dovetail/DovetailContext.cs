using System.Collections.Immutable;
using Joinery.Dovetail.Infrastructure;

namespace Joinery.Dovetail;

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
    ///     The categories of joints the context loads.
    /// </summary>
    /// <remarks>
    ///     The <c>Core</c> joints are always loaded, and when no category was given the <c>Application</c> joints are loaded as well,
    ///     so this always holds <c>Core</c>, and <c>Application</c> too in that case. A joint in any other category is loaded only
    ///     when its category was given.
    /// </remarks>
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

    internal DovetailContext(IEnumerable<IDovetailJointMetadata> jointsMetadata, IDovetailDictionary properties, IEnumerable<DovetailCategory> categories)
    {
        Categories = DovetailResolver.LoadedCategories(categories);
        Metadata = [.. jointsMetadata];
        _properties = properties;
        _properties.AddIfMissing(DovetailHostType.Undefined);
    }
}
