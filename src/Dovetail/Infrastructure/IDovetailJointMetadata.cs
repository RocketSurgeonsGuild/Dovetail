namespace Dovetail.Infrastructure;

/// <summary>
///     Declares a convention result with it's dependencies pre-computed
/// </summary>
public interface IDovetailJointMetadata
{
    /// <summary>
    ///     The convention
    /// </summary>
    IDovetailJoint Joint { get; }

    /// <summary>
    ///     The dependencies
    /// </summary>
    IReadOnlyCollection<IDovetailDependency> Dependencies { get; }

    /// <summary>
    ///     The host type of the convention
    /// </summary>
    DovetailHostType HostType { get; }

    /// <summary>
    ///     The category of the convention
    /// </summary>
    DovetailCategory Category { get; }

    /// <summary>
    ///    The value comparer for this category
    /// </summary>
    internal static IEqualityComparer<IDovetailJointMetadata> ValueComparer { get; } = new ValueEqualityComparer();

    private sealed class ValueEqualityComparer : IEqualityComparer<IDovetailJointMetadata>
    {
        public bool Equals(IDovetailJointMetadata? x, IDovetailJointMetadata? y) => ReferenceEquals(x?.Joint, y?.Joint) || ( x is { } && y is { } && x.Joint.GetType() == y.Joint.GetType() );

        public int GetHashCode(IDovetailJointMetadata obj) => obj.Joint.GetType().GetHashCode();
    }
}
