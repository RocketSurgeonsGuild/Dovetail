# IWebAssemblyHostAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/api/joinery.dovetail.hosting.webassembly.md)
- Type: [IWebAssemblyHostAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-webassemblyhost-cancellationtoken"></a>

## Register(IDovetailContext, WebAssemblyHost, CancellationToken)

- Name: `Register(IDovetailContext, WebAssemblyHost, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the WebAssemblyHost part asynchronously.

```csharp
public interface IWebAssemblyHostAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        WebAssemblyHost host,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `host` (`WebAssemblyHost`)
- `cancellationToken` (`CancellationToken`) `optional`
