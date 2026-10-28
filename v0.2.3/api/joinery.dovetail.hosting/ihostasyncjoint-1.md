# IHostAsyncJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostAsyncJoint<THost>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
    where THost : IHost
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost, CancellationToken)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/ihostasyncjoint-1/methods.md#register-idovetailcontext-thost-cancellationtoken) : `ValueTask` `abstract` -- Register the Host part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
