# DovetailMauiHelpers

- Kind: `class`
- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.maui.md)
- Version: `0.2.1-pr.21`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Shared helpers used to wire a `Hosting.MauiAppBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Dovetail;

public static class DovetailMauiHelpers
{
    // ...
}
```

## Methods

- [Configure(MauiAppBuilder, Func<MauiAppBuilder, MauiApp>, Assembly, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.maui/dovetailmauihelpers/methods.md#configure-mauiappbuilder-func-mauiappbuilder-mauiapp-assembly-dovetailcontextbuilder-cancellationtoken) : `ValueTask<MauiApp>` `extension` -- Populates the context, applies shared host configuration, services, and logging, wires up the service provider factory (if one was registered), and builds the app.

## Fields

- [EnvironmentNameMetadataKey](/Dovetail/pr-preview/pr-228/api/dovetail.hosting.maui/dovetailmauihelpers/fields.md#environmentnamemetadatakey) : `string` `static` -- The name of the `Reflection.AssemblyMetadataAttribute` the Dovetail SDK stamps on a MAUI application with its build-time environment name ( `Development` for Debug builds, `Production` otherwise, or `$(DovetailEnvironmentName)`).
