# IMeterProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.1faa3d8c1fb998da38c062158c5a17b8390b25fa/src/Joinery.Dovetail.OpenTelemetry/Joints/IMeterProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, MeterProviderBuilder)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint/methods.md#register-idovetailcontext-meterproviderbuilder) `abstract` -- Register the `Metrics.MeterProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
