# IDovetailJointMetadata

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.4`
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

- [Category](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/pr-preview/pr-235/api/dovetail/dovetailcategory.md) `abstract` `get` -- The category of the convention
- [Dependencies](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-235/api/dovetail/idovetaildependency.md) `abstract` `get` -- The dependencies
- [HostType](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-235/api/dovetail/dovetailhosttype.md) `abstract` `get` -- The host type of the convention
- [Joint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md) `abstract` `get` -- The convention
