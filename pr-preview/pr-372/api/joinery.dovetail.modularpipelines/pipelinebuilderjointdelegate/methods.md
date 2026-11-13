# PipelineBuilderJointDelegate Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-372/api/joinery.dovetail.modularpipelines.md)
- Type: [PipelineBuilderJointDelegate](/Dovetail/pr-preview/pr-372/api/joinery.dovetail.modularpipelines/pipelinebuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the PipelineBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-pipelinebuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, PipelineBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, PipelineBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate PipelineBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        PipelineBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`PipelineBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate PipelineBuilderJointDelegate
{
    public virtual void EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
