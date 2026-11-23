# AutofacAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.1faa3d8c1fb998da38c062158c5a17b8390b25fa/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/AutofacAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the Autofac part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask AutofacAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Autofac.ContainerBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [AutofacAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.autofac/autofacasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ContainerBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.autofac/autofacasyncjointdelegate/methods.md#begininvoke-idovetailcontext-containerbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.autofac/autofacasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
