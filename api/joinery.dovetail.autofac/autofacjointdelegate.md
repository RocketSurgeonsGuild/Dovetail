# AutofacJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-beta.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/AutofacJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Autofac part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void AutofacJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Autofac.ContainerBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)

## Constructors

- [AutofacJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.autofac/autofacjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ContainerBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.autofac/autofacjointdelegate/methods.md#begininvoke-idovetailcontext-containerbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.autofac/autofacjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
