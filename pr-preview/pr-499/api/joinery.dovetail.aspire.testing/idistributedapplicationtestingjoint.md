# IDistributedApplicationTestingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.2-pr.29`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail.Aspire.Testing/IDistributedApplicationTestingJoint.cs)
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

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder)](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder) `abstract` -- Register the DistributedApplicationTesting part.

## See Also

- `Infrastructure.IDovetailJoint`
