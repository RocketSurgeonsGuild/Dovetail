# IHostCreatedJoint<THost> Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-291/api/joinery.dovetail.md)
- Type: [IHostCreatedJoint<THost>](/Dovetail/pr-preview/pr-291/api/joinery.dovetail/ihostcreatedjoint-1.md)
- Kind: `Methods`
- Members: `1`

Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-291/api/joinery.dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-thost"></a>

## Register(IDovetailContext, THost)

- Name: `Register(IDovetailContext, THost)`
- Modifiers: `abstract`

Register the HostCreated part.

```csharp
public interface IHostCreatedJoint<THost>
{
    public abstract void Register(
        IDovetailContext context,
        THost host)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-291/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)
