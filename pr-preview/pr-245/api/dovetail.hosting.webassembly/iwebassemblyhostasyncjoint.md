# IWebAssemblyHostAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-245/api/dovetail.hosting.webassembly.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IWebAssemblyHostAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHost, CancellationToken)](/Dovetail/pr-preview/pr-245/api/dovetail.hosting.webassembly/iwebassemblyhostasyncjoint/methods.md#register-idovetailcontext-webassemblyhost-cancellationtoken) : `ValueTask` `abstract` -- Register the WebAssemblyHost part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
