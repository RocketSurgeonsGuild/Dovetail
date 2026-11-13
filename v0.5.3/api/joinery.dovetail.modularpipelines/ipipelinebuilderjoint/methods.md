# IPipelineBuilderJoint Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/v0.5.3/api/joinery.dovetail.modularpipelines.md)
- Type: [IPipelineBuilderJoint](/Dovetail/v0.5.3/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-pipelinebuilder"></a>

## Register(IDovetailContext, PipelineBuilder)

- Name: `Register(IDovetailContext, PipelineBuilder)`
- Modifiers: `abstract`

Register the PipelineBuilder part.

```csharp
public interface IPipelineBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        PipelineBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`PipelineBuilder`)
