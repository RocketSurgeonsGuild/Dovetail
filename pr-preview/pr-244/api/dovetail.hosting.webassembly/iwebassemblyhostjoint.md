# IWebAssemblyHostJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.webassembly.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IWebAssemblyHostJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHost)](/Dovetail/pr-preview/pr-244/api/dovetail.hosting.webassembly/iwebassemblyhostjoint/methods.md#register-idovetailcontext-webassemblyhost) `abstract` -- Register the WebAssemblyHost part.

## See Also

- `Infrastructure.IDovetailJoint`
