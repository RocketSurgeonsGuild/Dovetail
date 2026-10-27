# IDistributedApplicationTestingAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-228/api/dovetail.aspire.testing.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public interface IDistributedApplicationTestingAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)](/Dovetail/pr-preview/pr-228/api/dovetail.aspire.testing/idistributedapplicationtestingasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationTesting part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
