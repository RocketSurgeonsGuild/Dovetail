# IServiceAsyncJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-260/api/joinery.dovetail.md)
- Type: [IServiceAsyncJoint](/Dovetail/pr-preview/pr-260/api/joinery.dovetail/iserviceasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-260/api/joinery.dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iservicecollection-cancellationtoken"></a>

## Register(IDovetailContext, IServiceCollection, CancellationToken)

- Name: `Register(IDovetailContext, IServiceCollection, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Service part asynchronously.

```csharp
public interface IServiceAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IServiceCollection services,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-260/api/joinery.dovetail/idovetailcontext.md))
- `services` (`IServiceCollection`)
- `cancellationToken` (`CancellationToken`) `optional`
