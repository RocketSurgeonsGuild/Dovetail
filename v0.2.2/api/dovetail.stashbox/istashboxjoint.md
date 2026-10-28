# IStashboxJoint

- Kind: `interface`
- Package: [Dovetail.Stashbox](/Dovetail/v0.2.2/api/dovetail.stashbox.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IStashboxJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IStashboxContainer)](/Dovetail/v0.2.2/api/dovetail.stashbox/istashboxjoint/methods.md#register-idovetailcontext-istashboxcontainer) `abstract` -- Register the Stashbox part.

## See Also

- `Infrastructure.IDovetailJoint`
