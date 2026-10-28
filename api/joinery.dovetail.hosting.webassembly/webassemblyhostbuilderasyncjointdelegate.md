# WebAssemblyHostBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.4-beta.5`
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

- [WebAssemblyHostBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhostbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
