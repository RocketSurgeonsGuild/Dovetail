# SetupAsyncJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-290/api/joinery.dovetail.md)
- Type: [SetupAsyncJointDelegate](/Dovetail/pr-preview/pr-290/api/joinery.dovetail/setupasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Setup part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate SetupAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        CancellationToken cancellationToken,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-290/api/joinery.dovetail/idovetailcontext.md))
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
public delegate SetupAsyncJointDelegate
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
