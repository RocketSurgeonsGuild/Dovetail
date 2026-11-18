# IDistributedApplicationBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.aspire.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/IDistributedApplicationBuilderJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IDistributedApplicationBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IDistributedApplicationBuilder)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.aspire/idistributedapplicationbuilderjoint/methods.md#register-idovetailcontext-idistributedapplicationbuilder) `abstract` -- Register the DistributedApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
