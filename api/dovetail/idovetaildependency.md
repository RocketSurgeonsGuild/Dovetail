# IDovetailDependency

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.2`
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

- [Direction](/Dovetail/api/dovetail/idovetaildependency/properties.md#direction) : [DependencyDirection](/Dovetail/api/dovetail/dependencydirection.md) `abstract` `get` -- The direction
- [Type](/Dovetail/api/dovetail/idovetaildependency/properties.md#type) : `Type` `abstract` `get` -- The type
