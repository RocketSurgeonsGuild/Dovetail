# DistributedApplicationTestingJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.aspire.testing.md)
- Type: [DistributedApplicationTestingJointExtensions](/Dovetail/pr-preview/pr-469/api/joinery.dovetail.aspire.testing/distributedapplicationtestingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplicationTesting parts.

<a id="applydistributedapplicationtesting"></a>
<a id="applydistributedapplicationtesting-idistributedapplicationtestingbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplicationTesting(IDistributedApplicationTestingBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-469-merge.Sha.1a9b3d02337fce549da1c9bb8aadff64886fbdbf/src/Joinery.Dovetail.Aspire.Testing/obj/Release/net10.0/DovetailJoint.DistributedApplicationTesting.g.cs#L51-L52)

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
