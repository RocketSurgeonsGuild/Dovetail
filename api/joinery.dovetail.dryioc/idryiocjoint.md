# IDryIocJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/api/joinery.dovetail.dryioc.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/IDryIocJoint.cs)
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

- [Register(IDovetailContext, IContainer)](/Dovetail/api/joinery.dovetail.dryioc/idryiocjoint/methods.md#register-idovetailcontext-icontainer) `abstract` -- Register the DryIoc part.

## See Also

- `Infrastructure.IDovetailJoint`
