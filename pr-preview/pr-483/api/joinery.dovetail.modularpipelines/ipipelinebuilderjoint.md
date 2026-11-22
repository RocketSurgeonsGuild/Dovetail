# IPipelineBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.modularpipelines.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/IPipelineBuilderJoint.cs)
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

- [Register(IDovetailContext, PipelineBuilder)](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint/methods.md#register-idovetailcontext-pipelinebuilder) `abstract` -- Register the PipelineBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
