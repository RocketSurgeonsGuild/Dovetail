# TracerProviderJointDelegate Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-283/api/joinery.dovetail.opentelemetry.md)
- Type: [TracerProviderJointDelegate](/Dovetail/pr-preview/pr-283/api/joinery.dovetail.opentelemetry/tracerproviderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the `Trace.TracerProvider` part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-tracerproviderbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, TracerProviderBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate TracerProviderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        TracerProviderBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`TracerProviderBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate TracerProviderJointDelegate
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
