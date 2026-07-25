namespace Dovetail.Infrastructure;

/// <summary>
///     A dependency for a given convention
/// </summary>
public interface IDovetailDependency
{
    /// <summary>
    ///     The type
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type Type { get; }

    /// <summary>
    ///     The direction
    /// </summary>
    DependencyDirection Direction { get; }
}
