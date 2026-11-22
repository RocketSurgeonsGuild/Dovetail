# OpenTelemetryJointExtensions Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryJointExtensions](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.opentelemetry/joinery.dovetail.joints.opentelemetryjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying OpenTelemetry parts.

<a id="applyopentelemetry"></a>
<a id="applyopentelemetry-iopentelemetrybuilder-idovetailcontext-cancellationtoken"></a>

## ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyOpenTelemetry(IOpenTelemetryBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/DovetailJoint.OpenTelemetry.g.cs#L51-L52)

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
