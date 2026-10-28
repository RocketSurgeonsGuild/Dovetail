# DovetailDistributedApplicationHelpers

- Kind: `class`
- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-237/api/dovetail.aspire.md)
- Version: `0.2.2-pr.3`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Shared helpers used to wire an `Hosting.IDistributedApplicationBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Dovetail;

public static class DovetailDistributedApplicationHelpers
{
    // ...
}
```

## Methods

- [Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-237/api/dovetail.aspire/dovetaildistributedapplicationhelpers/methods.md#configure-idistributedapplicationbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application host.
