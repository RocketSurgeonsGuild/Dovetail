# UnoApplicationBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the UnoApplicationBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask UnoApplicationBuilderAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Uno.Extensions.Hosting.IApplicationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [UnoApplicationBuilderAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IApplicationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iapplicationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno/unoapplicationbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
