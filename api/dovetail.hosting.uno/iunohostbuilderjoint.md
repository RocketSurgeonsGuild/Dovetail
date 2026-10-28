# IUnoHostBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.Uno](/Dovetail/api/dovetail.hosting.uno.md)
- Version: `0.2.3-beta.1`
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

- [Register(IDovetailContext, IHostBuilder)](/Dovetail/api/dovetail.hosting.uno/iunohostbuilderjoint/methods.md#register-idovetailcontext-ihostbuilder) `abstract` -- Register the UnoHostBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
