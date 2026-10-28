# IHostApplicationAsyncJoint<TBuilder> Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.hosting.md)
- Type: [IHostApplicationAsyncJoint<TBuilder>](/Dovetail/pr-preview/pr-246/api/joinery.dovetail.hosting/ihostapplicationasyncjoint-1.md)
- Kind: `Methods`
- Members: `1`

Async part interface for HostApplication. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-tbuilder-cancellationtoken"></a>

## Register(IDovetailContext, TBuilder, CancellationToken)

- Name: `Register(IDovetailContext, TBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the HostApplication part asynchronously.

```csharp
public interface IHostApplicationAsyncJoint<TBuilder>
{
    public abstract ValueTask Register(
        IDovetailContext context,
        TBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
