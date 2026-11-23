# IDovetailContext

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail/IDovetailContext.cs)

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

- [Assembly](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of joints the context loads.
- [HostType](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
