# IMeterProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.23`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-464-merge.Sha.2eea5d87d2b172b966d5fac389220a20bb8003d0/src/Joinery.Dovetail.OpenTelemetry/Joints/IMeterProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public interface IMeterProviderBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MeterProviderBuilder)](/Dovetail/pr-preview/pr-464/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint/methods.md#register-idovetailcontext-meterproviderbuilder) `abstract` -- Register the `Metrics.MeterProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
