# ILoggingAsyncJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, ILoggingBuilder, CancellationToken)](/Dovetail/v0.2.2/api/dovetail/iloggingasyncjoint/methods.md#register-idovetailcontext-iloggingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Logging part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
