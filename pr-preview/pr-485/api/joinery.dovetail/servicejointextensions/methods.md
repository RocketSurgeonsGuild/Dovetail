# ServiceJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.md)
- Type: [ServiceJointExtensions](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/servicejointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Service parts.

<a id="applyservice"></a>
<a id="applyservice-iservicecollection-idovetailcontext-cancellationtoken"></a>

## ApplyService(IServiceCollection, IDovetailContext, CancellationToken)

- Name: `ApplyService(IServiceCollection, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Service.g.cs#L51-L52)

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
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-485/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
