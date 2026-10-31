# IAutofacJoint Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.autofac.md)
- Type: [IAutofacJoint](/Dovetail/pr-preview/pr-281/api/joinery.dovetail.autofac/iautofacjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-containerbuilder"></a>

## Register(IDovetailContext, ContainerBuilder)

- Name: `Register(IDovetailContext, ContainerBuilder)`
- Modifiers: `abstract`

Register the Autofac part.

```csharp
public interface IAutofacJoint
{
    public abstract void Register(
        IDovetailContext context,
        ContainerBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)
