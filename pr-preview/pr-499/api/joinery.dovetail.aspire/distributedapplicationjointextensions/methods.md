# DistributedApplicationJointExtensions Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire.md)
- Type: [DistributedApplicationJointExtensions](/Dovetail/pr-preview/pr-499/api/joinery.dovetail.aspire/distributedapplicationjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DistributedApplication parts.

<a id="applydistributedapplication"></a>
<a id="applydistributedapplication-distributedapplication-idovetailcontext-cancellationtoken"></a>

## ApplyDistributedApplication(DistributedApplication, IDovetailContext, CancellationToken)

- Name: `ApplyDistributedApplication(DistributedApplication, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-499-merge.Sha.68a41713586a7e9859f841bc08b323d357b9a00a/src/Joinery.Dovetail.Aspire/obj/Release/net10.0/DovetailJoint.DistributedApplication.g.cs#L51-L52)

Applies the registered DistributedApplication parts.

```csharp
public static class DistributedApplicationJointExtensions
{
    public static ValueTask ApplyDistributedApplication(
        this DistributedApplication host,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `host` (`DistributedApplication`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
