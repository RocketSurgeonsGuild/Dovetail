# AutofacJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Autofac](/Dovetail/api/dovetail.autofac.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Autofac part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void AutofacJointDelegate(
    Dovetail.IDovetailContext context,
    Autofac.ContainerBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)

## Constructors

- [AutofacJointDelegate(object, nint)](/Dovetail/api/dovetail.autofac/autofacjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ContainerBuilder, AsyncCallback, object)](/Dovetail/api/dovetail.autofac/autofacjointdelegate/methods.md#begininvoke-idovetailcontext-containerbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.autofac/autofacjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
