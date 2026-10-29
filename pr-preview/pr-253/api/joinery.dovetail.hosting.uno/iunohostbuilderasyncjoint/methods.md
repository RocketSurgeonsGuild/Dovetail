# IUnoHostBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno.md)
- Type: [IUnoHostBuilderAsyncJoint](/Dovetail/pr-preview/pr-253/api/joinery.dovetail.hosting.uno/iunohostbuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-ihostbuilder-cancellationtoken"></a>

## Register(IDovetailContext, IHostBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IHostBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the UnoHostBuilder part asynchronously.

```csharp
public interface IUnoHostBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IHostBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
