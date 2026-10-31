# IHostCreatedAsyncJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-293/api/joinery.dovetail.md)
- Version: `0.4.1-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-293/api/joinery.dovetail/idovetailjoint.md)

Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-293/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostCreatedAsyncJoint<THost>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost, CancellationToken)](/Dovetail/pr-preview/pr-293/api/joinery.dovetail/ihostcreatedasyncjoint-1/methods.md#register-idovetailcontext-thost-cancellationtoken) : `ValueTask` `abstract` -- Register the HostCreated part asynchronously.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-293/api/joinery.dovetail/idovetailjoint.md)
