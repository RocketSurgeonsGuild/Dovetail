# StashboxJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Stashbox](/Dovetail/v0.2.2/api/dovetail.stashbox.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Stashbox part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void StashboxJointDelegate(
    Dovetail.IDovetailContext context,
    Stashbox.IStashboxContainer container)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)

## Constructors

- [StashboxJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IStashboxContainer, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointdelegate/methods.md#begininvoke-idovetailcontext-istashboxcontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
