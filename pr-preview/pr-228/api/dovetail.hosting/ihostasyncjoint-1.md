# IHostAsyncJoint<THost>

- Kind: `interface`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostAsyncJoint<THost>
    : Dovetail.Infrastructure.IDovetailJoint
    where THost : IHost
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost, CancellationToken)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting/ihostasyncjoint-1/methods.md#register-idovetailcontext-thost-cancellationtoken) : `ValueTask` `abstract` -- Register the Host part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
