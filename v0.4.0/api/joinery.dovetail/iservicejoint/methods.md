# IServiceJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Type: [IServiceJoint](/Dovetail/v0.4.0/api/joinery.dovetail/iservicejoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailjoint.md)

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

- `context` ([IDovetailContext](/Dovetail/v0.4.0/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
