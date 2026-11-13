# DovetailPipelinePlugin

- Kind: `class`
- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-388/api/joinery.dovetail.modularpipelines.md)
- Version: `0.5.3-pr.3`
- Namespace: `Joinery.Dovetail`
- Target framework: `net11.0`
- Implements: `IModularPipelinesPlugin`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

## Definition

```csharp
namespace Joinery.Dovetail;

public sealed class DovetailPipelinePlugin
    : ModularPipelines.IModularPipelinesPlugin
{
    // ...
}
```

## Remarks

The joints run when the plugin is added, so modules and services the application adds afterwards come after theirs. The `HostCreated` joints do not run, as the plugin never sees the built pipeline; the generated `ConfigureDovetail` entry points run them.

`IModularPipelinesPlugin.Configure` is synchronous, so the plugin blocks on the asynchronous joints. That is safe because Dovetail never resumes on the calling thread's context.

## Constructors

- [DovetailPipelinePlugin(DovetailContextBuilder)](/Dovetail/pr-preview/pr-388/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin/constructors.md#constructor-dovetailcontextbuilder) -- A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

## Properties

- [Context](/Dovetail/pr-preview/pr-388/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin/properties.md#context) : `IDovetailContext?` `get` -- The context the joints ran with, once the plugin has been added to a builder.
- [Name](/Dovetail/pr-preview/pr-388/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin/properties.md#name) : `string` `get` -- Gets the unique name identifying this plugin. A pipeline can add each plugin name once.

## Methods

- [Configure(PipelineBuilder)](/Dovetail/pr-preview/pr-388/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin/methods.md#configure-pipelinebuilder) -- Configures the pipeline builder: register services through `PipelineBuilder.Services`, add modules, event handlers, requirements, or adjust options.
