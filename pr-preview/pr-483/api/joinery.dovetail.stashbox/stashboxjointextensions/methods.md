# StashboxJointExtensions Methods

- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.stashbox.md)
- Type: [StashboxJointExtensions](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.stashbox/stashboxjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Stashbox parts.

<a id="applystashbox"></a>
<a id="applystashbox-istashboxcontainer-idovetailcontext-cancellationtoken"></a>

## ApplyStashbox(IStashboxContainer, IDovetailContext, CancellationToken)

- Name: `ApplyStashbox(IStashboxContainer, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.Stashbox/obj/Release/net10.0/DovetailJoint.Stashbox.g.cs#L51-L52)

Applies the registered Stashbox parts.

```csharp
public static class StashboxJointExtensions
{
    public static ValueTask ApplyStashbox(
        this IStashboxContainer container,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `container` (`IStashboxContainer`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
