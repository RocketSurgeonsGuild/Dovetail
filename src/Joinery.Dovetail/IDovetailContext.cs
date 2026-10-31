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
    ///     The categories the context is limited to.
    /// </summary>
    /// <remarks>Only the joints whose category is in this set are loaded, and an empty set loads every joint.</remarks>
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
