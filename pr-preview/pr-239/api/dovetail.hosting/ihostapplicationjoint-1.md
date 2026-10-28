# IHostApplicationJoint<TBuilder>

- Kind: `interface`
- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-239/api/dovetail.hosting.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IHostApplicationJoint<TBuilder>
    : Dovetail.Infrastructure.IDovetailJoint
    where TBuilder : IHostApplicationBuilder
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TBuilder)](/Dovetail/pr-preview/pr-239/api/dovetail.hosting/ihostapplicationjoint-1/methods.md#register-idovetailcontext-tbuilder) `abstract` -- Register the HostApplication part.

## See Also

- `Infrastructure.IDovetailJoint`
