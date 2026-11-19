# StashboxAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.stashbox.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/StashboxAsyncJointDelegate.cs)
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

- [StashboxAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
