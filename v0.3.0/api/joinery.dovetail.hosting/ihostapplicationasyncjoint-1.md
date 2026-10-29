# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.3.0/api/joinery.dovetail.hosting.md)
- Version: `0.3.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostApplicationAsyncJoint<TBuilder>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/v0.3.0/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
