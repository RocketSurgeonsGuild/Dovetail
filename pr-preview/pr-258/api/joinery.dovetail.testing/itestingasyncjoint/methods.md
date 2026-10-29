# ITestingAsyncJoint Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-258/api/joinery.dovetail.testing.md)
- Type: [ITestingAsyncJoint](/Dovetail/pr-preview/pr-258/api/joinery.dovetail.testing/itestingasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Testing. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-cancellationtoken"></a>

## Register(IDovetailContext, CancellationToken)

- Name: `Register(IDovetailContext, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Testing part asynchronously.

```csharp
public interface ITestingAsyncJoint
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

- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`
