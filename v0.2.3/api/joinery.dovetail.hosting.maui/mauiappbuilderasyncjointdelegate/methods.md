# MauiAppBuilderAsyncJointDelegate Methods

- Package: [Joinery.Dovetail.Hosting.Maui](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui.md)
- Type: [MauiAppBuilderAsyncJointDelegate](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.maui/mauiappbuilderasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the MauiAppBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-mauiappbuilder-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, MauiAppBuilder, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, MauiAppBuilder, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate MauiAppBuilderAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        MauiAppBuilder builder,
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
- `builder` (`MauiAppBuilder`)
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
public delegate MauiAppBuilderAsyncJointDelegate
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
