# DistributedApplicationBuilderJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationBuilderJointExtensions](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.aspire/distributedapplicationbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplicationBuilder parts.

<a id="applydistributedapplicationbuilder"></a>
<a id="applydistributedapplicationbuilder-idistributedapplicationbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplicationBuilder(IDistributedApplicationBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplicationBuilder(IDistributedApplicationBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-487-merge.Sha.d2fe0860408e23901125d26601527b824db451a0/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DovetailJoint.DistributedApplicationBuilder.g.cs#L51-L52)

Applies the registered DistributedApplicationBuilder parts.

```csharp
public static class DistributedApplicationBuilderJointExtensions
{
    public static ValueTask ApplyDistributedApplicationBuilder(
        this IDistributedApplicationBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IDistributedApplicationBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
