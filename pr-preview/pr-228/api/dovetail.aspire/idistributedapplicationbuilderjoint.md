# IDistributedApplicationBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-228/api/dovetail.aspire.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDistributedApplicationBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder)](/Dovetail/pr-preview/pr-228/api/dovetail.aspire/idistributedapplicationbuilderjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder) `abstract` -- Register the DistributedApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
