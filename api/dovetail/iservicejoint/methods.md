# IServiceJoint Methods

- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Type: [IServiceJoint](/Dovetail/api/dovetail/iservicejoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection"></a>

## Register(IDovetailContext, IServiceCollection)

- Name: `Register(IDovetailContext, IServiceCollection)`
- Modifiers: `abstract`

Register the Service part.

```csharp
public interface IServiceJoint
{
    public abstract void Register(
        IDovetailContext context,
        IServiceCollection services)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/api/dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
