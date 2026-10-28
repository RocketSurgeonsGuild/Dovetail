# IDistributedApplicationBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Aspire](/Dovetail/api/dovetail.aspire.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IDistributedApplicationBuilderAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder, CancellationToken)](/Dovetail/api/dovetail.aspire/idistributedapplicationbuilderasyncjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the DistributedApplicationBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
