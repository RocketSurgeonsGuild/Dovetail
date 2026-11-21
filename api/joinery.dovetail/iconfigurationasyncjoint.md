# IConfigurationAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.2-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail/obj/Release/net10.0/IConfigurationAsyncJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IConfigurationBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail/iconfigurationasyncjoint/methods.md#register-idovetailcontext-iconfigurationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the Configuration part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)
