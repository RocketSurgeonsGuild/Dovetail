# ILoggingAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/obj/Release/net10.0/ILoggingAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, ILoggingBuilder, CancellationToken)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/iloggingasyncjoint/methods.md#register-idovetailcontext-iloggingbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Logging part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)
