# OpenTelemetryConnector

- Kind: `class`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.OpenTelemetry`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers OpenTelemetry services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry;

public class OpenTelemetryConnector
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [OpenTelemetryConnector](/Dovetail/v0.2.3/api/joinery.dovetail.opentelemetry/opentelemetryconnector/constructors.md#constructor)
