# IStashboxAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/IStashboxAsyncJoint.cs)
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

- [Register(IDovetailContext, IStashboxContainer, CancellationToken)](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox/istashboxasyncjoint/methods.md#register-idovetailcontext-istashboxcontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the Stashbox part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
