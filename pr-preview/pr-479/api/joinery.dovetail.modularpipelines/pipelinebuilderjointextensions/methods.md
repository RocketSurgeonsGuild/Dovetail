# PipelineBuilderJointExtensions Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.modularpipelines.md)
- Type: [PipelineBuilderJointExtensions](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.modularpipelines/pipelinebuilderjointextensions.md)
- Kind: `Methods`
- Members: `1`

Extension methods for applying PipelineBuilder parts.

<a id="applypipelinebuilder"></a>
<a id="applypipelinebuilder-pipelinebuilder-idovetailcontext-cancellationtoken"></a>

## ApplyPipelineBuilder(PipelineBuilder, IDovetailContext, CancellationToken)

- Name: `ApplyPipelineBuilder(PipelineBuilder, IDovetailContext, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.ca686f4f650b8283955a8e82f7ce758bbabd623f/src/Joinery.Dovetail.ModularPipelines/obj/Release/net11.0/DovetailJoint.PipelineBuilder.g.cs#L51-L52)

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
