# IAutofacAsyncJoint Methods

- Package: [Joinery.Dovetail.Autofac](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.autofac.md)
- Type: [IAutofacAsyncJoint](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.autofac/iautofacasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for Autofac. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-containerbuilder-cancellationtoken"></a>

## Register(IDovetailContext, ContainerBuilder, CancellationToken)

- Name: `Register(IDovetailContext, ContainerBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the Autofac part asynchronously.

```csharp
public interface IAutofacAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        ContainerBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`ContainerBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
