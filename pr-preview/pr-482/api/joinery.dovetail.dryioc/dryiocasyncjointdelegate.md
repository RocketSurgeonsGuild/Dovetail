# DryIocAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocAsyncJointDelegate.cs)
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

- [DryIocAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
