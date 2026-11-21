# ServiceAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-475/api/joinery.dovetail.md)
- Version: `0.8.2-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-475-merge.Sha.48c3c562fb368924fed28a88e87a1b7d995d98d0/src/Joinery.Dovetail/obj/Release/net10.0/ServiceAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the Service part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask ServiceAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.DependencyInjection.IServiceCollection services,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-475/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [ServiceAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-475/api/joinery.dovetail/serviceasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IServiceCollection, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-475/api/joinery.dovetail/serviceasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iservicecollection-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-475/api/joinery.dovetail/serviceasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
