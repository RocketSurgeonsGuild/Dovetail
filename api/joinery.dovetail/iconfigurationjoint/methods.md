# IConfigurationJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [IConfigurationJoint](/Dovetail/api/joinery.dovetail/iconfigurationjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iconfigurationbuilder"></a>

## Register(IDovetailContext, IConfigurationBuilder)

- Name: `Register(IDovetailContext, IConfigurationBuilder)`
- Modifiers: `abstract`

Register the Configuration part.

```csharp
public interface IConfigurationJoint
{
    public abstract void Register(
        IDovetailContext context,
        IConfigurationBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
