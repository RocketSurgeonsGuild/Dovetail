# IOpenTelemetryJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryJoint.cs)
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

- [Register(IDovetailContext, IOpenTelemetryBuilder)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.opentelemetry/iopentelemetryjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder) `abstract` -- Register the OpenTelemetry part.

## See Also

- `Infrastructure.IDovetailJoint`
