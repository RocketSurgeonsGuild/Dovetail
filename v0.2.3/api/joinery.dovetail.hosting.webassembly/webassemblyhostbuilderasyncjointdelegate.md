# WebAssemblyHostBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the WebAssemblyHostBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask WebAssemblyHostBuilderAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [WebAssemblyHostBuilderAsyncJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhostbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
