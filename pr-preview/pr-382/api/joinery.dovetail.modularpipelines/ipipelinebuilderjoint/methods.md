# IPipelineBuilderJoint Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-382/api/joinery.dovetail.modularpipelines.md)
- Type: [IPipelineBuilderJoint](/Dovetail/pr-preview/pr-382/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint.md)
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
