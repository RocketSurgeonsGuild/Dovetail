# IStashboxAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Stashbox](/Dovetail/api/dovetail.stashbox.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IStashboxAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IStashboxContainer, CancellationToken)](/Dovetail/api/dovetail.stashbox/istashboxasyncjoint/methods.md#register-idovetailcontext-istashboxcontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the Stashbox part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
