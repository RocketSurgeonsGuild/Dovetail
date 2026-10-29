# DovetailDistributedApplicationHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-259/api/joinery.dovetail.aspire.md)
- Version: `0.3.1-pr.2`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Shared helpers used to wire an `Hosting.IDistributedApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailDistributedApplicationHelpers
{
    // ...
}
```

## Methods

- [Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-259/api/joinery.dovetail.aspire/dovetaildistributedapplicationhelpers/methods.md#configure-idistributedapplicationbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application host.
