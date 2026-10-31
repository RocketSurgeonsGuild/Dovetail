using System.Collections.Immutable;
using System.Reflection;
using Joinery.Dovetail.Infrastructure;

namespace Joinery.Dovetail;

/// <summary>
///     The base context marker interface to define this as a context
/// </summary>
[PublicAPI]
public interface IDovetailContext
{
    /// <summary>
    ///     The assembly that is executing the conventions
    /// </summary>
    // ReSharper disable once NullableWarningSuppressionIsUsed
    Assembly Assembly => Properties.Get<Assembly>("ExecutingAssembly") ?? Assembly.GetEntryAssembly()!;

    /// <summary>
    ///     The categories of joints the context loads.
    /// </summary>
    /// <remarks>
    ///     The <c>Core</c> joints are always loaded, and when no category was given the <c>Application</c> joints are loaded as well,
    ///     so this always holds <c>Core</c>, and <c>Application</c> too in that case. A joint in any other category is loaded only
    ///     when its category was given.
    /// </remarks>
    ImmutableHashSet<DovetailCategory> Categories { get; }

    /// <summary>
    ///     Get the conventions from the context
    /// </summary>
    ImmutableList<IDovetailJoint> Joints { get; }

    /// <summary>
    ///     The underlying host type
    /// </summary>
    DovetailHostType HostType { get; }

    internal ImmutableList<IDovetailJointMetadata> Metadata { get; }
    internal IDovetailDictionary Properties { get; }
    internal DovetailExceptionPolicyDelegate ExceptionPolicy => Properties.Require<DovetailExceptionPolicyDelegate>();
}
