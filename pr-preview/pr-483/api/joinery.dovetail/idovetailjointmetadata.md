# IDovetailJointMetadata

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail/Infrastructure/IDovetailJointMetadata.cs)

Declares a convention result with it's dependencies pre-computed

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public interface IDovetailJointMetadata
{
    // ...
}
```

## Properties

- [Category](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailcategory.md) `abstract` `get` -- The category of the convention
- [Dependencies](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetaildependency.md) `abstract` `get` -- The dependencies
- [HostType](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The host type of the convention
- [Joint](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/pr-preview/pr-483/api/joinery.dovetail/idovetailjoint.md) `abstract` `get` -- The convention
