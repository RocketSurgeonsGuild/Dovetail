# IPipelineBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.1-pr.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-465-merge.Sha.4a32b50903cfcbdb71b84de18a5fb0fb879970b7/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/IPipelineBuilderAsyncJoint.cs)
- Implements: `IDovetailJoint`

Async part interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IPipelineBuilderAsyncJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, PipelineBuilder, CancellationToken)](/Dovetail/pr-preview/pr-465/api/joinery.dovetail.modularpipelines/ipipelinebuilderasyncjoint/methods.md#register-idovetailcontext-pipelinebuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the PipelineBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
