# ServiceJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.md)
- Type: [ServiceJointExtensions](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/servicejointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Service parts.

<a id="applyservice"></a>
<a id="applyservice-iservicecollection-idovetailcontext-cancellationtoken"></a>

## ApplyService(IServiceCollection, IDovetailContext, CancellationToken)

- Name: `ApplyService(IServiceCollection, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.1faa3d8c1fb998da38c062158c5a17b8390b25fa/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Service.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
