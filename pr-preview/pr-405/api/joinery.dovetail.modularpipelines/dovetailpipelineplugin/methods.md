# DovetailPipelinePlugin Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-405/api/joinery.dovetail.modularpipelines.md)
- Type: [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-405/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md)
- Kind: `Methods`
- Members: `1`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

<a id="configure"></a>
<a id="configure-pipelinebuilder"></a>

## Configure(PipelineBuilder)

- Name: `Configure(PipelineBuilder)`

Configures the pipeline builder: register services through `PipelineBuilder.Services`, add modules, event handlers, requirements, or adjust options.

```csharp
public sealed class DovetailPipelinePlugin
{
    public void Configure(
        PipelineBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`PipelineBuilder`)
  The pipeline builder to configure.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
