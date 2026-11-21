# IConfigurationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail/obj/Release/net10.0/IConfigurationJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IConfigurationJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IConfigurationBuilder)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/iconfigurationjoint/methods.md#register-idovetailcontext-iconfigurationbuilder) `abstract` -- Register the Configuration part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailjoint.md)
