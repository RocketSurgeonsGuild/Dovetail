# WebAssemblyHostBuilderAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/api/dovetail.hosting.webassembly.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the WebAssemblyHostBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask WebAssemblyHostBuilderAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [WebAssemblyHostBuilderAsyncJointDelegate(object, nint)](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhostbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.hosting.webassembly/webassemblyhostbuilderasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
