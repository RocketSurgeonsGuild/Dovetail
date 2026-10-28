# ITracerProviderJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/v0.2.2/api/dovetail.opentelemetry.md)
- Version: `0.2.2`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/v0.2.2/api/dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public interface ITracerProviderJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TracerProviderBuilder)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/itracerproviderjoint/methods.md#register-idovetailcontext-tracerproviderbuilder) `abstract` -- Register the `Trace.TracerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
