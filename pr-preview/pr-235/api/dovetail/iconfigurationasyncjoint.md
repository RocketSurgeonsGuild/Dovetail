# IConfigurationAsyncJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface IConfigurationAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IConfigurationBuilder, CancellationToken)](/Dovetail/pr-preview/pr-235/api/dovetail/iconfigurationasyncjoint/methods.md#register-idovetailcontext-iconfigurationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Configuration part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)
