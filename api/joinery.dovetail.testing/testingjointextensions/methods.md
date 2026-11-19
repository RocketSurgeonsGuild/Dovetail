# TestingJointExtensions Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/api/joinery.dovetail.testing.md)
- Type: [TestingJointExtensions](/Dovetail/api/joinery.dovetail.testing/testingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Testing parts.

<a id="applytesting"></a>
<a id="applytesting-idovetailcontext-cancellationtoken"></a>

## ApplyTesting(IDovetailContext, CancellationToken)

- Name: `ApplyTesting(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail.Testing/obj/Release/net10.0/DovetailJoint.Testing.g.cs#L51-L52)

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
