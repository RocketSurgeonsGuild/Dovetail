# IOpenTelemetryJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.OpenTelemetry/obj/Release/net10.0/IOpenTelemetryJoint.cs)
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

- [Register(IDovetailContext, IOpenTelemetryBuilder)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.opentelemetry/iopentelemetryjoint/methods.md#register-idovetailcontext-iopentelemetrybuilder) `abstract` -- Register the OpenTelemetry part.

## See Also

- `Infrastructure.IDovetailJoint`
