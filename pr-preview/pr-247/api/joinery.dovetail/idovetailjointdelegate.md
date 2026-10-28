# IDovetailJointDelegate

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.md)
- Version: `0.2.4-pr.4`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`

A marker interface to indicate a type is a joint delegate

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public interface IDovetailJointDelegate
{
    // ...
}
```

## Properties

- [Assembly](/Dovetail/pr-preview/pr-247/api/joinery.dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/pr-preview/pr-247/api/joinery.dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
