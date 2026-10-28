# IConfigurationAsyncJoint Methods

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [IConfigurationAsyncJoint](/Dovetail/v0.2.2/api/dovetail/iconfigurationasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iconfigurationbuilder-cancellationtoken"></a>

## Register(IDovetailContext, IConfigurationBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IConfigurationBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Configuration part asynchronously.

```csharp
public interface IConfigurationAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IConfigurationBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
