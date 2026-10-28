# IDistributedApplicationTestingJoint

- Kind: `interface`
- Package: [Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-236/api/dovetail.aspire.testing.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public interface IDistributedApplicationTestingJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder)](/Dovetail/pr-preview/pr-236/api/dovetail.aspire.testing/idistributedapplicationtestingjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder) `abstract` -- Register the DistributedApplicationTesting part.

## See Also

- `Infrastructure.IDovetailJoint`
