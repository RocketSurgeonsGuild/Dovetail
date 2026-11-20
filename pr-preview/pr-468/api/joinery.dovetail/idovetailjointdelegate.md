# IDovetailJointDelegate

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.md)
- Version: `0.8.1-pr.11`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-468-merge.Sha.1950047ba9b0769796f6cf96621cfafcd184f8e2/src/Joinery.Dovetail/Infrastructure/IDovetailJointDelegate.cs)

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

- [Assembly](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/idovetailjointdelegate/properties.md#assembly) : `Assembly` `abstract` `get` -- The assembly of the joint delegate
- [Expression](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/idovetailjointdelegate/properties.md#expression) : `string` `abstract` `get` -- The expression of the joint delegate
