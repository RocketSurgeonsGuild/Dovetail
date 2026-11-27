# IDovetailDependency

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail/Infrastructure/IDovetailDependency.cs)

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

- [Direction](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
