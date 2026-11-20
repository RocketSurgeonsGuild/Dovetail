# IServiceJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail/obj/Release/net10.0/IServiceJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, IServiceCollection)](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/iservicejoint/methods.md#register-idovetailcontext-iservicecollection) `abstract` -- Register the Service part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-472/api/joinery.dovetail/idovetailjoint.md)
