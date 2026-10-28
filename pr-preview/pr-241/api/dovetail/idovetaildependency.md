# IDovetailDependency

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-241/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

A dependency for a given convention

## Definition

```csharp
namespace Dovetail.Infrastructure;

public interface IDovetailDependency
{
    // ...
}
```

## Properties

- [Direction](/Dovetail/pr-preview/pr-241/api/dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/pr-preview/pr-241/api/dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/pr-preview/pr-241/api/dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
