# DovetailWebAssemblyHelpers

- Kind: `class`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.webassembly.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Shared helpers used to wire a `Hosting.WebAssemblyHostBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Dovetail;

public static class DovetailWebAssemblyHelpers
{
    // ...
}
```

## Methods

- [Configure(WebAssemblyHostBuilder, Func<WebAssemblyHostBuilder, WebAssemblyHost>, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.webassembly/dovetailwebassemblyhelpers/methods.md#configure-webassemblyhostbuilder-func-webassemblyhostbuilder-webassemblyhost-dovetailcontextbuilder-cancellationtoken) : `ValueTask<WebAssemblyHost>` `extension` -- Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the host.
