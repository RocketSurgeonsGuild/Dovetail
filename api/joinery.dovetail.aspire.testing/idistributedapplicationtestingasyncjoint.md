# IDistributedApplicationTestingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.1-beta.9`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail.Aspire.Testing/IDistributedApplicationTestingAsyncJoint.cs)
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

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationTesting part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
