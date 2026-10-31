# HostCreatedJointExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/v0.3.1/api/joinery.dovetail.md)
- Type: [HostCreatedJointExtensions](/Dovetail/v0.3.1/api/joinery.dovetail/hostcreatedjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying HostCreated parts.

<a id="applyhostcreated"></a>
<a id="applyhostcreated-thost-idovetailcontext-cancellationtoken"></a>

## ApplyHostCreated(THost, IDovetailContext, CancellationToken)

- Name: `ApplyHostCreated(THost, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered HostCreated parts.

```csharp
public static class HostCreatedJointExtensions
{
    public static ValueTask ApplyHostCreated<THost>(
        this THost host,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `host` (`THost`)
- `context` ([IDovetailContext](/Dovetail/v0.3.1/api/joinery.dovetail/idovetailcontext.md))
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
