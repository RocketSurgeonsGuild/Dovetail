# IDryIocJoint Methods

- Package: [Dovetail.DryIoc](/Dovetail/v0.2.2/api/dovetail.dryioc.md)
- Type: [IDryIocJoint](/Dovetail/v0.2.2/api/dovetail.dryioc/idryiocjoint.md)
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
