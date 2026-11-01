# IStashboxJoint Methods

- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.stashbox.md)
- Type: [IStashboxJoint](/Dovetail/pr-preview/pr-295/api/joinery.dovetail.stashbox/istashboxjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-istashboxcontainer"></a>

## Register(IDovetailContext, IStashboxContainer)

- Name: `Register(IDovetailContext, IStashboxContainer)`
- Modifiers: `abstract`

Register the Stashbox part.

```csharp
public interface IStashboxJoint
{
    public abstract void Register(
        IDovetailContext context,
        IStashboxContainer container)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)
