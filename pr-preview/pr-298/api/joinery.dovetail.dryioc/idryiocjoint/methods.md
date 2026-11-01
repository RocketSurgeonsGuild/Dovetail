# IDryIocJoint Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.dryioc.md)
- Type: [IDryIocJoint](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.dryioc/idryiocjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-icontainer"></a>

## Register(IDovetailContext, IContainer)

- Name: `Register(IDovetailContext, IContainer)`
- Modifiers: `abstract`

Register the DryIoc part.

```csharp
public interface IDryIocJoint
{
    public abstract void Register(
        IDovetailContext context,
        IContainer container)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)
