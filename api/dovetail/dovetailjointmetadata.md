# DovetailJointMetadata

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJointMetadata](/Dovetail/api/dovetail/idovetailjointmetadata.md)

Container for conventions

## Definition

```csharp
namespace Dovetail.Infrastructure;

public sealed class DovetailJointMetadata
    : Dovetail.Infrastructure.IDovetailJointMetadata
{
    // ...
}
```

## Constructors

- [DovetailJointMetadata(IDovetailJoint, DovetailHostType)](/Dovetail/api/dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype) -- The default constructor
- [DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)](/Dovetail/api/dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype-dovetailcategory) -- The default constructor

## Properties

- [Category](/Dovetail/api/dovetail/dovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/api/dovetail/dovetailcategory.md) `get` -- The category of the convention
- [Dependencies](/Dovetail/api/dovetail/dovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/api/dovetail/idovetaildependency.md) `get; set` -- The dependencies
- [HostType](/Dovetail/api/dovetail/dovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/api/dovetail/dovetailhosttype.md) `get` -- The host type of the convention
- [Joint](/Dovetail/api/dovetail/dovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md) `get` -- The convention

## Methods

- [GetHashCode](/Dovetail/api/dovetail/dovetailjointmetadata/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [WithDependency(DependencyDirection, Type)](/Dovetail/api/dovetail/dovetailjointmetadata/methods.md#withdependency-dependencydirection-type) : [DovetailJointMetadata](/Dovetail/api/dovetail/dovetailjointmetadata.md) -- Adds a new dependency to the list
