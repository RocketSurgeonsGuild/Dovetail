# IDryIocJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-289/api/joinery.dovetail.dryioc.md)
- Version: `0.3.2-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDryIocJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IContainer)](/Dovetail/pr-preview/pr-289/api/joinery.dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
