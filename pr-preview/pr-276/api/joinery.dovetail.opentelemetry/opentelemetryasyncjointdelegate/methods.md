# OpenTelemetryAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.opentelemetry.md)
- Type: [OpenTelemetryAsyncJointDelegate](/Dovetail/pr-preview/pr-276/api/joinery.dovetail.opentelemetry/opentelemetryasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the OpenTelemetry part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iopentelemetrybuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IOpenTelemetryBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate OpenTelemetryAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IOpenTelemetryBuilder builder,
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
- `builder` (`IOpenTelemetryBuilder`)
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
public delegate OpenTelemetryAsyncJointDelegate
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
