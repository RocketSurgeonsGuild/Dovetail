# LoggingJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-260/api/joinery.dovetail.md)
- Type: [LoggingJointDelegate](/Dovetail/pr-preview/pr-260/api/joinery.dovetail/loggingjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Logging part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-iloggingbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, ILoggingBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate LoggingJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        ILoggingBuilder builder,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-260/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate LoggingJointDelegate
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
