# IPipelineBuilderAsyncJoint

- Kind: `interface`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Version: `0.5.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net11.0`
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

- [Register(IDovetailContext, PipelineBuilder, CancellationToken)](/Dovetail/api/joinery.dovetail.modularpipelines/ipipelinebuilderasyncjoint/methods.md#register-idovetailcontext-pipelinebuilder-cancellationtoken) : `ValueTask` `abstract` -- Register the PipelineBuilder part asynchronously.

## See Also

- `Infrastructure.IDovetailJoint`
