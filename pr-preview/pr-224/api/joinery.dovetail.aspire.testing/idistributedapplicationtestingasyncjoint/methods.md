# IDistributedApplicationTestingAsyncJoint Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire.testing.md)
- Type: [IDistributedApplicationTestingAsyncJoint](/Dovetail/pr-preview/pr-224/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-idistributedapplicationtestingbuilder-cancellationtoken"></a>

## Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)

- Name: `Register(IDovetailContext, IDistributedApplicationTestingBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the DistributedApplicationTesting part asynchronously.

```csharp
public interface IDistributedApplicationTestingAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        IDistributedApplicationTestingBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
