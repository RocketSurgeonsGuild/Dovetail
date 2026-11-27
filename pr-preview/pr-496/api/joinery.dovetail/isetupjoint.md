# ISetupJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.md)
- Version: `0.8.2-pr.19`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail/obj/Release/net10.0/ISetupJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/isetupjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Setup part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/idovetailjoint.md)
