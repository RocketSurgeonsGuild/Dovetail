# IHostCreatedJoint<THost>

- Kind: `interface`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostCreatedJoint<THost>
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost)](/Dovetail/v0.2.2/api/dovetail/ihostcreatedjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the HostCreated part.

## See Also

- [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
