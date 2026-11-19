# ISetupJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail/obj/Release/net10.0/ISetupJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ISetupJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/isetupjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Setup part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-465/api/joinery.dovetail/idovetailjoint.md)
