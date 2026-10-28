# DovetailHostApplicationHelpers

- Kind: `class`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.md)
- Version: `0.2.2-pr.3`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an `Hosting.IHostApplicationBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Dovetail;

public static class DovetailHostApplicationHelpers
{
    // ...
}
```

## Methods

- [Configure(T, Func<T, THost>, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-238/api/dovetail.hosting/dovetailhostapplicationhelpers/methods.md#configure-t-func-t-thost-dovetailcontextbuilder-cancellationtoken) : `ValueTask<THost>` `static` -- Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the host.
