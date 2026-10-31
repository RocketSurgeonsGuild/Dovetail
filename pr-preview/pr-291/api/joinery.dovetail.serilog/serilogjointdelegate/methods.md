# SerilogJointDelegate Methods

- Package: [Joinery.Dovetail.Serilog](/Dovetail/pr-preview/pr-291/api/joinery.dovetail.serilog.md)
- Type: [SerilogJointDelegate](/Dovetail/pr-preview/pr-291/api/joinery.dovetail.serilog/serilogjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Serilog part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iservicecollection-loggerconfiguration-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IServiceCollection, LoggerConfiguration, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate SerilogJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IServiceCollection services,
        LoggerConfiguration loggerConfiguration,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `services` (`IServiceCollection`)
- `loggerConfiguration` (`LoggerConfiguration`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate SerilogJointDelegate
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
