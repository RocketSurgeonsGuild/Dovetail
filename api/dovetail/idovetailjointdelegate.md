# IDovetailJointDelegate

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

A marker interface to indicate a type is a joint delegate

## Definition

```csharp
namespace Dovetail.Infrastructure;

public interface IDovetailJointDelegate
{
    // ...
}
```

## Properties

- [Assembly](/Dovetail/api/dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/api/dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
