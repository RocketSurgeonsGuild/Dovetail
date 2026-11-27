# DistributedApplicationTestingJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingJointExtensions](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplicationTesting parts.

<a id="applydistributedapplicationtesting"></a>
<a id="applydistributedapplicationtesting-idistributedapplicationtestingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-487-merge.Sha.d2fe0860408e23901125d26601527b824db451a0/src/Joinery.Dovetail.Aspire.Testing/obj/Release/net10.0/DovetailJoint.DistributedApplicationTesting.g.cs#L51-L52)

Applies the registered DistributedApplicationTesting parts.

```csharp
public static class DistributedApplicationTestingJointExtensions
{
    public static ValueTask ApplyDistributedApplicationTesting(
        this IDistributedApplicationTestingBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IDistributedApplicationTestingBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
