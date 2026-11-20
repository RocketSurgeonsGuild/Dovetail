# ITracerProviderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-471-merge.Sha.bf40892310bcd6ceeb21a319fcdbc6a1dfabd05f/src/Joinery.Dovetail.OpenTelemetry/Joints/ITracerProviderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, TracerProviderBuilder)](/Dovetail/pr-preview/pr-471/api/joinery.dovetail.opentelemetry/itracerproviderjoint/methods.md#register-idovetailcontext-tracerproviderbuilder) `abstract` -- Register the `Trace.TracerProviderBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
