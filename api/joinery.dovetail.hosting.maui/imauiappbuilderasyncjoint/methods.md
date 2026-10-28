# IMauiAppBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/api/joinery.dovetail.hosting.maui.md)
- Type: [IMauiAppBuilderAsyncJoint](/Dovetail/api/joinery.dovetail.hosting.maui/imauiappbuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for MauiAppBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-mauiappbuilder-cancellationtoken"></a>

## Register(IDovetailContext, MauiAppBuilder, CancellationToken)

- Name: `Register(IDovetailContext, MauiAppBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the MauiAppBuilder part asynchronously.

```csharp
public interface IMauiAppBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        MauiAppBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`MauiAppBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
