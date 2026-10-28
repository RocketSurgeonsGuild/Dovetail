# IMauiAppJoint Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/v0.2.2/api/dovetail.hosting.maui.md)
- Type: [IMauiAppJoint](/Dovetail/v0.2.2/api/dovetail.hosting.maui/imauiappjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for MauiApp. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-mauiapp"></a>

## Register(IDovetailContext, MauiApp)

- Name: `Register(IDovetailContext, MauiApp)`
- Modifiers: `abstract`

Register the MauiApp part.

```csharp
public interface IMauiAppJoint
{
    public abstract void Register(
        IDovetailContext context,
        MauiApp app)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `app` (`MauiApp`)
