# DovetailUnoHelpers Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Type: [DovetailUnoHelpers](/Dovetail/api/joinery.dovetail.hosting.uno/dovetailunohelpers.md)
- Kind: `Methods`
- Members: `2`

Shared helpers used to wire an Uno.Extensions `Hosting.IApplicationBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-iapplicationbuilder-func-iapplicationbuilder-ihost-assembly-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(IApplicationBuilder, Func<IApplicationBuilder, IHost>, Assembly, DovetailContextBuilder, CancellationToken)

- Name: `Configure(IApplicationBuilder, Func<IApplicationBuilder, IHost>, Assembly, DovetailContextBuilder, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<IHost>`

Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the application's host, and builds it.

```csharp
public static class DovetailUnoHelpers
{
    public static ValueTask<IHost> Configure(
        this IApplicationBuilder builder,
        Func<IApplicationBuilder, IHost> buildHost,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IApplicationBuilder`)
  The Uno application builder to configure.
- `buildHost` (`Func<IApplicationBuilder, IHost>`)
  A callback that builds the host from the configured builder.
- `applicationAssembly` (`Assembly`)
  The application's assembly, which its embedded configuration files are loaded from.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<IHost>` -- The built and configured `Hosting.IHost`.

## Exceptions

- `ArgumentNullException` -- `builder`, `buildHost`, `applicationAssembly`, or `contextBuilder` is `null`.

<a id="navigate"></a>
<a id="navigate-iapplicationbuilder-assembly-dovetailcontextbuilder-cancellationtoken"></a>

## Navigate(IApplicationBuilder, Assembly, DovetailContextBuilder, CancellationToken)

- Name: `Navigate(IApplicationBuilder, Assembly, DovetailContextBuilder, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<IHost>`

Populates the context, wires configuration, services, and the service provider factory (if one was registered) into the application's host, then builds it and navigates to `TShell` (Uno.Extensions.Navigation).

```csharp
public static class DovetailUnoHelpers
{
    public static ValueTask<IHost> Navigate<TShell>(
        this IApplicationBuilder builder,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IApplicationBuilder`)
  The Uno application builder to configure.
- `applicationAssembly` (`Assembly`)
  The application's assembly, which its embedded configuration files are loaded from.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the host.

## Returns

`ValueTask<IHost>` -- The built and configured `Hosting.IHost`.

## Exceptions

- `ArgumentNullException` -- `builder`, `applicationAssembly`, or `contextBuilder` is `null`.
