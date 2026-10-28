# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.md)
- Version: `0.2.2-pr.4`
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

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/pr-preview/pr-235/api/dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
