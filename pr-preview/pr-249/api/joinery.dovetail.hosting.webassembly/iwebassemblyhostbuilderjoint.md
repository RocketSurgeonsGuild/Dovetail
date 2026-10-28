# IWebAssemblyHostBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IWebAssemblyHostBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHostBuilder)](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderjoint/methods.md#register-idovetailcontext-webassemblyhostbuilder) `abstract` -- Register the WebAssemblyHostBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
