# HostCreatedJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.md)
- Type: [HostCreatedJointExtensions](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/hostcreatedjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying HostCreated parts.

<a id="applyhostcreated"></a>
<a id="applyhostcreated-thost-idovetailcontext-cancellationtoken"></a>

## ApplyHostCreated(THost, IDovetailContext, CancellationToken)

- Name: `ApplyHostCreated(THost, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-480-merge.Sha.bbd00571859e9fa257a29d29502064fe14badc48/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.HostCreated.g.cs#L51-L52)

Applies the registered HostCreated parts.

```csharp
public static class HostCreatedJointExtensions
{
    public static ValueTask ApplyHostCreated<THost>(
        this THost host,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `host` (`THost`)
- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
