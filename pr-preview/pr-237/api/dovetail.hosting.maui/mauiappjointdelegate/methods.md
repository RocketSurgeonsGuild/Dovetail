# MauiAppJointDelegate Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui.md)
- Type: [MauiAppJointDelegate](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.maui/mauiappjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the MauiApp part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-mauiapp-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, MauiApp, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, MauiApp, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate MauiAppJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        MauiApp app,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate MauiAppJointDelegate
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
