# SetupJointDelegate Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-296/api/joinery.dovetail.md)
- Type: [SetupJointDelegate](/Dovetail/pr-preview/pr-296/api/joinery.dovetail/setupjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Setup part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate SetupJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-296/api/joinery.dovetail/idovetailcontext.md))
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate SetupJointDelegate
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
