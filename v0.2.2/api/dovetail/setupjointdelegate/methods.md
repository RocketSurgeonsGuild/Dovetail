# SetupJointDelegate Methods

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [SetupJointDelegate](/Dovetail/v0.2.2/api/dovetail/setupjointdelegate.md)
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

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
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
