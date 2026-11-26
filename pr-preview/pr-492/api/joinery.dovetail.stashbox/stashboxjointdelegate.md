# StashboxJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/StashboxJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Stashbox part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void StashboxJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Stashbox.IStashboxContainer container)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)

## Constructors

- [StashboxJointDelegate(object, nint)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.stashbox/stashboxjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, AsyncCallback, object)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.stashbox/stashboxjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.stashbox/stashboxjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
