# IDryIocJoint

- Kind: `interface`
- Package: [Dovetail.DryIoc](/Dovetail/api/dovetail.dryioc.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDryIocJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IContainer)](/Dovetail/api/dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
