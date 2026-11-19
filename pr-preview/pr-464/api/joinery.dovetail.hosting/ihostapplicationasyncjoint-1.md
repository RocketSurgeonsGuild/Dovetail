# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/IHostApplicationAsyncJoint.cs)
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

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
