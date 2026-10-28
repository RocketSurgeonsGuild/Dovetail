# IDovetailJointDelegate

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-238/api/dovetail.md)
- Version: `0.2.2-pr.3`
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

- [Assembly](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
