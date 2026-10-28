# IDistributedApplicationJoint

- Kind: `interface`
- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-235/api/dovetail.aspire.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDistributedApplicationJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, DistributedApplication)](/Dovetail/pr-preview/pr-235/api/dovetail.aspire/idistributedapplicationjoint/methods.md#register-idovetailcontext-distributedapplication) `abstract` -- Register the DistributedApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
