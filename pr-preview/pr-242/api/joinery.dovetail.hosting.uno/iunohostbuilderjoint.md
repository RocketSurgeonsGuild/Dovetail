# IUnoHostBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IUnoHostBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IHostBuilder)](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.hosting.uno/iunohostbuilderjoint/methods.md#register-idovetailcontext-ihostbuilder) `abstract` -- Register the UnoHostBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
