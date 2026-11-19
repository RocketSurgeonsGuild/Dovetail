# DryIocJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.dryioc.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocJointDelegate.cs)
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

- [DryIocJointDelegate(object, nint)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.dryioc/dryiocjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.dryioc/dryiocjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
