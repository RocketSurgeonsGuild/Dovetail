# IStashboxJoint Methods

- Package: [Dovetail.Stashbox](/Dovetail/pr-preview/pr-234/api/dovetail.stashbox.md)
- Type: [IStashboxJoint](/Dovetail/pr-preview/pr-234/api/dovetail.stashbox/istashboxjoint.md)
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
