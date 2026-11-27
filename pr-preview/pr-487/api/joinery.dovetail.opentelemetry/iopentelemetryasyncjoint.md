# IOpenTelemetryAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-487-merge.Sha.d2fe0860408e23901125d26601527b824db451a0/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryAsyncJoint.cs)
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

- [Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.opentelemetry/iopentelemetryasyncjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the OpenTelemetry part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
