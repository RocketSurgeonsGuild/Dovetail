# StashboxAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Stashbox](/Dovetail/pr-preview/pr-228/api/dovetail.stashbox.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Stashbox part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask StashboxAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Stashbox.IStashboxContainer container,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [StashboxAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-228/api/dovetail.stashbox/stashboxasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-228/api/dovetail.stashbox/stashboxasyncjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-228/api/dovetail.stashbox/stashboxasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
