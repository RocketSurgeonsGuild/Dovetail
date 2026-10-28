# IMauiAppBuilderJoint Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui.md)
- Type: [IMauiAppBuilderJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui/imauiappbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-mauiappbuilder"></a>

## Register(IDovetailContext, MauiAppBuilder)

- Name: `Register(IDovetailContext, MauiAppBuilder)`
- Modifiers: `abstract`

Register the MauiAppBuilder part.

```csharp
public interface IMauiAppBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        MauiAppBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)
