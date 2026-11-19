# IDovetailDependency

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/Infrastructure/IDovetailDependency.cs)

A dependency for a given convention

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public interface IDovetailDependency
{
    // ...
}
```

## Properties

- [Direction](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
