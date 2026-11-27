# IAutofacJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IAutofacJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ContainerBuilder)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.autofac/iautofacjoint/methods.md#register-idovetailcontext-containerbuilder) `abstract` -- Register the Autofac part.

## See Also

- `Infrastructure.IDovetailJoint`
