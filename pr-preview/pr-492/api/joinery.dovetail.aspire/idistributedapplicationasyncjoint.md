# IDistributedApplicationAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/IDistributedApplicationAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, DistributedApplication, CancellationToken)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.aspire/idistributedapplicationasyncjoint/methods.md#register-idovetailcontext-distributedapplication-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
