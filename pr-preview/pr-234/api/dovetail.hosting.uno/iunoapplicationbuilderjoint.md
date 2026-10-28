# IUnoApplicationBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno.md)
- Version: `0.2.2-pr.2`
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

- [Register(IDovetailContext, IApplicationBuilder)](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.uno/iunoapplicationbuilderjoint/methods.md#register-idovetailcontext-iapplicationbuilder) `abstract` -- Register the UnoApplicationBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
