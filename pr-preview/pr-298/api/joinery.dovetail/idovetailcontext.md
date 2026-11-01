# IDovetailContext

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.md)
- Version: `0.4.2-pr.3`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

The base context marker interface to define this as a context

## Definition

```csharp
namespace Joinery.Dovetail;

public interface IDovetailContext
{
    // ...
}
```

## Properties

- [Assembly](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of joints the context loads.
- [HostType](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/pr-preview/pr-298/api/joinery.dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
