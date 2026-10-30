# TestingJointExtensions Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.testing.md)
- Type: [TestingJointExtensions](/Dovetail/pr-preview/pr-267/api/joinery.dovetail.testing/testingjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Testing parts.

<a id="applytesting"></a>
<a id="applytesting-idovetailcontext-cancellationtoken"></a>

## ApplyTesting(IDovetailContext, CancellationToken)

- Name: `ApplyTesting(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

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
