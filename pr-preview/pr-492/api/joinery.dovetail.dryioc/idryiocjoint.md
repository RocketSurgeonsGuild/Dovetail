# IDryIocJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/IDryIocJoint.cs)
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

- [Register(IDovetailContext, IContainer)](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
