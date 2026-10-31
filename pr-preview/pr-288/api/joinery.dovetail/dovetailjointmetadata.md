# DovetailJointMetadata

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-288/api/joinery.dovetail.md)
- Version: `0.3.2-pr.3`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Implements: [IDovetailJointMetadata](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/idovetailjointmetadata.md)

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

- [DovetailJointMetadata(IDovetailJoint, DovetailHostType)](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype) -- The default constructor
- [DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/constructors.md#constructor-idovetailjoint-dovetailhosttype-dovetailcategory) -- The default constructor

## Properties

- [Category](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailcategory.md) `get` -- The category of the convention
- [Dependencies](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/idovetaildependency.md) `get; set` -- The dependencies
- [HostType](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailhosttype.md) `get` -- The host type of the convention
- [Joint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/idovetailjoint.md) `get` -- The convention

## Methods

- [FromFactory(Func<IServiceProvider, IDovetailJoint>, DovetailHostType, DovetailCategory)](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/methods.md#fromfactory-func-iserviceprovider-idovetailjoint-dovetailhosttype-dovetailcategory) : [DovetailJointMetadata](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata.md) `static` -- Describes a joint that is created the first time [DovetailJointMetadata.Joint](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/properties.md#joint) is read, rather than up front.
- [GetHashCode](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/methods.md#gethashcode) : `int` -- Serves as the default hash function.
- [WithDependency(DependencyDirection, Type)](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata/methods.md#withdependency-dependencydirection-type) : [DovetailJointMetadata](/Dovetail/pr-preview/pr-288/api/joinery.dovetail/dovetailjointmetadata.md) -- Adds a new dependency to the list
