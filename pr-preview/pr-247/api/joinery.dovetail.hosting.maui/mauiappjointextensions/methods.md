# MauiAppJointExtensions Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui.md)
- Type: [MauiAppJointExtensions](/Dovetail/pr-preview/pr-247/api/joinery.dovetail.hosting.maui/mauiappjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying MauiApp parts.

<a id="applymauiapp"></a>
<a id="applymauiapp-mauiapp-idovetailcontext-cancellationtoken"></a>

## ApplyMauiApp(MauiApp, IDovetailContext, CancellationToken)

- Name: `ApplyMauiApp(MauiApp, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered MauiApp parts.

```csharp
public static class MauiAppJointExtensions
{
    public static ValueTask ApplyMauiApp(
        this MauiApp app,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `app` (`MauiApp`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
