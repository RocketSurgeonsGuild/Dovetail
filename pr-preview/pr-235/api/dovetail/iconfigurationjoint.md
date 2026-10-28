# IConfigurationJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface IConfigurationJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IConfigurationBuilder)](/Dovetail/pr-preview/pr-235/api/dovetail/iconfigurationjoint/methods.md#register-idovetailcontext-iconfigurationbuilder) `abstract` -- Register the Configuration part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)
