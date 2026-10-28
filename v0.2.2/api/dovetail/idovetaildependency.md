# IDovetailDependency

- Kind: `interface`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
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

- [Direction](/Dovetail/v0.2.2/api/dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/v0.2.2/api/dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/v0.2.2/api/dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
