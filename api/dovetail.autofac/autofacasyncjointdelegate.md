# AutofacAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Autofac](/Dovetail/api/dovetail.autofac.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Autofac part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask AutofacAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Autofac.ContainerBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [AutofacAsyncJointDelegate(object, nint)](/Dovetail/api/dovetail.autofac/autofacasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, ContainerBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/dovetail.autofac/autofacasyncjointdelegate/methods.md#begininvoke-idovetailcontext-containerbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.autofac/autofacasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
