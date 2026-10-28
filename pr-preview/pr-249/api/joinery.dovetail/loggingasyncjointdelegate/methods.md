# LoggingAsyncJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-249/api/joinery.dovetail.md)
- Type: [LoggingAsyncJointDelegate](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/loggingasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Logging part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iloggingbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, ILoggingBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, ILoggingBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate LoggingAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        ILoggingBuilder builder,
        CancellationToken cancellationToken,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-249/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
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
public delegate LoggingAsyncJointDelegate
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
