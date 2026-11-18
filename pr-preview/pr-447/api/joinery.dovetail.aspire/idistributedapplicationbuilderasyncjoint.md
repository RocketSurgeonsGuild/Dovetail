# IDistributedApplicationBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.aspire.md)
- Version: `0.7.2-pr.13`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-447-merge.Sha.07e64d25a30d87fc48248bbdc4924e0013be133c/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/IDistributedApplicationBuilderAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder, CancellationToken)](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.aspire/idistributedapplicationbuilderasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
