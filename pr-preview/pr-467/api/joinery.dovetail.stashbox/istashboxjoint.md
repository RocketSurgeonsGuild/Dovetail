# IStashboxJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-467/api/joinery.dovetail.stashbox.md)
- Version: `0.8.1-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-467-merge.Sha.d99fa32788fc6ba98c72987996ae5cba72e8fc58/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/IStashboxJoint.cs)
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

- [Register(IDovetailContext, IStashboxContainer)](/Dovetail/pr-preview/pr-467/api/joinery.dovetail.stashbox/istashboxjoint/methods.md#register-idovetailcontext-istashboxcontainer) `abstract` -- Register the Stashbox part.

## See Also

- `Infrastructure.IDovetailJoint`
