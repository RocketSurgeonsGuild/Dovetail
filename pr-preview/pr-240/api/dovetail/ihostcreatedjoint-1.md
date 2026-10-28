# IHostCreatedJoint<THost>

- Kind: `interface`
- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, THost)](/Dovetail/pr-preview/pr-240/api/dovetail/ihostcreatedjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the HostCreated part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)
