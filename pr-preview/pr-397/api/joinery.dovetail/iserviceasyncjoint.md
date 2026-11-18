# IServiceAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/obj/Release/net10.0/IServiceAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IServiceCollection, CancellationToken)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/iserviceasyncjoint/methods.md#register-idovetailcontext-iservicecollection-cancellationtoken) : `ValueTask` `abstract` -- Register the Service part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)
