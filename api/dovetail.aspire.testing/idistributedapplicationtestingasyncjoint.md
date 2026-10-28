# IDistributedApplicationTestingAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Aspire.Testing](/Dovetail/api/dovetail.aspire.testing.md)
- Version: `0.2.3-beta.1`
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

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)](/Dovetail/api/dovetail.aspire.testing/idistributedapplicationtestingasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationTesting part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
