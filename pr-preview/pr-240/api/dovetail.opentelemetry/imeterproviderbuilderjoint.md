# IMeterProviderBuilderJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-240/api/dovetail.opentelemetry.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-240/api/dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Metrics.MeterProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public interface IMeterProviderBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, MeterProviderBuilder)](/Dovetail/pr-preview/pr-240/api/dovetail.opentelemetry/imeterproviderbuilderjoint/methods.md#register-idovetailcontext-meterproviderbuilder) `abstract` -- Register the `Metrics.MeterProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
