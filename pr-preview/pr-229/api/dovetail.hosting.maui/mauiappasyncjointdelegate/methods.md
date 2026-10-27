# MauiAppAsyncJointDelegate Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-229/api/dovetail.hosting.maui.md)
- Type: [MauiAppAsyncJointDelegate](/Dovetail/pr-preview/pr-229/api/dovetail.hosting.maui/mauiappasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the MauiApp part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-mauiapp-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, MauiApp, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, MauiApp, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate MauiAppAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        MauiApp app,
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
- `app` (`MauiApp`)
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
public delegate MauiAppAsyncJointDelegate
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
