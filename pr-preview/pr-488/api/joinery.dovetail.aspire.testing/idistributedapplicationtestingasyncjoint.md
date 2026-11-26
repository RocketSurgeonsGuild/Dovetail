# IDistributedApplicationTestingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail.Aspire.Testing/IDistributedApplicationTestingAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public interface IDistributedApplicationTestingAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationTesting part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
