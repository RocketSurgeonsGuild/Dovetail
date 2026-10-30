# StashboxAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.stashbox.md)
- Version: `0.3.1-pr.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Stashbox part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask StashboxAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Stashbox.IStashboxContainer container,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [StashboxAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
