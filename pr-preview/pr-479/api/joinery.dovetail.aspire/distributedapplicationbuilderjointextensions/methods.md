# DistributedApplicationBuilderJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationBuilderJointExtensions](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.aspire/distributedapplicationbuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplicationBuilder parts.

<a id="applydistributedapplicationbuilder"></a>
<a id="applydistributedapplicationbuilder-idistributedapplicationbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplicationBuilder(IDistributedApplicationBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplicationBuilder(IDistributedApplicationBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DovetailJoint.DistributedApplicationBuilder.g.cs#L51-L52)

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
