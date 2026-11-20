# ILoggingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-473/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-473-merge.Sha.c55e59e58abb6483addadae4101ca132a19fd392/src/Joinery.Dovetail/obj/Release/net10.0/ILoggingAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ILoggingAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ILoggingBuilder, CancellationToken)](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/iloggingasyncjoint/methods.md#register-idovetailcontext-iloggingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Logging part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-473/api/joinery.dovetail/idovetailjoint.md)
