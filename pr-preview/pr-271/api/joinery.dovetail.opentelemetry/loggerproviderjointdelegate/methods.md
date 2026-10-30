# LoggerProviderJointDelegate Methods

- Package: [Joinery.Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.opentelemetry.md)
- Type: [LoggerProviderJointDelegate](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.opentelemetry/loggerproviderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the `Logs.LoggerProviderBuilder` part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-loggerproviderbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, LoggerProviderBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, LoggerProviderBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate LoggerProviderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        LoggerProviderBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`LoggerProviderBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate LoggerProviderJointDelegate
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
