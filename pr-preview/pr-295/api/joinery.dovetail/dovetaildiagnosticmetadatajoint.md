# DovetailDiagnosticMetadataJoint

- Kind: `record`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.md)
- Version: `0.4.1-pr.6`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Implements: `IEquatable<DovetailDiagnosticMetadataJoint>`

Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public record DovetailDiagnosticMetadataJoint
    : System.IEquatable<Joinery.Dovetail.Diagnostics.DovetailDiagnosticMetadataJoint>
{
    // ...
}
```

## Constructors

- [DovetailDiagnosticMetadataJoint(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/constructors.md#constructor-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency) -- Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

## Properties

- [AssemblyName](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#assemblyname) : `string` `get; init` -- The assembly name of the joint.
- [Category](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#category) : `string` `get; init` -- The category of the joint.
- [ConnectsToJointTypes](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#connectstojointtypes) : `ImmutableList<string>` `get; init` -- The types of the joint that this joint connects to.
- [Dependencies](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#dependencies) : `ImmutableList<DovetailDiagnosticMetadataJointDependency>` `get; init` -- The dependencies of the joint.
- [HostType](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetailhosttype.md) `get; init` -- The host type of the joint.
- [ImplementsJointTypes](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#implementsjointtypes) : `ImmutableList<string>` `get; init` -- The types of the joint.
- [JointName](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/properties.md#jointname) : `string` `get; init` -- The name of the joint.

## Methods

- [<Clone>$](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#clone) : [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) `virtual`
- [Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#deconstruct-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency)
- [Equals(object?)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [Equals(DovetailDiagnosticMetadataJoint?)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#equals-dovetaildiagnosticmetadatajoint) : `bool` `virtual` -- Indicates whether the current object is equal to another object of the same type.
- [GetHashCode](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Equality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#op-equality-dovetaildiagnosticmetadatajoint-dovetaildiagnosticmetadatajoint) : `bool` `static`
- [op_Inequality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#op-inequality-dovetaildiagnosticmetadatajoint-dovetaildiagnosticmetadatajoint) : `bool` `static`
- [ToString](/Dovetail/pr-preview/pr-295/api/joinery.dovetail/dovetaildiagnosticmetadatajoint/methods.md#tostring) : `string` -- Returns a string that represents the current object.
