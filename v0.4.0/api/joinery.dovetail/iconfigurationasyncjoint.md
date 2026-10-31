# IConfigurationAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IConfigurationAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IConfigurationBuilder, CancellationToken)](/Dovetail/v0.4.0/api/joinery.dovetail/iconfigurationasyncjoint/methods.md#register-idovetailcontext-iconfigurationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Configuration part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)
