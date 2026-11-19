# IDovetailContext

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/IDovetailContext.cs)

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

- [Assembly](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext/properties.md#assembly) : `Assembly` `virtual` `get` -- The assembly that is executing the conventions
- [Categories](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `abstract` `get` -- The categories of joints the context loads.
- [HostType](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext/properties.md#hosttype) : [DovetailHostType](/Dovetail/v0.8.0/api/joinery.dovetail/dovetailhosttype.md) `abstract` `get` -- The underlying host type
- [Joints](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext/properties.md#joints) : `ImmutableList<IDovetailJoint>` `abstract` `get` -- Get the conventions from the context
