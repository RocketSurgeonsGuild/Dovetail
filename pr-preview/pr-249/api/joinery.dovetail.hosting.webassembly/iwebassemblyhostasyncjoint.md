# IWebAssemblyHostAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IWebAssemblyHostAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHost, CancellationToken)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostasyncjoint/methods.md#register-idovetailcontext-webassemblyhost-cancellationtoken) : `ValueTask` `abstract` -- Register the WebAssemblyHost part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
