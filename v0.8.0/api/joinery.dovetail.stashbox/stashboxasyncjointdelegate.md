# StashboxAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/StashboxAsyncJointDelegate.cs)
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

- [StashboxAsyncJointDelegate(object, nint)](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.8.0/api/joinery.dovetail.stashbox/stashboxasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
