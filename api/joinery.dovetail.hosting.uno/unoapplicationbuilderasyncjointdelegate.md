# UnoApplicationBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-beta.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the UnoApplicationBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask UnoApplicationBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Uno.Extensions.Hosting.IApplicationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [UnoApplicationBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IApplicationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iapplicationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
