# IHostJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.hosting.md)
- Version: `0.2.4-pr.11`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostJoint<THost>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
    where THost : IHost
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost)](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.hosting/ihostjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the Host part.

## See Also

- `Infrastructure.IDovetailJoint`
