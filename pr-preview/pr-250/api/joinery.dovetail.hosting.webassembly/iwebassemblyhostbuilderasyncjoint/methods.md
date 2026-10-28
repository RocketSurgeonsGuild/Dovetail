# IWebAssemblyHostBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-250/api/joinery.dovetail.hosting.webassembly.md)
- Type: [IWebAssemblyHostBuilderAsyncJoint](/Dovetail/pr-preview/pr-250/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-webassemblyhostbuilder-cancellationtoken"></a>

## Register(IDovetailContext, WebAssemblyHostBuilder, CancellationToken)

- Name: `Register(IDovetailContext, WebAssemblyHostBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the WebAssemblyHostBuilder part asynchronously.

```csharp
public interface IWebAssemblyHostBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        WebAssemblyHostBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
