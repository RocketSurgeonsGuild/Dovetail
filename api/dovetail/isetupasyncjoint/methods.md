# ISetupAsyncJoint Methods

- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Type: [ISetupAsyncJoint](/Dovetail/api/dovetail/isetupasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-cancellationtoken"></a>

## Register(IDovetailContext, CancellationToken)

- Name: `Register(IDovetailContext, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Setup part asynchronously.

```csharp
public interface ISetupAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/api/dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`
