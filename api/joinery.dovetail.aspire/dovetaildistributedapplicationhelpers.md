# DovetailDistributedApplicationHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-beta.19`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d3d86de99d09b379bd09cfaa27e848e7aa21ab62/src/Joinery.Dovetail.Aspire/RocketDistributedApplicationExtensions.cs)

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

- [Configure(IDistributedApplicationBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.aspire/dovetaildistributedapplicationhelpers/methods.md#configure-idistributedapplicationbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application host.
