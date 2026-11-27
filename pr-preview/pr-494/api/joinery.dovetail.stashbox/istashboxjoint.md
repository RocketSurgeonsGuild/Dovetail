# IStashboxJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-494/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-494-merge.Sha.60ec96c618c8e073441c11142954fbb8c2ec3264/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/IStashboxJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IStashboxJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IStashboxContainer)](/Dovetail/pr-preview/pr-494/api/joinery.dovetail.stashbox/istashboxjoint/methods.md#register-idovetailcontext-istashboxcontainer) `abstract` -- Register the Stashbox part.

## See Also

- `Infrastructure.IDovetailJoint`
