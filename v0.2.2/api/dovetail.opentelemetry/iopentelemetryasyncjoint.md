# IOpenTelemetryAsyncJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/v0.2.2/api/dovetail.opentelemetry.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`

Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.Joints;

public interface IOpenTelemetryAsyncJoint
    : Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/iopentelemetryasyncjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the OpenTelemetry part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
