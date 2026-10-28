# IOpenTelemetryJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-236/api/dovetail.opentelemetry.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Joint interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IOpenTelemetryJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IOpenTelemetryBuilder)](/Dovetail/pr-preview/pr-236/api/dovetail.opentelemetry/iopentelemetryjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder) `abstract` -- Register the OpenTelemetry part.

## See Also

- `Infrastructure.IDovetailJoint`
