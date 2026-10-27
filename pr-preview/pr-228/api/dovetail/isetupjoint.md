# ISetupJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface ISetupJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext)](/Dovetail/pr-preview/pr-228/api/dovetail/isetupjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Setup part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)
