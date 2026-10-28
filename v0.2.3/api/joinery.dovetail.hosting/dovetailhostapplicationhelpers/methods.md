# DovetailHostApplicationHelpers Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.md)
- Type: [DovetailHostApplicationHelpers](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/dovetailhostapplicationhelpers.md)
- Kind: `Methods`
- Members: `1`

Shared helpers used by host-specific integration packages (e.g. web, WebAssembly) to wire an `Hosting.IHostApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-t-func-t-thost-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(T, Func<T, THost>, DovetailContextBuilder, CancellationToken)

- Name: `Configure(T, Func<T, THost>, DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<THost>`

Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the host.

```csharp
public static class DovetailHostApplicationHelpers
{
    public static ValueTask<THost> Configure<T, THost>(
        T builder,
        Func<T, THost> buildHost,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`T`)
  The host application builder to configure.
- `buildHost` (`Func<T, THost>`)
  A callback that builds the host from the configured builder.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<THost>` -- The built and configured `THost`.

## Exceptions

- `ArgumentNullException` -- `buildHost` or `contextBuilder` is `null`.
