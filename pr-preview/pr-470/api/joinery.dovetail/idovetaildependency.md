# IDovetailDependency

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail/Infrastructure/IDovetailDependency.cs)

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

- [Direction](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
