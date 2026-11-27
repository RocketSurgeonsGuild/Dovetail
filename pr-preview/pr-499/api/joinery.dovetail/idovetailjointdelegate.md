# IDovetailJointDelegate

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail/Infrastructure/IDovetailJointDelegate.cs)

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

- [Assembly](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
