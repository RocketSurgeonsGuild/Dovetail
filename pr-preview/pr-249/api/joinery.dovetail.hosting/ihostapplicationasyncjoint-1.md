# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostApplicationAsyncJoint<TBuilder>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
    where TBuilder : IHostApplicationBuilder
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
