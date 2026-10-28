# IDovetailContext

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-234/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail`
- Target framework: `net10.0`

The base context marker interface to define this as a context

## Definition

```csharp
namespace Dovetail;

public interface IDovetailContext
{
    // ...
}
```

## Properties

- [Assembly](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of the convention context
- [HostType](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/pr-preview/pr-234/api/dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
