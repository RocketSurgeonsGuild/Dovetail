# IWebAssemblyHostJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IWebAssemblyHostJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHost)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostjoint/methods.md#register-idovetailcontext-webassemblyhost) `abstract` -- Register the WebAssemblyHost part.

## See Also

- `Infrastructure.IDovetailJoint`
