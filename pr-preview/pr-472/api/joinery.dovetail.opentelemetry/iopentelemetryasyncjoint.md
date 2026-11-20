# IOpenTelemetryAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-472-merge.Sha.e03dcea79ebd9fb048ebef2a394530597c6503d0/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryAsyncJoint.cs)
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

- [Register(IDovetailContext, IOpenTelemetryBuilder, CancellationToken)](/Dovetail/pr-preview/pr-472/api/joinery.dovetail.opentelemetry/iopentelemetryasyncjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the OpenTelemetry part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
