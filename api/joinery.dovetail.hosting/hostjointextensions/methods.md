# HostJointExtensions Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/api/joinery.dovetail.hosting.md)
- Type: [HostJointExtensions](/Dovetail/api/joinery.dovetail.hosting/hostjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying Host parts.

<a id="applyhost"></a>
<a id="applyhost-thost-idovetailcontext-cancellationtoken"></a>

## ApplyHost(THost, IDovetailContext, CancellationToken)

- Name: `ApplyHost(THost, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Applies the registered Host parts.

```csharp
public static class HostJointExtensions
{
    public static ValueTask ApplyHost<THost>(
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
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
