# DovetailDistributedApplicationTestingHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/api/joinery.dovetail.aspire.testing.md)
- Version: `0.4.1-beta.2`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`

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

- [Configure(IDistributedApplicationTestingBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.aspire.testing/dovetaildistributedapplicationtestinghelpers/methods.md#configure-idistributedapplicationtestingbuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<DistributedApplication>` `static` -- Populates the context, applies shared configuration, and builds the distributed application test host.
