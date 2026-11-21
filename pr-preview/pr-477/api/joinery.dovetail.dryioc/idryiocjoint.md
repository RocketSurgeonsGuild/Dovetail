# IDryIocJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-477/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-477-merge.Sha.36056a013c51cd3b8c842fdef8810ae3835a17f4/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/IDryIocJoint.cs)
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

- [Register(IDovetailContext, IContainer)](/Dovetail/pr-preview/pr-477/api/joinery.dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
