# IUnoApplicationBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IUnoApplicationBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IApplicationBuilder)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderjoint/methods.md#register-idovetailcontext-iapplicationbuilder) `abstract` -- Register the UnoApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
