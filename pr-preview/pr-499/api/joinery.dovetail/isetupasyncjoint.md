# ISetupAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail/obj/Release/net10.0/ISetupAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ISetupAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/isetupasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Setup part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-499/api/joinery.dovetail/idovetailjoint.md)
