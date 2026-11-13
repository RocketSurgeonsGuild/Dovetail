# Joinery.Dovetail.ModularPipelines

- Package: `Joinery.Dovetail.ModularPipelines`
- Version: `0.5.4-pr.4`
- Target framework: `net11.0`
- NuGet: [Joinery.Dovetail.ModularPipelines](https://www.nuget.org/packages/Joinery.Dovetail.ModularPipelines)

## Namespaces

### Joinery.Dovetail

- [DovetailPipelineHelpers](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/dovetailpipelinehelpers.md) -- `class` `static` -- Wires a ModularPipelines `PipelineBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Called by the `ConfigureDovetail` entry points generated for the ModularPipelines host and by [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md); hidden from IntelliSense via `EditorBrowsableState.Never`.
- [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md) -- `class` `sealed` -- A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

### Joinery.Dovetail.Joints

- [IPipelineBuilderAsyncJoint](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/ipipelinebuilderasyncjoint.md) -- `interface` -- Async part interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`
- [IPipelineBuilderJoint](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/ipipelinebuilderjoint.md) -- `interface` -- Joint interface for PipelineBuilder. Implements the `Infrastructure.IDovetailJoint`
- [PipelineBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate.md) -- `delegate` -- Async delegate for the PipelineBuilder part.
- [PipelineBuilderHostBuilderExtensions](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying PipelineBuilder parts to a `Dovetail.DovetailContextBuilder`.
- [PipelineBuilderJointDelegate](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate.md) -- `delegate` -- Delegate for the PipelineBuilder part.
- [PipelineBuilderJointExtensions](/Dovetail/pr-preview/pr-391/api/joinery.dovetail.modularpipelines/pipelinebuilderjointextensions.md) -- `class` `static` -- Extension methods for applying PipelineBuilder parts.
