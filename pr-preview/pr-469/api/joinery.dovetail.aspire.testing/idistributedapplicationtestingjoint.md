# IDistributedApplicationTestingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.aspire.testing.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail.Aspire.Testing/IDistributedApplicationTestingJoint.cs)
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

- [Register(IDovetailContext, IDistributedApplicationTestingBuilder)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingjoint/methods.md#register-idovetailcontext-idistributedapplicationtestingbuilder) `abstract` -- Register the DistributedApplicationTesting part.

## See Also

- `Infrastructure.IDovetailJoint`
