# DovetailWebAssemblyHelpers Methods

- Package: [Joinery.Dovetail.Hosting.WebAssembly](/Dovetail/pr-preview/pr-251/api/joinery.dovetail.hosting.webassembly.md)
- Type: [DovetailWebAssemblyHelpers](/Dovetail/pr-preview/pr-251/api/joinery.dovetail.hosting.webassembly/dovetailwebassemblyhelpers.md)
- Kind: `Methods`
- Members: `1`

Shared helpers used to wire a `Hosting.WebAssemblyHostBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-webassemblyhostbuilder-func-webassemblyhostbuilder-webassemblyhost-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(WebAssemblyHostBuilder, Func<WebAssemblyHostBuilder, WebAssemblyHost>, DovetailContextBuilder, CancellationToken)

- Name: `Configure(WebAssemblyHostBuilder, Func<WebAssemblyHostBuilder, WebAssemblyHost>, DovetailContextBuilder, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<WebAssemblyHost>`

Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the host.

```csharp
public static class DovetailWebAssemblyHelpers
{
    public static ValueTask<WebAssemblyHost> Configure(
        this WebAssemblyHostBuilder builder,
        Func<WebAssemblyHostBuilder, WebAssemblyHost> buildHost,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`WebAssemblyHostBuilder`)
  The WebAssembly host builder to configure.
- `buildHost` (`Func<WebAssemblyHostBuilder, WebAssemblyHost>`)
  A callback that builds the host from the configured builder.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<WebAssemblyHost>` -- The built and configured `Hosting.WebAssemblyHost`.

## Exceptions

- `ArgumentNullException` -- `builder`, `buildHost`, or `contextBuilder` is `null`.
