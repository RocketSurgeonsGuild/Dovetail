# ResourceBuilderJointDelegate Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-243/api/joinery.dovetail.opentelemetry.md)
- Type: [ResourceBuilderJointDelegate](/Dovetail/pr-preview/pr-243/api/joinery.dovetail.opentelemetry/resourcebuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the `Resources.ResourceBuilder` part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-resourcebuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, ResourceBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, ResourceBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate ResourceBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        ResourceBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`ResourceBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate ResourceBuilderJointDelegate
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
