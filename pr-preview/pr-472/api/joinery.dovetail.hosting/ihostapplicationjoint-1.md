# IHostApplicationJoint<TBuilder>

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.hosting.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/IHostApplicationJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IHostApplicationJoint<TBuilder>
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder)](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.hosting/ihostapplicationjoint-1/methods.md#register-idovetailcontext-tbuilder) `abstract` -- Register the HostApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
