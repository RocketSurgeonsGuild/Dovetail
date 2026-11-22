# IResourceBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.opentelemetry.md)
- Version: `0.8.2-pr.10`
- Namespace: `Joinery.Dovetail.OpenTelemetry.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-485-merge.Sha.e8a390e6a30398b83014f44c87a7311afd4ff030/src/Joinery.Dovetail.OpenTelemetry/Joints/IResourceBuilderJoint.cs)
- Implements: `IDovetailJoint`, [IOpenTelemetryJoint](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.opentelemetry/iopentelemetryjoint.md)

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

- [Register(IDovetailContext, ResourceBuilder)](/Dovetail/pr-preview/pr-485/api/joinery.dovetail.opentelemetry/iresourcebuilderjoint/methods.md#register-idovetailcontext-resourcebuilder) `abstract` -- Register the `Resources.ResourceBuilder` part.

## See Also

- `Infrastructure.IDovetailJoint`
