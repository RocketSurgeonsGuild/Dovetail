# DryIocJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocJointDelegate.cs)
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

- [DryIocJointDelegate(object, nint)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc/dryiocjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
