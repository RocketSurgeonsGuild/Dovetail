# ISetupJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext)](/Dovetail/api/dovetail/isetupjoint/methods.md#register-idovetailcontext) `abstract` -- Register the Setup part.

## See Also

- [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)
