# SetupJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.md)
- Type: [SetupJointExtensions](/Dovetail/pr-preview/pr-492/api/joinery.dovetail/setupjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Setup parts.

<a id="applysetup"></a>
<a id="applysetup-idovetailcontext-cancellationtoken"></a>

## ApplySetup(IDovetailContext, CancellationToken)

- Name: `ApplySetup(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Setup.g.cs#L51-L52)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-492/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
