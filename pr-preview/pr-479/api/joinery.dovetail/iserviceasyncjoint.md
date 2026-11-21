# IServiceAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/obj/Release/net10.0/IServiceAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/iserviceasyncjoint/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` `abstract` -- Register the Service part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)
