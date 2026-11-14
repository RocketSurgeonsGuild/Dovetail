# DovetailPipelinePlugin Constructors

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-404/api/joinery.dovetail.modularpipelines.md)
- Type: [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-404/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md)
- Kind: `Constructors`
- Members: `1`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

<a id="constructor"></a>
<a id="constructor-dovetailcontextbuilder"></a>

## DovetailPipelinePlugin(DovetailContextBuilder)

- Name: `Constructor(DovetailContextBuilder)`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

```csharp
public sealed class DovetailPipelinePlugin
{
    public DovetailPipelinePlugin(
        DovetailContextBuilder contextBuilder)
    {
        // ...
    }
}
```

## Parameters

- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.

## Remarks

The joints run when the plugin is added, so modules and services the application adds afterwards come after theirs. The `HostCreated` joints do not run, as the plugin never sees the built pipeline; the generated `ConfigureDovetail` entry points run them.

`IModularPipelinesPlugin.Configure` is synchronous, so the plugin blocks on the asynchronous joints. That is safe because Dovetail never resumes on the calling thread's context.
