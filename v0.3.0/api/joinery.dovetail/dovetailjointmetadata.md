# DovetailJointMetadata

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/v0.3.0/api/joinery.dovetail.md)
- Version: `0.3.0`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJointMetadata](/Dovetail/v0.3.0/api/joinery.dovetail/idovetailjointmetadata.md)

Container for conventions

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public sealed class DovetailJointMetadata
    : Joinery.Dovetail.Infrastructure.IDovetailJointMetadata
{
    // ...
}
```

## Constructors

- [DovetailJointMetadata(IDovetailJoint, DovetailHostType)](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype) -- The default constructor
- [DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype-dovetailcategory) -- The default constructor

## Properties

- [Category](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailcategory.md) `get` -- The category of the convention
- [Dependencies](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/v0.3.0/api/joinery.dovetail/idovetaildependency.md) `get; set` -- The dependencies
- [HostType](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailhosttype.md) `get` -- The host type of the convention
- [Joint](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/v0.3.0/api/joinery.dovetail/idovetailjoint.md) `get` -- The convention

## Methods

- [GetHashCode](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [WithDependency(DependencyDirection, Type)](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata/methods.md#withdependency-dependencydirection-type) : [DovetailJointMetadata](/Dovetail/v0.3.0/api/joinery.dovetail/dovetailjointmetadata.md) -- Adds a new dependency to the list
