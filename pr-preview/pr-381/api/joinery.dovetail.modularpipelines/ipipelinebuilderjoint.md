# IPipelineBuilderJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-381/api/joinery.dovetail.modularpipelines.md)
- Version: `0.4.6-pr.9`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
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

- [Register(IDovetailContext, PipelineBuilder)](/Dovetail/pr-preview/pr-381/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint/methods.md#register-idovetailcontext-pipelinebuilder) `abstract` -- Register the PipelineBuilder part.

## See Also

- `Infrastructure.IDovetailJoint`
