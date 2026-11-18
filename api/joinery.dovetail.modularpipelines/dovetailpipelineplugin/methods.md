# DovetailPipelinePlugin Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/api/joinery.dovetail.modularpipelines.md)
- Type: [DovetailPipelinePlugin](/Dovetail/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md)
- Kind: `Methods`
- Members: `1`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

<a id="configure"></a>
<a id="configure-pipelinebuilder"></a>

## Configure(PipelineBuilder)

- Name: `Configure(PipelineBuilder)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d6b0f8d96f8417678d91819460b242bed026e72e/src/Joinery.Dovetail.ModularPipelines/DovetailPipelinePlugin.cs#L37-L40)

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
