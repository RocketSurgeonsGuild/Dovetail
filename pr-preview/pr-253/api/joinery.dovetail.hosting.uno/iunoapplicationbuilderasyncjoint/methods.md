# IUnoApplicationBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno.md)
- Type: [IUnoApplicationBuilderAsyncJoint](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/iunoapplicationbuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iapplicationbuilder-cancellationtoken"></a>

## Register(IDovetailContext, IApplicationBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IApplicationBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the UnoApplicationBuilder part asynchronously.

```csharp
public interface IUnoApplicationBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IApplicationBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
