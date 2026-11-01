# IResourceBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.opentelemetry.md)
- Version: `0.4.2-pr.5`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.OpenTelemetry.Joints;

public interface IResourceBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint,
      Joinery.Dovetail.Joints.IOpenTelemetryJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
