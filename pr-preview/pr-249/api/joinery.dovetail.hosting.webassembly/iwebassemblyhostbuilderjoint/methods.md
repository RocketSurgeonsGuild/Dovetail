# IWebAssemblyHostBuilderJoint Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly.md)
- Type: [IWebAssemblyHostBuilderJoint](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.webassembly/iwebassemblyhostbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for WebAssemblyHostBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-webassemblyhostbuilder"></a>

## Register(IDovetailContext, WebAssemblyHostBuilder)

- Name: `Register(IDovetailContext, WebAssemblyHostBuilder)`
- Modifiers: `abstract`

Register the WebAssemblyHostBuilder part.

```csharp
public interface IWebAssemblyHostBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        WebAssemblyHostBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`WebAssemblyHostBuilder`)
