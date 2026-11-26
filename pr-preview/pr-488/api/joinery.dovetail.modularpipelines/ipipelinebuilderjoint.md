# IPipelineBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/IPipelineBuilderJoint.cs)
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

- [Register(IDovetailContext, PipelineBuilder)](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint/methods.md#register-idovetailcontext-pipelinebuilder) `abstract` -- Register the PipelineBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
