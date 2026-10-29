# IHostApplicationJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.3.0/api/joinery.dovetail.hosting.md)
- Version: `0.3.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostApplicationJoint<TBuilder>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder)](/Dovetail/v0.3.0/api/joinery.dovetail.hosting/ihostapplicationjoint-1/methods.md#register-idovetailcontext-tbuilder) `abstract` -- Register the HostApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
