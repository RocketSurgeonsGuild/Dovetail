# IHostJoint<THost> Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/api/joinery.dovetail.hosting.md)
- Type: [IHostJoint<THost>](/Dovetail/api/joinery.dovetail.hosting/ihostjoint-1.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Host. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-thost"></a>

## Register(IDovetailContext, THost)

- Name: `Register(IDovetailContext, THost)`
- Modifiers: `abstract`

Register the Host part.

```csharp
public interface IHostJoint<THost>
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

- `context` (`IDovetailContext`)
- `host` (`THost`)
