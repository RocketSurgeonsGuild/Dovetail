# IResourceBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.13`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-447-merge.Sha.07e64d25a30d87fc48248bbdc4924e0013be133c/src/Joinery.Dovetail.OpenTelemetry/Joints/IResourceBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
