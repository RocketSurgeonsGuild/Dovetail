using System.Collections.Immutable;
using Dovetail.Infrastructure;

namespace Dovetail.Diagnostics;

/// <summary>
///  Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.
/// </summary>
/// <param name="JointName">The name of the joint.</param>
/// <param name="AssemblyName">The assembly name of the joint.</param>
/// <param name="ImplementsJointTypes">The types of the joint.</param>
/// <param name="ConnectsToJointTypes">The types of the joint that this joint connects to.</param>
/// <param name="HostType">The host type of the joint.</param>
/// <param name="Category">The category of the joint.</param>
/// <param name="Dependencies">The dependencies of the joint.</param>
/// <param name="Direction">The direction of the dependency.</param>
public sealed record DovetailDiagnosticMetadataJointDependency(
string JointName,
string AssemblyName,
ImmutableList<string> ImplementsJointTypes,
ImmutableList<string> ConnectsToJointTypes,
    DovetailHostType HostType,
 string Category,
ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies,
DependencyDirection Direction) : DovetailDiagnosticMetadataJoint(JointName, AssemblyName, ImplementsJointTypes, ConnectsToJointTypes, HostType, Category, Dependencies);
