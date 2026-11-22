# IDistributedApplicationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.8.2-beta.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/IDistributedApplicationJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, DistributedApplication)](/Dovetail/api/joinery.dovetail.aspire/idistributedapplicationjoint/methods.md#register-idovetailcontext-distributedapplication) `abstract` -- Register the DistributedApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
