# DovetailDiagnosticMetadataJointDependency

- Kind: `record`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-259/api/joinery.dovetail.md)
- Version: `0.3.1-pr.2`
- Namespace: `Joinery.Dovetail.Diagnostics`
- Target framework: `net10.0`
- Inherits: [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md)
- Implements: `IEquatable<DovetailDiagnosticMetadataJointDependency>`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

## Definition

```csharp
namespace Joinery.Dovetail.Diagnostics;

public sealed record DovetailDiagnosticMetadataJointDependency
    : Joinery.Dovetail.Diagnostics.DovetailDiagnosticMetadataJoint,
      System.IEquatable<Joinery.Dovetail.Diagnostics.DovetailDiagnosticMetadataJointDependency>
{
    // ...
}
```

## Constructors

- [DovetailDiagnosticMetadataJointDependency(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/constructors.md#constructor-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency-dependencydirection) -- Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

## Properties

- [Direction](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dependencydirection.md) `get; init` -- The direction of the dependency.

## Methods

- [<Clone>$](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#clone) : [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- [Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#deconstruct-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency-dependencydirection)
- [Equals(object?)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#equals-object) : `bool` -- Determines whether the specified object is equal to the current object.
- [Equals(DovetailDiagnosticMetadataJoint?)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#equals-dovetaildiagnosticmetadatajoint) : `bool` -- Indicates whether the current object is equal to another object of the same type.
- [Equals(DovetailDiagnosticMetadataJointDependency?)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#equals-dovetaildiagnosticmetadatajointdependency) : `bool` -- Indicates whether the current object is equal to another object of the same type.
- [GetHashCode](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [op_Equality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#op-equality-dovetaildiagnosticmetadatajointdependency-dovetaildiagnosticmetadatajointdependency) : `bool` `static`
- [op_Inequality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#op-inequality-dovetaildiagnosticmetadatajointdependency-dovetaildiagnosticmetadatajointdependency) : `bool` `static`
- [ToString](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency/methods.md#tostring) : `string` -- Returns a string that represents the current object.
