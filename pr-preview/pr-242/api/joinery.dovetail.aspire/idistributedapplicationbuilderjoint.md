# IDistributedApplicationBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.aspire.md)
- Version: `0.2.3-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.aspire/idistributedapplicationbuilderjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder) `abstract` -- Register the DistributedApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
