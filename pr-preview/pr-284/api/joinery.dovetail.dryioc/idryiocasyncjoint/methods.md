# IDryIocAsyncJoint Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.dryioc.md)
- Type: [IDryIocAsyncJoint](/Dovetail/pr-preview/pr-284/api/joinery.dovetail.dryioc/idryiocasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for DryIoc. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-icontainer-cancellationtoken"></a>

## Register(IDovetailContext, IContainer, CancellationToken)

- Name: `Register(IDovetailContext, IContainer, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the DryIoc part asynchronously.

```csharp
public interface IDryIocAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IContainer container,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `container` (`IContainer`)
- `cancellationToken` (`CancellationToken`) `optional`
