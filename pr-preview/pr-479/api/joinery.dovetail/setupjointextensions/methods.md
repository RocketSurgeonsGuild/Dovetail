# SetupJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Type: [SetupJointExtensions](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/setupjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Setup parts.

<a id="applysetup"></a>
<a id="applysetup-idovetailcontext-cancellationtoken"></a>

## ApplySetup(IDovetailContext, CancellationToken)

- Name: `ApplySetup(IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/obj/Release/net10.0/DovetailJoint.Setup.g.cs#L51-L52)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
