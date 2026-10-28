# DovetailDistributedApplicationTestingHelpers

- Kind: `class`
- Package: [Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-240/api/dovetail.aspire.testing.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`

Shared helpers used to wire an `Testing.IDistributedApplicationTestingBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public static class DovetailDistributedApplicationTestingHelpers
{
    // ...
}
```

## Methods

- [Configure(IDistributedApplicationTestingBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-240/api/dovetail.aspire.testing/dovetaildistributedapplicationtestinghelpers/methods.md#configure-idistributedapplicationtestingbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application test host.
