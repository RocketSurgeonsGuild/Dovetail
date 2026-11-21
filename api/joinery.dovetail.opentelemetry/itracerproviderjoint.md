# ITracerProviderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-beta.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail.OpenTelemetry/Joints/ITracerProviderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Trace.TracerProviderBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public interface ITracerProviderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, TracerProviderBuilder)](/Dovetail/api/joinery.dovetail.opentelemetry/itracerproviderjoint/methods.md#register-idovetailcontext-tracerproviderbuilder) `abstract` -- Register the `Trace.TracerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
