# StashboxJointDelegate Methods

- Package: [Dovetail.Stashbox](/Dovetail/v0.2.2/api/dovetail.stashbox.md)
- Type: [StashboxJointDelegate](/Dovetail/v0.2.2/api/dovetail.stashbox/stashboxjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Delegate for the Stashbox part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-istashboxcontainer-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IStashboxContainer, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IStashboxContainer, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate StashboxJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IStashboxContainer container,
        AsyncCallback callback,
        object @object)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `container` (`IStashboxContainer`)
- `callback` (`AsyncCallback`)
- `object` (`object`)

<a id="endinvoke"></a>
<a id="endinvoke-iasyncresult"></a>

## EndInvoke(IAsyncResult)

- Name: `EndInvoke(IAsyncResult)`
- Modifiers: `virtual`

```csharp
public delegate StashboxJointDelegate
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
