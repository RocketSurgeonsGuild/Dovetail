# DistributedApplicationTestingJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingJointExtensions](/Dovetail/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplicationTesting parts.

<a id="applydistributedapplicationtesting"></a>
<a id="applydistributedapplicationtesting-idistributedapplicationtestingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail.Aspire.Testing/obj/Release/net10.0/DovetailJoint.DistributedApplicationTesting.g.cs#L51-L52)

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
