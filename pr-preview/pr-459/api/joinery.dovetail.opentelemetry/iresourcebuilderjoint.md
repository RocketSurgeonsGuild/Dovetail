# IResourceBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.OpenTelemetry/Joints/IResourceBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
