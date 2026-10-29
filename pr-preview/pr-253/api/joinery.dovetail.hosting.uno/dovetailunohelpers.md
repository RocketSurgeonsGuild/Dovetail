# DovetailUnoHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-pr.8`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Shared helpers used to wire an Uno.Extensions `Hosting.IApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailUnoHelpers
{
    // ...
}
```

## Methods

- [Configure(IApplicationBuilder, Func<IApplicationBuilder, IHost>, Assembly, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/dovetailunohelpers/methods.md#configure-iapplicationbuilder-func-iapplicationbuilder-ihost-assembly-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IHost>` `extension` -- Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the application's host, and builds it.
- [Navigate(IApplicationBuilder, Assembly, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/dovetailunohelpers/methods.md#navigate-iapplicationbuilder-assembly-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IHost>` `extension` -- Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the application's host, then builds it and navigates to `TShell` (Uno.Extensions.Navigation).
