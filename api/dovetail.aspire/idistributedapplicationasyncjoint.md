# IDistributedApplicationAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Aspire](/Dovetail/api/dovetail.aspire.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDistributedApplicationAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, DistributedApplication, CancellationToken)](/Dovetail/api/dovetail.aspire/idistributedapplicationasyncjoint/methods.md#register-idovetailcontext-distributedapplication-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
