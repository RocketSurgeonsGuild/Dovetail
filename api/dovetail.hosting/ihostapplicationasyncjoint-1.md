# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Dovetail.Hosting](/Dovetail/api/dovetail.hosting.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostApplicationAsyncJoint<TBuilder>
    : Dovetail.Infrastructure.IDovetailJoint
    where TBuilder : IHostApplicationBuilder
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/api/dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
