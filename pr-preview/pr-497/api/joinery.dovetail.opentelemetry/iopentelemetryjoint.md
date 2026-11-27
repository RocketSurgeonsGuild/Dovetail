# IOpenTelemetryJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryJoint.cs)
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

- [Register(IDovetailContext, IOpenTelemetryBuilder)](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.opentelemetry/iopentelemetryjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder) `abstract` -- Register the OpenTelemetry part.

## See Also

- `Infrastructure.IDovetailJoint`
