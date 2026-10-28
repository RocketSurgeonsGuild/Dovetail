# DovetailMauiHelpers Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui.md)
- Type: [DovetailMauiHelpers](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui/dovetailmauihelpers.md)
- Kind: `Methods`
- Members: `1`

Shared helpers used to wire a `Hosting.MauiAppBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="configure"></a>
<a id="configure-mauiappbuilder-func-mauiappbuilder-mauiapp-assembly-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(MauiAppBuilder, Func<MauiAppBuilder, MauiApp>, Assembly, DovetailContextBuilder, CancellationToken)

- Name: `Configure(MauiAppBuilder, Func<MauiAppBuilder, MauiApp>, Assembly, DovetailContextBuilder, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<MauiApp>`

Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the app.

```csharp
public static class DovetailMauiHelpers
{
    public static ValueTask<MauiApp> Configure(
        this MauiAppBuilder builder,
        Func<MauiAppBuilder, MauiApp> buildHost,
        Assembly applicationAssembly,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`MauiAppBuilder`)
  The MAUI app builder to configure.
- `buildHost` (`Func<MauiAppBuilder, MauiApp>`)
  A callback that builds the app from the configured builder.
- `applicationAssembly` (`Assembly`)
  The application's assembly. Its embedded configuration files are loaded, and its name and build-time environment name are used, since `Assembly.GetEntryAssembly` isn't reliable on every MAUI platform.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying configuration and building the app.

## Returns

`ValueTask<MauiApp>` -- The built and configured `Hosting.MauiApp`.

## Exceptions

- `ArgumentNullException` -- `builder`, `buildHost`, `applicationAssembly`, or `contextBuilder` is `null`.
