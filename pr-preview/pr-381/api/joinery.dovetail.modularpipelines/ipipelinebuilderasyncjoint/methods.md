# IPipelineBuilderAsyncJoint Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-381/api/joinery.dovetail.modularpipelines.md)
- Type: [IPipelineBuilderAsyncJoint](/Dovetail/pr-preview/pr-381/api/joinery.dovetail.modularpipelines/ipipelinebuilderasyncjoint.md)
- Kind: `Methods`
- Members: `1`

Async part interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-pipelinebuilder-cancellationtoken"></a>

## Register(IDovetailContext, PipelineBuilder, CancellationToken)

- Name: `Register(IDovetailContext, PipelineBuilder, CancellationToken)`
- Modifiers: `abstract`
- Returns: `ValueTask`

Register the PipelineBuilder part asynchronously.

```csharp
public interface IPipelineBuilderAsyncJoint
{
    public abstract ValueTask Register(
        IDovetailContext context,
        PipelineBuilder builder,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`PipelineBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`
