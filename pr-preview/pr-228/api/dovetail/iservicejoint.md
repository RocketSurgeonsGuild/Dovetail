# IServiceJoint

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/pr-preview/pr-228/api/dovetail/iservicejoint/methods.md#register-idovetailcontext-iservicecollection) `abstract` -- Register the Service part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-228/api/dovetail/idovetailjoint.md)
