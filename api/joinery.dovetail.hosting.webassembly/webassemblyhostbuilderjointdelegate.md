# WebAssemblyHostBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.3-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the WebAssemblyHostBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void WebAssemblyHostBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)

## Constructors

- [WebAssemblyHostBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, WebAssemblyHostBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-webassemblyhostbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.webassembly/webassemblyhostbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
