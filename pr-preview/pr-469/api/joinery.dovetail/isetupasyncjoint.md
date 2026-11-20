# ISetupAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/obj/Release/net10.0/ISetupAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/isetupasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Setup part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)
