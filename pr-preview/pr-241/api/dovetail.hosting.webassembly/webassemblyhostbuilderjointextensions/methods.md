# WebAssemblyHostBuilderJointExtensions Methods

- Package: [Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.webassembly.md)
- Type: [WebAssemblyHostBuilderJointExtensions](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.webassembly/webassemblyhostbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying WebAssemblyHostBuilder parts.

<a id="applywebassemblyhostbuilder"></a>
<a id="applywebassemblyhostbuilder-webassemblyhostbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyWebAssemblyHostBuilder(WebAssemblyHostBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyWebAssemblyHostBuilder(WebAssemblyHostBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered WebAssemblyHostBuilder parts.

```csharp
public static class WebAssemblyHostBuilderJointExtensions
{
    public static ValueTask ApplyWebAssemblyHostBuilder(
        this WebAssemblyHostBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`WebAssemblyHostBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
