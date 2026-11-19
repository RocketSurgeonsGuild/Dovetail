# DryIocAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/v0.8.0/api/joinery.dovetail.dryioc.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DryIocAsyncJointDelegate.cs)
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

- [DryIocAsyncJointDelegate(object, nint)](/Dovetail/v0.8.0/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, CancellationToken, AsyncCallback, object)](/Dovetail/v0.8.0/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.8.0/api/joinery.dovetail.dryioc/dryiocasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
