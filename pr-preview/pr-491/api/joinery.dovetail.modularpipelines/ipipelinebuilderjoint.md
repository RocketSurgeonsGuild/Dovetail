# IPipelineBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/IPipelineBuilderJoint.cs)
- Implements: `IDovetailJoint`

Joint interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface IPipelineBuilderJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, PipelineBuilder)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint/methods.md#register-idovetailcontext-pipelinebuilder) `abstract` -- Register the PipelineBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
