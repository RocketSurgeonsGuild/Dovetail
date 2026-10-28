# IWebAssemblyHostBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.4-pr.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IWebAssemblyHostBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHostBuilder, CancellationToken)](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderasyncjoint/methods.md#register-idovetailcontext-webassemblyhostbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the WebAssemblyHostBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
