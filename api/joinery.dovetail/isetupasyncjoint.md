# ISetupAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail/obj/Release/net10.0/ISetupAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, CancellationToken)](/Dovetail/api/joinery.dovetail/isetupasyncjoint/methods.md#register-idovetailcontext-cancellationtoken) : `ValueTask` `abstract` -- Register the Setup part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)
