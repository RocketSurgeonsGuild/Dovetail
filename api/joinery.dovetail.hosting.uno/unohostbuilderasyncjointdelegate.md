# UnoHostBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-beta.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the UnoHostBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask UnoHostBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Hosting.IHostBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [UnoHostBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IHostBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-ihostbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.uno/unohostbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
