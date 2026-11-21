# IDovetailDependency

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.4`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail/Infrastructure/IDovetailDependency.cs)

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

- [Direction](/Dovetail/api/joinery.dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/api/joinery.dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/api/joinery.dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
