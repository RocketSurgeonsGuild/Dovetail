# StashboxJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-beta.11`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.2281acdd6f0011b26520551395637e54409e48e8/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/StashboxJointDelegate.cs)
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

- [StashboxJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.stashbox/stashboxjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.stashbox/stashboxjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.stashbox/stashboxjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
