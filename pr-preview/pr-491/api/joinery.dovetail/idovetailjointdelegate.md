# IDovetailJointDelegate

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail/Infrastructure/IDovetailJointDelegate.cs)

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

- [Assembly](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
