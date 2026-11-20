# OpenTelemetryJointExtensions Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.opentelemetry/joinery.dovetail.joints.opentelemetryjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying OpenTelemetry parts.

<a id="applyopentelemetry"></a>
<a id="applyopentelemetry-iopentelemetrybuilder-idovetailcontext-cancellationtoken"></a>

## ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/DovetailJoint.OpenTelemetry.g.cs#L51-L52)

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
