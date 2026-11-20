# IServiceAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-473-merge.Sha.c55e59e58abb6483addadae4101ca132a19fd392/src/Joinery.Dovetail/obj/Release/net10.0/IServiceAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IServiceAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/iserviceasyncjoint/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` `abstract` -- Register the Service part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)
