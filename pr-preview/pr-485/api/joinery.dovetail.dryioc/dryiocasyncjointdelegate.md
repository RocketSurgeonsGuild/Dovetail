# DryIocAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-pr.10`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the DryIoc part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DryIocAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    DryIoc.IContainer container,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DryIocAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
