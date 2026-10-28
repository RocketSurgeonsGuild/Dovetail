# MauiAppBuilderJointExtensions Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.maui.md)
- Type: [MauiAppBuilderJointExtensions](/Dovetail/pr-preview/pr-234/api/dovetail.hosting.maui/mauiappbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying MauiAppBuilder parts.

<a id="applymauiappbuilder"></a>
<a id="applymauiappbuilder-mauiappbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyMauiAppBuilder(MauiAppBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyMauiAppBuilder(MauiAppBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered MauiAppBuilder parts.

```csharp
public static class MauiAppBuilderJointExtensions
{
    public static ValueTask ApplyMauiAppBuilder(
        this MauiAppBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`MauiAppBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
