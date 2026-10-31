# IHostApplicationJoint<TBuilder> Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-292/api/joinery.dovetail.hosting.md)
- Type: [IHostApplicationJoint<TBuilder>](/Dovetail/pr-preview/pr-292/api/joinery.dovetail.hosting/ihostapplicationjoint-1.md)
- Kind: `Methods`
- Members: `1`

Joint interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-tbuilder"></a>

## Register(IDovetailContext, TBuilder)

- Name: `Register(IDovetailContext, TBuilder)`
- Modifiers: `abstract`

Register the HostApplication part.

```csharp
public interface IHostApplicationJoint<TBuilder>
{
    public abstract void Register(
        IDovetailContext context,
        TBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)
