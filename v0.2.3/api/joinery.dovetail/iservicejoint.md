# IServiceJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/v0.2.3/api/joinery.dovetail.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IServiceJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/v0.2.3/api/joinery.dovetail/iservicejoint/methods.md#register-idovetailcontext-iservicecollection) `abstract` -- Register the Service part.

## See Also

- [IDovetailJoint](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailjoint.md)
