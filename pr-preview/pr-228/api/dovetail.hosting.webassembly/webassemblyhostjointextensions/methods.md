# WebAssemblyHostJointExtensions Methods

- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostJointExtensions](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.webassembly/webassemblyhostjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying WebAssemblyHost parts.

<a id="applywebassemblyhost"></a>
<a id="applywebassemblyhost-webassemblyhost-idovetailcontext-cancellationtoken"></a>

## ApplyWebAssemblyHost(WebAssemblyHost, IDovetailContext, CancellationToken)

- Name: `ApplyWebAssemblyHost(WebAssemblyHost, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered WebAssemblyHost parts.

```csharp
public static class WebAssemblyHostJointExtensions
{
    public static ValueTask ApplyWebAssemblyHost(
        this WebAssemblyHost host,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `host` (`WebAssemblyHost`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
