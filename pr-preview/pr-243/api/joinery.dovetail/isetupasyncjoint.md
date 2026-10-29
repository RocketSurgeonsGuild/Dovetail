# ISetupAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-243/api/joinery.dovetail.md)
- Version: `0.3.1-pr.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, CancellationToken)](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/isetupasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Setup part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-243/api/joinery.dovetail/idovetailjoint.md)
