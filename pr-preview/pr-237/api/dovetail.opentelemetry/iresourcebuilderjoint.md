# IResourceBuilderJoint

- Kind: `interface`
- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-237/api/dovetail.opentelemetry.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-237/api/dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/pr-preview/pr-237/api/dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
