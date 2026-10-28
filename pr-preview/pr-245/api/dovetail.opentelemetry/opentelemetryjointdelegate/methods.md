# OpenTelemetryJointDelegate Methods

- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-245/api/dovetail.opentelemetry.md)
- Type: [OpenTelemetryJointDelegate](/Dovetail/pr-preview/pr-245/api/dovetail.opentelemetry/opentelemetryjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the OpenTelemetry part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iopentelemetrybuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate OpenTelemetryJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IOpenTelemetryBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IOpenTelemetryBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate OpenTelemetryJointDelegate
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
