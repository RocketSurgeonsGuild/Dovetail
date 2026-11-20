# IHostCreatedJoint<THost>

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail/obj/Release/net10.0/IHostCreatedJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)

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

- [Register(IDovetailContext, THost)](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/ihostcreatedjoint-1/methods.md#register-idovetailcontext-thost) `abstract` -- Register the HostCreated part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-469/api/joinery.dovetail/idovetailjoint.md)
