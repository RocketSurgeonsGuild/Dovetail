# HostApplicationJointExtensions Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-484/api/joinery.dovetail.hosting.md)
- Type: [HostApplicationJointExtensions](/Dovetail/pr-preview/pr-484/api/joinery.dovetail.hosting/hostapplicationjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying HostApplication parts.

<a id="applyhostapplication"></a>
<a id="applyhostapplication-tbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyHostApplication(TBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyHostApplication(TBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/DovetailJoint.HostApplication.g.cs#L51-L52)

Applies the registered HostApplication parts.

```csharp
public static class HostApplicationJointExtensions
{
    public static ValueTask ApplyHostApplication<TBuilder>(
        this TBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`TBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
