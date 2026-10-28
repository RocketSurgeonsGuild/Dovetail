# IDistributedApplicationAsyncJoint Methods

- Package: [Dovetail.Aspire](/Dovetail/pr-preview/pr-238/api/dovetail.aspire.md)
- Type: [IDistributedApplicationAsyncJoint](/Dovetail/pr-preview/pr-238/api/dovetail.aspire/idistributedapplicationasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-distributedapplication-cancellationtoken"></a>

## Register(IDovetailContext, DistributedApplication, CancellationToken)

- Name: `Register(IDovetailContext, DistributedApplication, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the DistributedApplication part asynchronously.

```csharp
public interface IDistributedApplicationAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        DistributedApplication host,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)
- `cancellationToken` (`CancellationToken`) `optional`
