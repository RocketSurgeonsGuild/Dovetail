# IOpenTelemetryAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-beta.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.1a70f4998e74bb5e13c08f9f6cc5e89b61dcd8dc/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for OpenTelemetry. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IOpenTelemetryAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.opentelemetry/iopentelemetryasyncjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the OpenTelemetry part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
