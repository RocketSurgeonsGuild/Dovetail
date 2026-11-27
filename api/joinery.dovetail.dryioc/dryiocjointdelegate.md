# DryIocJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-beta.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the DryIoc part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void DryIocJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    DryIoc.IContainer container)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)

## Constructors

- [DryIocJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.dryioc/dryiocjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
