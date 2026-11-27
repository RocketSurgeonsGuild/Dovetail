# HostApplicationJointExtensions Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.hosting.md)
- Type: [HostApplicationJointExtensions](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.hosting/hostapplicationjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying HostApplication parts.

<a id="applyhostapplication"></a>
<a id="applyhostapplication-tbuilder-idovetailcontext-cancellationtoken"></a>

## ApplyHostApplication(TBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyHostApplication(TBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/DovetailJoint.HostApplication.g.cs#L51-L52)

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
