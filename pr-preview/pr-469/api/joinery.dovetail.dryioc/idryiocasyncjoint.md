# IDryIocAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.dryioc.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/IDryIocAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDryIocAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IContainer, CancellationToken)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.dryioc/idryiocasyncjoint/methods.md#register-idovetailcontext-icontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the DryIoc part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
