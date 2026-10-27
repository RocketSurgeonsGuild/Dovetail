# OpenTelemetryConnector

- Kind: `class`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-229/api/dovetail.opentelemetry.md)
- Version: `0.2.1-pr.18`
- Namespace: `Dovetail.OpenTelemetry`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, `IServiceAsyncJoint`

A joint that registers OpenTelemetry services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.

## Definition

```csharp
namespace Dovetail.OpenTelemetry;

public class OpenTelemetryConnector
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IServiceAsyncJoint
{
    // ...
}
```

## Constructors

- [OpenTelemetryConnector](/Dovetail/pr-preview/pr-229/api/dovetail.opentelemetry/opentelemetryconnector/constructors.md#constructor)
