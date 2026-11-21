# IHostCreatedJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-482/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-482-merge.Sha.ed7ce705781acedbf95959cccbb82d44907b62b8/src/Joinery.Dovetail/obj/Release/net10.0/IHostCreatedJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/idovetailjoint.md)

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostCreatedJoint<THost>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, THost)](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/ihostcreatedjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the HostCreated part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-482/api/joinery.dovetail/idovetailjoint.md)
