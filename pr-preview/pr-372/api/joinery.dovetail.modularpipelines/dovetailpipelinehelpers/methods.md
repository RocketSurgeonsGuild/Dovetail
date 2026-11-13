# DovetailPipelineHelpers Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-372/api/joinery.dovetail.modularpipelines.md)
- Type: [DovetailPipelineHelpers](/Dovetail/pr-preview/pr-372/api/joinery.dovetail.modularpipelines/dovetailpipelinehelpers.md)
- Kind: `Methods`
- Members: `2`

Wires a ModularPipelines `PipelineBuilder` up to a Dovetail `Dovetail.IDovetailContext`. Called by the `ConfigureDovetail` entry points generated for the ModularPipelines host and by [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-372/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md); hidden from IntelliSense via `EditorBrowsableState.Never`.

<a id="apply"></a>
<a id="apply-pipelinebuilder-dovetailcontextbuilder-cancellationtoken"></a>

## Apply(PipelineBuilder, DovetailContextBuilder, CancellationToken)

- Name: `Apply(PipelineBuilder, DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<IDovetailContext>`

Populates the context and applies the configuration, service (and with them logging) and `PipelineBuilder` joints to the builder, without building it.

```csharp
public static class DovetailPipelineHelpers
{
    public static ValueTask<IDovetailContext> Apply(
        PipelineBuilder builder,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`PipelineBuilder`)
  The pipeline builder to configure.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying the joints.

## Returns

`ValueTask<IDovetailContext>` -- The context the joints ran with.

## Exceptions

- `ArgumentNullException` -- `builder` or `contextBuilder` is `null`.

<a id="configure"></a>
<a id="configure-pipelinebuilder-dovetailcontextbuilder-cancellationtoken"></a>

## Configure(PipelineBuilder, DovetailContextBuilder, CancellationToken)

- Name: `Configure(PipelineBuilder, DovetailContextBuilder, CancellationToken)`
- Modifiers: `static`
- Returns: `ValueTask<IPipeline>`

Applies the joints to the builder, builds the pipeline and applies the `HostCreated` joints to it.

```csharp
public static class DovetailPipelineHelpers
{
    public static ValueTask<IPipeline> Configure(
        PipelineBuilder builder,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `builder` (`PipelineBuilder`)
  The pipeline builder to configure.
- `contextBuilder` (`DovetailContextBuilder`)
  The context builder used to create the `Dovetail.IDovetailContext`.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used while applying the joints and building the pipeline.

## Returns

`ValueTask<IPipeline>` -- The built pipeline, ready to run.

## Exceptions

- `ArgumentNullException` -- `builder` or `contextBuilder` is `null`.
