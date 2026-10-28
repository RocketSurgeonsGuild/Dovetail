# IHostJoint<THost>

- Kind: `interface`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostJoint<THost>
    : Dovetail.Infrastructure.IDovetailJoint
    where THost : IHost
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting/ihostjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the Host part.

## See Also

- `Infrastructure.IDovetailJoint`
