# IStashboxAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/IStashboxAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IStashboxAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IStashboxContainer, CancellationToken)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.stashbox/istashboxasyncjoint/methods.md#register-idovetailcontext-istashboxcontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the Stashbox part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
