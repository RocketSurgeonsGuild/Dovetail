# ILoggingAsyncJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface ILoggingAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ILoggingBuilder, CancellationToken)](/Dovetail/pr-preview/pr-240/api/dovetail/iloggingasyncjoint/methods.md#register-idovetailcontext-iloggingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Logging part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)
