# SetupJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-489/api/joinery.dovetail.md)
- Type: [SetupJointExtensions](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/setupjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Setup parts.

<a id="applysetup"></a>
<a id="applysetup-idovetailcontext-cancellationtoken"></a>

## ApplySetup(IDovetailContext, CancellationToken)

- Name: `ApplySetup(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-489-merge.Sha.e15aa1d5a7d1f73d8e63e3f87bcb11b8c9c8ec93/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Setup.g.cs#L51-L52)

Applies the registered Setup parts.

```csharp
public static class SetupJointExtensions
{
    public static ValueTask ApplySetup(
        this IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-489/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
