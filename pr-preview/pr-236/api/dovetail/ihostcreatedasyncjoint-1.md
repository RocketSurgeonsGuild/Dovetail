# IHostCreatedAsyncJoint<THost>

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-236/api/dovetail/idovetailjoint.md)

Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-236/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostCreatedAsyncJoint<THost>
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost, CancellationToken)](/Dovetail/pr-preview/pr-236/api/dovetail/ihostcreatedasyncjoint-1/methods.md#register-idovetailcontext-thost-cancellationtoken) : `ValueTask` `abstract` -- Register the HostCreated part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-236/api/dovetail/idovetailjoint.md)
