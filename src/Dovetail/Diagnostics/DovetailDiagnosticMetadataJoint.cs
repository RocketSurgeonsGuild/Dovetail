using System.Collections.Immutable;

namespace Dovetail.Diagnostics;

/// <summary>
///   Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.
/// </summary>
/// <param name="JointName">The name of the joint.</param>
/// <param name="AssemblyName">The assembly name of the joint.</param>
/// <param name="ImplementsJointTypes">The types of the joint.</param>
/// <param name="ConnectsToJointTypes">The types of the joint that this joint connects to.</param>
/// <param name="HostType">The host type of the joint.</param>
/// <param name="Category">The category of the joint.</param>
/// <param name="Dependencies">The dependencies of the joint.</param>
public record DovetailDiagnosticMetadataJoint
(
string JointName,
string AssemblyName,
ImmutableList<string> ImplementsJointTypes,
ImmutableList<string> ConnectsToJointTypes,
    DovetailHostType HostType,
 string Category,
ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies);
