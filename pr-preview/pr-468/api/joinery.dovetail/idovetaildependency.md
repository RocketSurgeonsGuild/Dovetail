# IDovetailDependency

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.md)
- Version: `0.8.1-pr.11`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-468-merge.Sha.1950047ba9b0769796f6cf96621cfafcd184f8e2/src/Joinery.Dovetail/Infrastructure/IDovetailDependency.cs)

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

- [Direction](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
