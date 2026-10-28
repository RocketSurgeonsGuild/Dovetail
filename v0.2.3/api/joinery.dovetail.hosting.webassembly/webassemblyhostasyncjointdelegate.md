# WebAssemblyHostAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the WebAssemblyHost part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask WebAssemblyHostAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`WebAssemblyHost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [WebAssemblyHostAsyncJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHost, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
