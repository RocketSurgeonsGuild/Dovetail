# IHostApplicationAsyncJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/api/joinery.dovetail.hosting.md)
- Version: `0.8.2-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/IHostApplicationAsyncJoint.cs)
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

- [Register(IDovetailContext, TBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1/methods.md#register-idovetailcontext-tbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the HostApplication part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
