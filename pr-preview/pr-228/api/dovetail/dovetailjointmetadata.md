# DovetailJointMetadata

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJointMetadata](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjointmetadata.md)

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

- [DovetailJointMetadata(IDovetailJoint, DovetailHostType)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype) -- The default constructor
- [DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype-dovetailcategory) -- The default constructor

## Properties

- [Category](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailcategory.md) `get` -- The category of the convention
- [Dependencies](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-228/api/dovetail/idovetaildependency.md) `get; set` -- The dependencies
- [HostType](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailhosttype.md) `get` -- The host type of the convention
- [Joint](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md) `get` -- The convention

## Methods

- [GetHashCode](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [WithDependency(DependencyDirection, Type)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata/methods.md#withdependency-dependencydirection-type) : [DovetailJointMetadata](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailjointmetadata.md) -- Adds a new dependency to the list
