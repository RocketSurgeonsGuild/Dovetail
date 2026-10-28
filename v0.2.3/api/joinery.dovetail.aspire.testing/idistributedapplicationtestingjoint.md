# IDistributedApplicationTestingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public interface IDistributedApplicationTestingJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder)](/Dovetail/v0.2.3/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder) `abstract` -- Register the DistributedApplicationTesting part.

## See Also

- `Infrastructure.IDovetailJoint`
