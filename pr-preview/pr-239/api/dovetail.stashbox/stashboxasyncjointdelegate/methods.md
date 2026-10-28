# StashboxAsyncJointDelegate Methods

- Package: [Dovetail.Stashbox](/Dovetail/pr-preview/pr-239/api/dovetail.stashbox.md)
- Type: [StashboxAsyncJointDelegate](/Dovetail/pr-preview/pr-239/api/dovetail.stashbox/stashboxasyncjointdelegate.md)
- Kind: `Methods`
- Members: `2`

Async delegate for the Stashbox part.

<a id="begininvoke"></a>
<a id="begininvoke-idovetailcontext-istashboxcontainer-cancellationtoken-asynccallback-object"></a>

## BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)

- Name: `BeginInvoke(IDovetailContext, IStashboxContainer, CancellationToken, AsyncCallback, object)`
- Modifiers: `virtual`
- Returns: `IAsyncResult`

```csharp
public delegate StashboxAsyncJointDelegate
{
    public virtual IAsyncResult BeginInvoke(
        IDovetailContext context,
        IStashboxContainer container,
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
- `container` (`IStashboxContainer`)
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
public delegate StashboxAsyncJointDelegate
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
