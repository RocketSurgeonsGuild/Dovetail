# IHostCreatedAsyncJoint<THost> Methods

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [IHostCreatedAsyncJoint<THost>](/Dovetail/v0.2.2/api/dovetail/ihostcreatedasyncjoint-1.md)
- Kind: `Methods`
- Members: `1`

Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-thost-cancellationtoken"></a>

## Register(IDovetailContext, THost, CancellationToken)

- Name: `Register(IDovetailContext, THost, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the HostCreated part asynchronously.

```csharp
public interface IHostCreatedAsyncJoint<THost>
{
    public abstract ValueTask Register(
        IDovetailContext context,
        THost host,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`
