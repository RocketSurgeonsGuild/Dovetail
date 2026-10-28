# WebAssemblyHostJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.webassembly.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the WebAssemblyHost part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void WebAssemblyHostJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost host)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`WebAssemblyHost`)

## Constructors

- [WebAssemblyHostJointDelegate(object, nint)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.webassembly/webassemblyhostjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHost, AsyncCallback, object)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.webassembly/webassemblyhostjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.webassembly/webassemblyhostjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
