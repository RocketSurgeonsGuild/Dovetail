# IUnoHostBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-pr.8`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IUnoHostBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IHostBuilder)](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/iunohostbuilderjoint/methods.md#register-idovetailcontext-ihostbuilder) `abstract` -- Register the UnoHostBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
