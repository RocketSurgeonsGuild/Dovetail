# DryIocAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.dryioc.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the DryIoc part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask DryIocAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    DryIoc.IContainer container,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [DryIocAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
