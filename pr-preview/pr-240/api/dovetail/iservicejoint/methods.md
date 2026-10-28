# IServiceJoint Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Type: [IServiceJoint](/Dovetail/pr-preview/pr-240/api/dovetail/iservicejoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
