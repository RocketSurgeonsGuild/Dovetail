# IHostAsyncJoint<THost> Methods

- Package: [Dovetail.Hosting](/Dovetail/pr-preview/pr-238/api/dovetail.hosting.md)
- Type: [IHostAsyncJoint<THost>](/Dovetail/pr-preview/pr-238/api/dovetail.hosting/ihostasyncjoint-1.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Host. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-thost-cancellationtoken"></a>

## Register(IDovetailContext, THost, CancellationToken)

- Name: `Register(IDovetailContext, THost, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Host part asynchronously.

```csharp
public interface IHostAsyncJoint<THost>
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

- `context` (`IDovetailContext`)
- `host` (`THost`)
- `cancellationToken` (`CancellationToken`) `optional`
