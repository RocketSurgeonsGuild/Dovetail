# PipelineBuilderAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-365/api/joinery.dovetail.modularpipelines.md)
- Type: [PipelineBuilderAsyncJointDelegate](/Dovetail/pr-preview/pr-365/api/joinery.dovetail.modularpipelines/pipelinebuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the PipelineBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-pipelinebuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, PipelineBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, PipelineBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate PipelineBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        PipelineBuilder builder,
        CancellationToken cancellationToken,
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
- `cancellationToken` (`CancellationToken`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`
- Returns: `ValueTask`

```csharp
public delegate PipelineBuilderAsyncJointDelegate
{
    public virtual ValueTask EndInvoke(
        IAsyncResult result)
    {
        // ...
    }
}
```

## Parameters

- `result` (`IAsyncResult`)
