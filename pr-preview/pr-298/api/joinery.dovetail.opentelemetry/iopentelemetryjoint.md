# IOpenTelemetryJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry.md)
- Version: `0.4.2-pr.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IOpenTelemetryJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IOpenTelemetryBuilder)](/Dovetail/pr-preview/pr-298/api/joinery.dovetail.opentelemetry/iopentelemetryjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder) `abstract` -- Register the OpenTelemetry part.

## See Also

- `Infrastructure.IDovetailJoint`
