# IHostCreatedJoint<THost>

- Kind: `interface`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.1-beta.8`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, THost)](/Dovetail/api/dovetail/ihostcreatedjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the HostCreated part.

## See Also

- [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)
