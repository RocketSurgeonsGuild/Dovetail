# DryIocJointExtensions Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc.md)
- Type: [DryIocJointExtensions](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc/dryiocjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DryIoc parts.

<a id="applydryioc"></a>
<a id="applydryioc-icontainer-idovetailcontext-cancellationtoken"></a>

## ApplyDryIoc(IContainer, IDovetailContext, CancellationToken)

- Name: `ApplyDryIoc(IContainer, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs#L51-L52)

Applies the registered DryIoc parts.

```csharp
public static class DryIocJointExtensions
{
    public static ValueTask ApplyDryIoc(
        this IContainer container,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `container` (`IContainer`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
