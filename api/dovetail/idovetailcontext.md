# IDovetailContext

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.3-beta.1`
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

- [Assembly](/Dovetail/api/dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/api/dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of the convention context
- [HostType](/Dovetail/api/dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/api/dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/api/dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
