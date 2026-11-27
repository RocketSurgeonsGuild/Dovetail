# DovetailDistributedApplicationHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail.Aspire/RocketDistributedApplicationExtensions.cs)

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

- [Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire/dovetaildistributedapplicationhelpers/methods.md#configure-idistributedapplicationbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application host.
