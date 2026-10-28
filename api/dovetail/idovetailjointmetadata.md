# IDovetailJointMetadata

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

Declares a convention result with it's dependencies pre-computed

## Definition

```csharp
namespace Dovetail.Infrastructure;

public interface IDovetailJointMetadata
{
    // ...
}
```

## Properties

- [Category](/Dovetail/api/dovetail/idovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/api/dovetail/dovetailcategory.md) `abstract` `get` -- The category of the convention
- [Dependencies](/Dovetail/api/dovetail/idovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/api/dovetail/idovetaildependency.md) `abstract` `get` -- The dependencies
- [HostType](/Dovetail/api/dovetail/idovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/api/dovetail/dovetailhosttype.md) `abstract` `get` -- The host type of the convention
- [Joint](/Dovetail/api/dovetail/idovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md) `abstract` `get` -- The convention
