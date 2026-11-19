# IDryIocAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/IDryIocAsyncJoint.cs)
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

- [Register(IDovetailContext, IContainer, CancellationToken)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.dryioc/idryiocasyncjoint/methods.md#register-idovetailcontext-icontainer-cancellationtoken) : `ValueTask` `abstract` -- Register the DryIoc part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
