# IDovetailJointMetadata

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/Infrastructure/IDovetailJointMetadata.cs)

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

- [Category](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjointmetadata/properties.md#category) : [DovetailCategory](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dovetailcategory.md) `abstract` `get` -- The category of the convention
- [Dependencies](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjointmetadata/properties.md#dependencies) : [IReadOnlyCollection<IDovetailDependency>](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetaildependency.md) `abstract` `get` -- The dependencies
- [HostType](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjointmetadata/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The host type of the convention
- [Joint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjointmetadata/properties.md#joint) : [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md) `abstract` `get` -- The convention
