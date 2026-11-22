# DovetailContext

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.0dbc318ad08f7f5b82ee2d67aaffd22e5ac9b9a1/src/Joinery.Dovetail/DovetailContext.cs)
- Implements: [IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md)

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md)

## Definition

```csharp
namespace Joinery.Dovetail;

public sealed class DovetailContext
    : Joinery.Dovetail.IDovetailContext
{
    // ...
}
```

## Properties

- [Categories](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get; set` -- The categories of joints the context loads.
- [HostType](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailhosttype.md) `get` -- The host type
- [Joints](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `get` -- Get the conventions from the context

## Methods

- [FromAsync(DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailcontext/methods.md#fromasync-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IDovetailContext>` `static` -- Create a context from a given builder

## See Also

- [IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md)
