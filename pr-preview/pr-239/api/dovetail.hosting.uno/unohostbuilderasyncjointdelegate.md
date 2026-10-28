# UnoHostBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.uno.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the UnoHostBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask UnoHostBuilderAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Hosting.IHostBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [UnoHostBuilderAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.uno/unohostbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IHostBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.uno/unohostbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-ihostbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.uno/unohostbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
