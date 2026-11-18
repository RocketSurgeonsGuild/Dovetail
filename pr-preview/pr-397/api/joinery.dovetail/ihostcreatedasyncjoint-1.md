# IHostCreatedAsyncJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/obj/Release/net10.0/IHostCreatedAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostCreatedAsyncJoint<THost>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost, CancellationToken)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/ihostcreatedasyncjoint-1/methods.md#register-idovetailcontext-thost-cancellationtoken) : `ValueTask` `abstract` -- Register the HostCreated part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)
