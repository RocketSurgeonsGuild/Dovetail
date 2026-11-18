# TestingJointExtensions Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing.md)
- Type: [TestingJointExtensions](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing/testingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Testing parts.

<a id="applytesting"></a>
<a id="applytesting-idovetailcontext-cancellationtoken"></a>

## ApplyTesting(IDovetailContext, CancellationToken)

- Name: `ApplyTesting(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.Testing/obj/Release/net10.0/DovetailJoint.Testing.g.cs#L51-L52)

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
