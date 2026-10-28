# IResourceBuilderJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/v0.2.2/api/dovetail.opentelemetry.md)
- Version: `0.2.2`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/v0.2.2/api/dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Dovetail.OpenTelemetry.Joints;

public interface IResourceBuilderJoint
    : Dovetail.Infrastructure.IDovetailJoint,
      Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/v0.2.2/api/dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
