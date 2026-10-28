# WebAssemblyHostBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the WebAssemblyHostBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void WebAssemblyHostBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)

## Constructors

- [WebAssemblyHostBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhostbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
