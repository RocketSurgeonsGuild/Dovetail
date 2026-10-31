# IDistributedApplicationBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire.md)
- Type: [IDistributedApplicationBuilderAsyncJoint](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire/idistributedapplicationbuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-idistributedapplicationbuilder-cancellationtoken"></a>

## Register(IDovetailContext, IDistributedApplicationBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IDistributedApplicationBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the DistributedApplicationBuilder part asynchronously.

```csharp
public interface IDistributedApplicationBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IDistributedApplicationBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
