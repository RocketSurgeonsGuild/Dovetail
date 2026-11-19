# DovetailDistributedApplicationTestingHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.aspire.testing.md)
- Version: `0.7.2-pr.11`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.a1990fe6695c5fa9e9c003a3a333f8ff361aa602/src/Joinery.Dovetail.Aspire.Testing/RocketDistributedApplicationTestingExtensions.cs)

Shared helpers used to wire an `Testing.IDistributedApplicationTestingBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public static class DovetailDistributedApplicationTestingHelpers
{
    // ...
}
```

## Methods

- [Configure(IDistributedApplicationTestingBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.aspire.testing/dovetaildistributedapplicationtestinghelpers/methods.md#configure-idistributedapplicationtestingbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application test host.
