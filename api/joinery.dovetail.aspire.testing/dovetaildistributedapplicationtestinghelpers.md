# DovetailDistributedApplicationTestingHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.2-beta.17`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.6690600a1d8dc3e5d18fb6e69136cd6b75569d71/src/Joinery.Dovetail.Aspire.Testing/RocketDistributedApplicationTestingExtensions.cs)

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
