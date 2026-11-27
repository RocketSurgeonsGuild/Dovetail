# AutofacJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/AutofacJointDelegate.cs)
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

- [AutofacJointDelegate(object, nint)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac/autofacjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ContainerBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac/autofacjointdelegate/methods.md#begininvoke-idovetailcontext-containerbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac/autofacjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
