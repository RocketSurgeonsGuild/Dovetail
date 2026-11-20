# IConfigurationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail/obj/Release/net10.0/IConfigurationJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IConfigurationBuilder)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/iconfigurationjoint/methods.md#register-idovetailcontext-iconfigurationbuilder) `abstract` -- Register the Configuration part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)
