# ServiceJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Type: [ServiceJointExtensions](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/servicejointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Service parts.

<a id="applyservice"></a>
<a id="applyservice-iservicecollection-idovetailcontext-cancellationtoken"></a>

## ApplyService(IServiceCollection, IDovetailContext, CancellationToken)

- Name: `ApplyService(IServiceCollection, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Service.g.cs#L51-L52)

Applies the registered Service parts.

```csharp
public static class ServiceJointExtensions
{
    public static ValueTask ApplyService(
        this IServiceCollection services,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `services` (`IServiceCollection`)
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
