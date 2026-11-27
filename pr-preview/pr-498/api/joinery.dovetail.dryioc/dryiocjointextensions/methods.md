# DryIocJointExtensions Methods

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.dryioc.md)
- Type: [DryIocJointExtensions](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.dryioc/dryiocjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying DryIoc parts.

<a id="applydryioc"></a>
<a id="applydryioc-icontainer-idovetailcontext-cancellationtoken"></a>

## ApplyDryIoc(IContainer, IDovetailContext, CancellationToken)

- Name: `ApplyDryIoc(IContainer, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail.DryIoc/obj/Release/net10.0/DovetailJoint.DryIoc.g.cs#L51-L52)

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
