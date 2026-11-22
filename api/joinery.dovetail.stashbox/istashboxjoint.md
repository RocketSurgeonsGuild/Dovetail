# IStashboxJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Stashbox](/Dovetail/api/joinery.dovetail.stashbox.md)
- Version: `0.8.2-beta.11`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.2281acdd6f0011b26520551395637e54409e48e8/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/IStashboxJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IStashboxJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IStashboxContainer)](/Dovetail/api/joinery.dovetail.stashbox/istashboxjoint/methods.md#register-idovetailcontext-istashboxcontainer) `abstract` -- Register the Stashbox part.

## See Also

- `Infrastructure.IDovetailJoint`
