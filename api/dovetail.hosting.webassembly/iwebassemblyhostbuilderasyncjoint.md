# IWebAssemblyHostBuilderAsyncJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/api/dovetail.hosting.webassembly.md)
- Version: `0.2.3-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IWebAssemblyHostBuilderAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHostBuilder, CancellationToken)](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostbuilderasyncjoint/methods.md#register-idovetailcontext-webassemblyhostbuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the WebAssemblyHostBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
