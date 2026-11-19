# DovetailContext

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/DovetailContext.cs)
- Implements: [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)

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

- [Categories](/Dovetail/api/joinery.dovetail/dovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get; set` -- The categories of joints the context loads.
- [HostType](/Dovetail/api/joinery.dovetail/dovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md) `get` -- The host type
- [Joints](/Dovetail/api/joinery.dovetail/dovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `get` -- Get the conventions from the context

## Methods

- [FromAsync(DovetailContextBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailcontext/methods.md#fromasync-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IDovetailContext>` `static` -- Create a context from a given builder

## See Also

- [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)
