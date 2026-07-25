using System.Reflection;

namespace Dovetail.Infrastructure;

/// <summary>
///  A marker interface to indicate a type is a joint delegate
/// </summary>
public interface IDovetailJointDelegate
{
    /// <summary>
    ///    The assembly of the joint delegate
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    ///   The expression of the joint delegate
    /// </summary>
    string Expression { get; }
}
