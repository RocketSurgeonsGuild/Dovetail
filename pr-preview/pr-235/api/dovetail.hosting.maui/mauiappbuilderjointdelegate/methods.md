# MauiAppBuilderJointDelegate Methods

- Package: [Dovetail.Hosting.Maui](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui.md)
- Type: [MauiAppBuilderJointDelegate](/Dovetail/pr-preview/pr-235/api/dovetail.hosting.maui/mauiappbuilderjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the MauiAppBuilder part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-mauiappbuilder-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, MauiAppBuilder, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, MauiAppBuilder, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate MauiAppBuilderJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        MauiAppBuilder builder,
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
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate MauiAppBuilderJointDelegate
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
