using System.Collections.Immutable;
using System.Reflection;
using Dovetail.Infrastructure;

namespace Dovetail;

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
    ///     The categories of the convention context
    /// </summary>
    ImmutableHashSet<DovetailCategory> Categories { get; }

    /// <summary>
    ///     Get the conventions from the context
    /// </summary>
    ImmutableList<IDovetailJoint> Joints { get; }

    /// <summary>
    ///     The underlying host type
    /// </summary>
    DovetailHostType HostType { get; }

    /// <summary>
    ///  The raw metadata for the joints in this context
    /// </summary>
    ImmutableList<IDovetailJointMetadata> Metadata { get; }
    internal IDovetailDictionary Properties { get; }

    internal DovetailExceptionPolicyDelegate ExceptionPolicy => Properties.Require<DovetailExceptionPolicyDelegate>();
}
