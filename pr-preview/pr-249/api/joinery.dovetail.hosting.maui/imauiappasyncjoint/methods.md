# IMauiAppAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui.md)
- Type: [IMauiAppAsyncJoint](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.hosting.maui/imauiappasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-mauiapp-cancellationtoken"></a>

## Register(IDovetailContext, MauiApp, CancellationToken)

- Name: `Register(IDovetailContext, MauiApp, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the MauiApp part asynchronously.

```csharp
public interface IMauiAppAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        MauiApp app,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `app` (`MauiApp`)
- `cancellationToken` (`CancellationToken`) `optional`
