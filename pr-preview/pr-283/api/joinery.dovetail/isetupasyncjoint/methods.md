# ISetupAsyncJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-283/api/joinery.dovetail.md)
- Type: [ISetupAsyncJoint](/Dovetail/pr-preview/pr-283/api/joinery.dovetail/isetupasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-283/api/joinery.dovetail/idovetailjoint.md)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-283/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`
