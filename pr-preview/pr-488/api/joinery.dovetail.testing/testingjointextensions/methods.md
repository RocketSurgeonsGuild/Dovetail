# TestingJointExtensions Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.testing.md)
- Type: [TestingJointExtensions](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.testing/testingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Testing parts.

<a id="applytesting"></a>
<a id="applytesting-idovetailcontext-cancellationtoken"></a>

## ApplyTesting(IDovetailContext, CancellationToken)

- Name: `ApplyTesting(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail.Testing/obj/Release/net10.0/DovetailJoint.Testing.g.cs#L51-L52)

Applies the registered Testing parts.

```csharp
public static class TestingJointExtensions
{
    public static ValueTask ApplyTesting(
        this IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
