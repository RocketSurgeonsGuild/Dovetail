# DryIocJointDelegate

- Kind: `delegate`
- Package: [Dovetail.DryIoc](/Dovetail/api/dovetail.dryioc.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the DryIoc part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void DryIocJointDelegate(
    Dovetail.IDovetailContext context,
    DryIoc.IContainer container)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)

## Constructors

- [DryIocJointDelegate(object, nint)](/Dovetail/api/dovetail.dryioc/dryiocjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IContainer, AsyncCallback, object)](/Dovetail/api/dovetail.dryioc/dryiocjointdelegate/methods.md#begininvoke-idovetailcontext-icontainer-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.dryioc/dryiocjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
