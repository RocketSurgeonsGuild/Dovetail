using System.Collections.Immutable;
using Dovetail.Infrastructure;
using Dovetail.Joints;

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
    /// <param name="builder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    public ImmutableList<IDovetailJoint> Joints { get; }

    /// <summary>
    ///     The host type
    /// </summary>
    public DovetailHostType HostType => this.GetHostType();
    IDovetailDictionary IDovetailContext.Properties => _properties;

    internal DovetailContext(
        DovetailHostType hostType,
        IEnumerable<IDovetailJointMetadata> joints,
        IDovetailDictionary properties,
        IEnumerable<DovetailCategory> categories)
    {
        Categories = categories.ToImmutableHashSet(DovetailCategory.ValueComparer);
        Joints = DovetailResolver.Resolve(hostType, Categories, joints);
        _properties = properties;
    }

}
