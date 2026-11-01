# MeterProviderJointDelegate Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.opentelemetry.md)
- Type: [MeterProviderJointDelegate](/Dovetail/pr-preview/pr-297/api/joinery.dovetail.opentelemetry/meterproviderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the `Metrics.MeterProvider` part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-meterproviderbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, MeterProviderBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate MeterProviderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        MeterProviderBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`MeterProviderBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate MeterProviderJointDelegate
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
