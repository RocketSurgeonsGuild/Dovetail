# IWebAssemblyHostJoint Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.webassembly.md)
- Type: [IWebAssemblyHostJoint](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for WebAssemblyHost. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-webassemblyhost"></a>

## Register(IDovetailContext, WebAssemblyHost)

- Name: `Register(IDovetailContext, WebAssemblyHost)`
- Modifiers: `abstract`

Register the WebAssemblyHost part.

```csharp
public interface IWebAssemblyHostJoint
{
    public abstract void Register(
        IDovetailContext context,
        WebAssemblyHost host)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `host` (`WebAssemblyHost`)
