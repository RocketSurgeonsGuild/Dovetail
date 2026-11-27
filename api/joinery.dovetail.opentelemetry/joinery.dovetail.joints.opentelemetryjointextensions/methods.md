# OpenTelemetryJointExtensions Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryJointExtensions](/Dovetail/api/joinery.dovetail.opentelemetry/joinery.dovetail.joints.opentelemetryjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying OpenTelemetry parts.

<a id="applyopentelemetry"></a>
<a id="applyopentelemetry-iopentelemetrybuilder-idovetailcontext-cancellationtoken"></a>

## ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d3d86de99d09b379bd09cfaa27e848e7aa21ab62/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/DovetailJoint.OpenTelemetry.g.cs#L51-L52)

Applies the registered OpenTelemetry parts.

```csharp
public static class OpenTelemetryJointExtensions
{
    public static ValueTask ApplyOpenTelemetry(
        this IOpenTelemetryBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`IOpenTelemetryBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
