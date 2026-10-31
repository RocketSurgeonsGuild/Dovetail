# IStashboxAsyncJoint Methods

- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-283/api/joinery.dovetail.stashbox.md)
- Type: [IStashboxAsyncJoint](/Dovetail/pr-preview/pr-283/api/joinery.dovetail.stashbox/istashboxasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Stashbox. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-istashboxcontainer-cancellationtoken"></a>

## Register(IDovetailContext, IStashboxContainer, CancellationToken)

- Name: `Register(IDovetailContext, IStashboxContainer, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Stashbox part asynchronously.

```csharp
public interface IStashboxAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IStashboxContainer container,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)
- `cancellationToken` (`CancellationToken`) `optional`
