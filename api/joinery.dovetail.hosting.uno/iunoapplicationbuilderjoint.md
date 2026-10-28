# IUnoApplicationBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-beta.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IUnoApplicationBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IApplicationBuilder)](/Dovetail/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderjoint/methods.md#register-idovetailcontext-iapplicationbuilder) `abstract` -- Register the UnoApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
