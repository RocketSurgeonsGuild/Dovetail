# DovetailMauiHelpers Fields

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-245/api/dovetail.hosting.maui.md)
- Type: [DovetailMauiHelpers](/Dovetail/pr-preview/pr-245/api/dovetail.hosting.maui/dovetailmauihelpers.md)
- Kind: `Fields`
- Members: `1`

Shared helpers used to wire a `Hosting.MauiAppBuilder` up to a Dovetail `IDovetailContext`. Not intended to be called directly by consumers; hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="environmentnamemetadatakey"></a>

## EnvironmentNameMetadataKey

- Name: `EnvironmentNameMetadataKey`
- Modifiers: `static` `const`
- Returns: `string`

The name of the `Reflection.AssemblyMetadataAttribute` the Dovetail SDK stamps on a MAUI application with its build-time environment name ( `Development` for Debug builds, `Production` otherwise, or `$(DovetailEnvironmentName)`).

```csharp
public const static string EnvironmentNameMetadataKey
```
