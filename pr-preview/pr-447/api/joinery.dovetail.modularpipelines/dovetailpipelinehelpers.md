# DovetailPipelineHelpers

- Kind: `class`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.modularpipelines.md)
- Version: `0.7.2-pr.13`
- Namespace: `Joinery.Dovetail`
- Target framework: `net11.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-447-merge.Sha.07e64d25a30d87fc48248bbdc4924e0013be133c/src/Joinery.Dovetail.ModularPipelines/DovetailPipelineHelpers.cs)

Wires a ModularPipelines `PipelineBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Called by the `ConfigureDovetail` entry points generated for the ModularPipelines host and by [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md); hidden from IntelliSense via `EditorBrowsableState.Never`.

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailPipelineHelpers
{
    // ...
}
```

## Methods

- [Apply(PipelineBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.modularpipelines/dovetailpipelinehelpers/methods.md#apply-pipelinebuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IDovetailContext>` `static` -- Populates the context and applies the configuration, service (and with them logging) and `PipelineBuilder` joints to the builder, without building it.
- [Configure(PipelineBuilder, DovetailContextBuilder, CancellationToken)](/Dovetail/pr-preview/pr-447/api/joinery.dovetail.modularpipelines/dovetailpipelinehelpers/methods.md#configure-pipelinebuilder-dovetailcontextbuilder-cancellationtoken) : `ValueTask<IPipeline>` `static` -- Applies the joints to the builder, builds the pipeline and applies the `HostCreated` joints to it.
