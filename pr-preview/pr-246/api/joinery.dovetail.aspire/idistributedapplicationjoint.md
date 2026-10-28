# IDistributedApplicationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.aspire.md)
- Version: `0.2.4-pr.10`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, DistributedApplication)](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.aspire/idistributedapplicationjoint/methods.md#register-idovetailcontext-distributedapplication) `abstract` -- Register the DistributedApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
