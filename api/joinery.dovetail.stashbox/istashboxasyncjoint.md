# IStashboxAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/api/joinery.dovetail.stashbox.md)
- Version: `0.2.4-beta.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
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

- [Register(IDovetailContext, IStashboxContainer, CancellationToken)](/Dovetail/api/joinery.dovetail.stashbox/istashboxasyncjoint/methods.md#register-idovetailcontext-istashboxcontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the Stashbox part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
