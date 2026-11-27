# PipelineBuilderJointExtensions Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-495/api/joinery.dovetail.modularpipelines.md)
- Type: [PipelineBuilderJointExtensions](/Dovetail/pr-preview/pr-495/api/joinery.dovetail.modularpipelines/pipelinebuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying PipelineBuilder parts.

<a id="applypipelinebuilder"></a>
<a id="applypipelinebuilder-pipelinebuilder-idovetailcontext-cancellationtoken"></a>

## ApplyPipelineBuilder(PipelineBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyPipelineBuilder(PipelineBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-495-merge.Sha.fd3d23685f218ca4cae24c50782b49b292f37ef0/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/DovetailJoint.PipelineBuilder.g.cs#L51-L52)

Applies the registered PipelineBuilder parts.

```csharp
public static class PipelineBuilderJointExtensions
{
    public static ValueTask ApplyPipelineBuilder(
        this PipelineBuilder builder,
        IDovetailContext context,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `builder` (`PipelineBuilder`)
- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
