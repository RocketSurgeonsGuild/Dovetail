# IDovetailContext

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/IDovetailContext.cs)

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

- [Assembly](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of joints the context loads.
- [HostType](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
