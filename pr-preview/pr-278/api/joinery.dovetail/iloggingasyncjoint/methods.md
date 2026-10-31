# ILoggingAsyncJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-278/api/joinery.dovetail.md)
- Type: [ILoggingAsyncJoint](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/iloggingasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iloggingbuilder-cancellationtoken"></a>

## Register(IDovetailContext, ILoggingBuilder, CancellationToken)

- Name: `Register(IDovetailContext, ILoggingBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Logging part asynchronously.

```csharp
public interface ILoggingAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        ILoggingBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-278/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
