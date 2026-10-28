# DovetailContext

- Kind: `class`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail`
- Target framework: `net10.0`
- Implements: [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md)

Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md)

## Definition

```csharp
namespace Dovetail;

public sealed class DovetailContext
    : Dovetail.IDovetailContext
{
    // ...
}
```

## Properties

- [Categories](/Dovetail/v0.2.2/api/dovetail/dovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get; set` -- The categories of the convention context
- [HostType](/Dovetail/v0.2.2/api/dovetail/dovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/v0.2.2/api/dovetail/dovetailhosttype.md) `get` -- The host type
- [Joints](/Dovetail/v0.2.2/api/dovetail/dovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `get` -- Get the conventions from the context

## Methods

- [FromAsync(DovetailContextBuilder, CancellationToken)](/Dovetail/v0.2.2/api/dovetail/dovetailcontext/methods.md#fromasync-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IDovetailContext>` `static` -- Create a context from a given builder

## See Also

- [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md)
