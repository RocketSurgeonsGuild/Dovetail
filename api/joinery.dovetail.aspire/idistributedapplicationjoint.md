# IDistributedApplicationJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/api/joinery.dovetail.aspire.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/IDistributedApplicationJoint.cs)
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
