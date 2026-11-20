# IAutofacJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.autofac.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail.Autofac/obj/Release/net10.0/IAutofacJoint.cs)
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

- [Register(IDovetailContext, ContainerBuilder)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.autofac/iautofacjoint/methods.md#register-idovetailcontext-containerbuilder) `abstract` -- Register the Autofac part.

## See Also

- `Infrastructure.IDovetailJoint`
