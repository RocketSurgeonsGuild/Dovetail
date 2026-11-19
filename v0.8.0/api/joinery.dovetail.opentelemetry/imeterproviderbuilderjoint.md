# IMeterProviderBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/v0.8.0/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail.OpenTelemetry/Joints/IMeterProviderBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/v0.8.0/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, MeterProviderBuilder)](/Dovetail/v0.8.0/api/joinery.dovetail.opentelemetry/imeterproviderbuilderjoint/methods.md#register-idovetailcontext-meterproviderbuilder) `abstract` -- Register the `Metrics.MeterProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
