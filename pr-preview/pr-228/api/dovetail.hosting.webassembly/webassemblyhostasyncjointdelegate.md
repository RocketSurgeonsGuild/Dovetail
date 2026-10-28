# WebAssemblyHostAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the WebAssemblyHost part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask WebAssemblyHostAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost host,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`WebAssemblyHost`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [WebAssemblyHostAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHost, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhost-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
