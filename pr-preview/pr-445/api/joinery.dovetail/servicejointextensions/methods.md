# ServiceJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.md)
- Type: [ServiceJointExtensions](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/servicejointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Service parts.

<a id="applyservice"></a>
<a id="applyservice-iservicecollection-idovetailcontext-cancellationtoken"></a>

## ApplyService(IServiceCollection, IDovetailContext, CancellationToken)

- Name: `ApplyService(IServiceCollection, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Service.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
