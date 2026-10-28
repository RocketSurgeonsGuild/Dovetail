# IWebAssemblyHostBuilderJoint

- Kind: `interface`
- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/api/dovetail.hosting.webassembly.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IWebAssemblyHostBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, WebAssemblyHostBuilder)](/Dovetail/api/dovetail.hosting.webassembly/iwebassemblyhostbuilderjoint/methods.md#register-idovetailcontext-webassemblyhostbuilder) `abstract` -- Register the WebAssemblyHostBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
