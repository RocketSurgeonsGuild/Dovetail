# IDryIocJoint

- Kind: `interface`
- Package: [Dovetail.DryIoc](/Dovetail/pr-preview/pr-240/api/dovetail.dryioc.md)
- Version: `0.2.3-pr.3`
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

- [Register(IDovetailContext, IContainer)](/Dovetail/pr-preview/pr-240/api/dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
