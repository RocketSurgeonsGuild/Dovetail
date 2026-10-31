# IDistributedApplicationBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/v0.4.0/api/joinery.dovetail.aspire.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder, CancellationToken)](/Dovetail/v0.4.0/api/joinery.dovetail.aspire/idistributedapplicationbuilderasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
