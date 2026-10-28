# IServiceJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Dovetail.Joints;

public interface IServiceJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/v0.2.2/api/dovetail/iservicejoint/methods.md#register-idovetailcontext-iservicecollection) `abstract` -- Register the Service part.

## See Also

- [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
