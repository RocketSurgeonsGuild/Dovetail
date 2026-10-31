# ISetupAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, CancellationToken)](/Dovetail/v0.4.0/api/joinery.dovetail/isetupasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Setup part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)
