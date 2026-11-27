# SetupJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [SetupJointExtensions](/Dovetail/api/joinery.dovetail/setupjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Setup parts.

<a id="applysetup"></a>
<a id="applysetup-idovetailcontext-cancellationtoken"></a>

## ApplySetup(IDovetailContext, CancellationToken)

- Name: `ApplySetup(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.17b7d26d99ec453dd8fde79e881ab33c1107ffd5/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Setup.g.cs#L51-L52)

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

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
